using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Newtonsoft.Json;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using ScottPlot;
using System.Drawing;
using Color = System.Drawing.Color;

namespace CALORY
{
    public partial class Diary : Window
    {
        public List<Repast> productsBreakfast = new List<Repast>();
        public List<Repast> productsLunch = new List<Repast>();
        public List<Repast> productsDiner = new List<Repast>();
        public List<Repast> productsBase = new List<Repast>();
        public static Diary? instance;
        private string  Login;
     
        public Diary(string login)
        {
            Login = login;
            InitializeComponent();
            instance = this;
            CalendarPiker.SelectedDate = DateTime.Now;
            CalendarPiker.DisplayDateStart = DateTime.Today.AddDays(-7);
            DataOfBirth.DisplayDateEnd = DateTime.Now.AddYears(-14);
            CalendarPiker.DisplayDateEnd = DateTime.Now;
            using (StreamReader GroceryList = new StreamReader("..\\..\\..\\products.txt"))
            {
                var jsr = new JsonTextReader(GroceryList);
                productsBase = new JsonSerializer().Deserialize<List<Repast>>(jsr);
            }
            ComboBoxSearch.ItemsSource = productsBase;
        }
        private void ComboBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)e.OriginalSource;
            if (tb.SelectionStart == 0 && ComboBoxSearch.SelectedItem == null)
            {
                ComboBoxSearch.IsDropDownOpen = false; // Если сбросили текст и элемент не выбран, сбросить фокус выпадающего списка
                tb.Select(tb.SelectionStart + tb.SelectionLength, 0);
            }
            if (ComboBoxSearch.Text.Length > 1)
            {
                if (tb.SelectionStart != 0)
                {
                    ComboBoxSearch.SelectedItem = null; // Если набирается текст сбросить выбраный элемент
                }

                if (ComboBoxSearch.SelectedItem == null && ComboBoxSearch.Text.Length > 0)
                {
                    // Если элемент не выбран менять фильтр
                    CollectionView cv = (CollectionView)CollectionViewSource.GetDefaultView(ComboBoxSearch.ItemsSource);
                    cv.Filter = s => s.ToString().IndexOf(ComboBoxSearch.Text, StringComparison.CurrentCultureIgnoreCase) >= 0;
                }
                ComboBoxSearch.IsDropDownOpen = true;
                tb.Select(tb.SelectionStart + tb.SelectionLength, 0);
            }
        }
        #region Добавление еды
        public void Add(string timeMeal)
        {
            bool b = productsBreakfast.Exists(x => x.name == ComboBoxSearch.SelectedItem.ToString());
            bool d = productsDiner.Exists(x => x.name == ComboBoxSearch.SelectedItem.ToString());
            bool l = productsLunch.Exists(x => x.name == ComboBoxSearch.SelectedItem.ToString());
            if (ComboBoxSearch.SelectedItem == null)
                MessageBox.Show("Продукт не выбран", "Ошибка добавления");
            else if ((timeMeal == "Breakfast" && b) || (timeMeal == "Diner" && d) || (timeMeal == "Lunch" && l))
            {
                MessageBox.Show("Данный продукт уже выбран, если вы хотите изменить граммы - удалите старый продукт из списка", "Выбор продукта");
            }
            else
            {
                AddWindow SelectedProductsWindow = new AddWindow(timeMeal, Login);
                SelectedProductsWindow.ShowDialog();
            }
        }
        private void AddBreakfast_Click(object sender, RoutedEventArgs e){          
            Add("Breakfast");}
        
        private void AddLunch_Click(object sender, RoutedEventArgs e){
            Add("Lunch"); }

        private void AddDinner_Click(object sender, RoutedEventArgs e){
            Add("Diner"); }
        #endregion
        private void CalendarPiker_Changed(object sender, SelectionChangedEventArgs e)
        {
            listBoxBreakfast.Items.Clear();
            listBoxLunch.Items.Clear();
            listBoxDiner.Items.Clear();
            productsBreakfast.Clear();
            productsLunch.Clear();
            productsDiner.Clear();
            var calendarDay = CalendarPiker.SelectedDate;
            using (var db = new ApplicationContext())
            {
                foreach (var item in db.Meal.Where(x => x.loginUser == Login && x.day == calendarDay && x.ration == "Breakfast"))
                {
                    listBoxBreakfast.Items.Add(item.ToStringFull());
                    productsBreakfast.Add(item);
                }

                foreach (var item in db.Meal.Where(x => x.loginUser == Login && x.day == calendarDay && x.ration == "Diner"))
                {
                    listBoxDiner.Items.Add(item.ToStringFull());
                    productsDiner.Add(item);
                }

                foreach (var item in db.Meal.Where(x => x.loginUser == Login && x.day == calendarDay && x.ration == "Lunch"))
                {
                    listBoxLunch.Items.Add(item.ToStringFull());
                    productsLunch.Add(item);
                }
                //var user = db.Users.FirstOrDefault(x => x.login == Login);
                //rskGoalTextBox.Text = user.rsk.ToString();
            }
        }

        private void buttonDeleteBreakfast_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxBreakfast.SelectedItems.Count == 0)
                MessageBox.Show("Выберите продукт для удаления", "Ошибка удаления");
            else 
            {
                using (var db = new ApplicationContext())
                {
                    var deleting = db.Meal.Where(x => x.day == CalendarPiker.SelectedDate && x.loginUser == Login && x.name == productsBreakfast[listBoxBreakfast.SelectedIndex].name);
                    db.Meal.RemoveRange(deleting);
                    db.SaveChanges();
                }
                productsBreakfast.RemoveAt(listBoxBreakfast.SelectedIndex);
                listBoxBreakfast.Items.RemoveAt(listBoxBreakfast.SelectedIndex);
                
            }
        }
        private void buttonDeleteLunch_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxLunch.SelectedItems.Count == 0)
                MessageBox.Show("Выберите продукт для удаления", "Ошибка удаления");
            else
            {
                using (var db = new ApplicationContext())
                {
                    var deleting = db.Meal.Where(x => x.day == CalendarPiker.SelectedDate && x.loginUser == Login && x.name == productsLunch[listBoxLunch.SelectedIndex].name);
                    db.Meal.RemoveRange(deleting);
                    db.SaveChanges();
                }
                productsLunch.RemoveAt(listBoxLunch.SelectedIndex);
                listBoxLunch.Items.RemoveAt(listBoxLunch.SelectedIndex);
            }
        }
        private void buttonDeleteDiner_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxDiner.SelectedItems.Count == 0)
                MessageBox.Show("Выберите продукт для удаления", "Ошибка удаления");
            else 
            {
                using (var db = new ApplicationContext())
                {
                    var deleting = db.Meal.Where(x => x.day == CalendarPiker.SelectedDate && x.loginUser == Login && x.name == productsDiner[listBoxDiner.SelectedIndex].name);
                    db.Meal.RemoveRange(deleting);
                    db.SaveChanges();
                }
                productsDiner.RemoveAt(listBoxDiner.SelectedIndex);
                listBoxDiner.Items.RemoveAt(listBoxDiner.SelectedIndex); 
            }
        }
        private void ExitButtonDiary_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }

        private void TabControlDiary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                switch (TabControlDiary.SelectedIndex)
                {
                    case 0:
                        using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
                        {
                            using (var db = new ApplicationContext())
                            {
                                var user = db.Users.FirstOrDefault(x => x.login == Login);
                                rskGoalTextBox.Text = user.rsk.ToString();
                            }
                        }
                        break;
                    case 1:
                        using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
                        {
                            ChangeProfileButton.Content = "Изменить параметры";
                            using (var db = new ApplicationContext())
                            {
                                var user = db.Users.FirstOrDefault(x => x.login == Login);
                                textBoxName.Text = user.name;
                                textBoxHeight.Text = user.growth.ToString();
                                textBoxWeight.Text = user.weight.ToString();
                                DataOfBirth.SelectedDate = user.Birth;
                                if (user.activity == 1) ActivityComboBox.Text = "Сидячий";
                                if (user.activity == 2) ActivityComboBox.Text = "Малоактивный";
                                if (user.activity == 3) ActivityComboBox.Text = "Активный";
                                if (user.activity == 4) ActivityComboBox.Text = "Очень активный";
                                if (user.goal == 1) GoalComboBox.Text = "Набор веса";
                                if (user.goal == 2) GoalComboBox.Text = "Удержание";
                                if (user.goal == 3) GoalComboBox.Text = "Похудение";
                                if (user.male == 1) GenderComboBox.Text = "Мужской";
                                if (user.male == 0) GenderComboBox.Text = "Женский";
                            }
                        }
                        break;
                    case 2:
                        #region Отчет
                        using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
                        {
                            using (var db = new ApplicationContext())
                            {
                                var user = db.Users.FirstOrDefault(x => x.login == Login);
                                Goal.Text = user.rsk.ToString() + " ккал";
                            }
                            Chart.Plot.Clear();
                            Donut.Plot.Clear();
                            DateTime thisDay = DateTime.Today; ;
                            double EatenKkal = 0;
                            double EatenUgl = 0;
                            double EatenFats = 0;
                            double EatenBel = 0;
                            double[] values = { 0, 0, 0, 0, 0, 0, 0 };
                            using (var db = new ApplicationContext())
                            {
                                for (int i = 0; i < 7; i++)
                                {
                                    foreach (var item in db.Meal.Where(x => x.loginUser == Login && x.day == thisDay.AddDays(-i).Date))
                                        EatenKkal += item.kkal;
                                    values[6 - i] = Math.Round(EatenKkal);
                                    EatenKkal = 0;
                                }
                                foreach (var item in db.Meal.Where(x => x.loginUser == Login && x.day == thisDay.Date))
                                {
                                    EatenUgl += item.ugl;
                                    EatenFats += item.fats;
                                    EatenBel += item.bel;
                                }
                            }
                            double[] positions = { 0, 1, 2, 3, 4, 5, 6 };
                            string[] labels = { thisDay.AddDays(-6).ToString("d"), thisDay.AddDays(-5).ToString("d"), thisDay.AddDays(-4).ToString("d"), thisDay.AddDays(-3).ToString("d"), thisDay.AddDays(-2).ToString("d"), thisDay.AddDays(-1).ToString("d"), thisDay.ToString("d") };
                            Chart.Plot.AddBar(values, positions);
                            var bar = Chart.Plot.AddBar(values);
                            bar.ShowValuesAboveBars = true;
                            Chart.Plot.XTicks(positions, labels);
                            Chart.Plot.SetAxisLimits(yMin: 0);
                            Chart.Plot.Style(ScottPlot.Style.Control);
                            bar.FillColor = Color.FromArgb(179, 145, 212);
                            Chart.Plot.Grid(lineStyle: LineStyle.Dot);
                            if (EatenUgl == 0 && EatenFats == 0 && EatenBel == 0)
                            {
                                Donut.Visibility = Visibility.Hidden;
                                ImageDonut.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                Donut.Visibility = Visibility.Visible;
                                ImageDonut.Visibility = Visibility.Hidden;
                            }
                            Donut.Visibility = Visibility;
                            double[] values1 = { EatenUgl, EatenFats, EatenBel };
                            Donut.Plot.Style(ScottPlot.Style.Control);
                            var pie = Donut.Plot.AddPie(values1);
                            pie.Explode = true;
                            pie.DonutSize = .4;
                            Color color1 = Color.FromArgb(179, 145, 212);
                            Color color2 = Color.FromArgb(217, 125, 161);
                            Color color3 = Color.FromArgb(190, 29, 86);
                            pie.SliceFillColors = new Color[] { color1, color2, color3 };
                            textBoxUgl.Text = Math.Round(EatenUgl, 2).ToString();
                            textBoxFats.Text = Math.Round(EatenFats, 2).ToString();
                            textBoxBel.Text = Math.Round(EatenBel, 2).ToString();
                            LabelDonutDay.Content = thisDay.ToString("d");
                            Donut.Plot.Legend();
                            Chart.Refresh();
                            Donut.Refresh();
                        }
                        #endregion
                        break;
                    default:
                        break;
                }
            }
        }

        private protected void ChangeProfileButton_Click(object sender, RoutedEventArgs e)
        {
            if (ChangeProfileButton.Content == "Изменить параметры")
            {
                ChangeProfileButton.Content = "Сохранить";
                textBoxName.IsReadOnly = false;
                textBoxHeight.IsReadOnly = false;
                textBoxWeight.IsReadOnly = false;
                ActivityComboBox.IsEnabled = true;
                GoalComboBox.IsEnabled = true;
                GenderComboBox.IsEnabled = true;
                RectangleCalendar.Visibility = Visibility.Hidden;
            }
            else
            {
                if (Convert.ToInt32(textBoxWeight.Text) < 30 || Convert.ToInt32(textBoxWeight.Text) > 250)
                {
                    MessageBox.Show("Допустимый вес от 30 до 250 кг", "Некорректно введен вес");
                    return;
                }
                if (Convert.ToInt32(textBoxHeight.Text) < 100 || Convert.ToInt32(textBoxHeight.Text) > 250)
                {
                    MessageBox.Show("Допустимый рост от 100 до 250 см", "Некорректно введен рост");
                    return;
                }
                if (!Registration.Verification(textBoxName))
                {
                    MessageBox.Show("Имя должно соответствовать следующим требованиям:\n 1.Длина не менее 3 символов \n 2.Cодержит только латинские буквы и цифры", "Некорректно введено имя");
                    return;
                }
                ChangeProfileButton.Content = "Изменить параметры";
                textBoxName.IsReadOnly = true;
                textBoxHeight.IsReadOnly = true;
                textBoxWeight.IsReadOnly = true;
                ActivityComboBox.IsEnabled = false;
                GoalComboBox.IsEnabled = false;
                GenderComboBox.IsEnabled = false;
                RectangleCalendar.Visibility = Visibility.Visible;
                bool _gender = false;
                int _activity = 1;
                int _goal = 1;
                if (ActivityComboBox.Text == "Сидячий") _activity = 1;
                if (ActivityComboBox.Text == "Малоактивный") _activity = 2;
                if (ActivityComboBox.Text == "Активный") _activity = 3;
                if (ActivityComboBox.Text == "Очень активный") _activity = 4;
                if (GoalComboBox.Text == "Набор веса") _goal = 1;
                if (GoalComboBox.Text == "Удержание") _goal = 2;
                if (GoalComboBox.Text == "Похудение") _goal = 3;
                if (GenderComboBox.Text == "Мужской") _gender = true;
                if (GenderComboBox.Text == "Женский") _gender = false;
                using (var db = new ApplicationContext())
                {
                    var users = db.Users.FirstOrDefault(x => x.login == Login);
                    users.name = textBoxName.Text;
                    users.growth = Convert.ToByte(textBoxHeight.Text);
                    users.weight = Convert.ToByte(textBoxWeight.Text);
                    users.activity = Convert.ToByte(_activity);
                    users.goal = Convert.ToByte(_goal);
                    users.male = (byte)((_gender) ? 1 : 0);
                    users.Birth = DataOfBirth.SelectedDate.Value;
                    users.age = (byte)(DateTime.Now.Year - DataOfBirth.SelectedDate.Value.Year);
                    users.rsk = InfoUser.CalculateRSK(Convert.ToDouble(textBoxWeight.Text), Convert.ToDouble(DateTime.Now.Year - DataOfBirth.SelectedDate.Value.Year), Convert.ToDouble(textBoxHeight.Text), _gender, _activity, _goal);
                    db.SaveChanges();
                }
            }
        }

        private void textBoxHeight_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void textBoxWeight_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
    }
}
