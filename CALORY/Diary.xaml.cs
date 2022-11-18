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

namespace CALORY
{
    public partial class Diary : Window
    {
        public List<Product> productsBreakfast = new List<Product>();
        public List<Product> productsLunch = new List<Product>();
        public List<Product> productsDiner = new List<Product>();
        public List<Product> productsBase = new List<Product>();
        public static Diary? instance;
        private string Login;
     
        public Diary(string login)
        {
            Login = login;
            InitializeComponent();
            instance = this;
            //CalendarPiker.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddDays(1), DateTime.Now.AddDays(340)));
            CalendarPiker.SelectedDate = DateTime.Now;
            CalendarPiker.DisplayDateStart = DateTime.Today.AddDays(-7);
            CalendarPiker.DisplayDateEnd = DateTime.Now;
            using (StreamReader GroceryList = new StreamReader("..\\..\\..\\products.txt"))
            {
                var jsr = new JsonTextReader(GroceryList);
                productsBase = new JsonSerializer().Deserialize<List<Product>>(jsr);
            }
            ComboBoxSearch.ItemsSource = productsBase;
        }
        private void ComboBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ComboBoxSearch.Text.Length > 2)
            {
                var tb = (TextBox)e.OriginalSource;
                if (tb.SelectionStart != 0)
                {
                    ComboBoxSearch.SelectedItem = null; // Если набирается текст сбросить выбраный элемент
                }
                if (tb.SelectionStart == 0 && ComboBoxSearch.SelectedItem == null)
                {
                    ComboBoxSearch.IsDropDownOpen = false; // Если сбросили текст и элемент не выбран, сбросить фокус выпадающего списка
                }
                ComboBoxSearch.IsDropDownOpen = true;
                if (ComboBoxSearch.SelectedItem == null && ComboBoxSearch.Text.Length > 0)
                {
                    // Если элемент не выбран менять фильтр
                    CollectionView cv = (CollectionView)CollectionViewSource.GetDefaultView(ComboBoxSearch.ItemsSource);
                    cv.Filter = s => s.ToString().IndexOf(ComboBoxSearch.Text, StringComparison.CurrentCultureIgnoreCase) >= 0;
                }
            }
        }

        public void Add(string timeMeal)
        {
            if (ComboBoxSearch.SelectedItem == null)
                MessageBox.Show("Продукт не выбран");
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
                var user = db.Users.FirstOrDefault(x => x.login == Login);
                rskGoalTextBox.Text = user.rsk.ToString();
            }
            //if (sender.ToString() == "") CalendarPiker.Text = "";
        }

        private void buttonDeleteBreakfast_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxBreakfast.SelectedItems.Count == 0)
                MessageBox.Show("Выберите продукт для удаления");
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
                MessageBox.Show("Выберите продукт для удаления");
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
                MessageBox.Show("Выберите продукт для удаления");
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
            if (TabControlDiary.SelectedIndex == 2)
            {
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
                        values[6 - i] = EatenKkal;
                        EatenKkal = 0;
                    }
                    foreach (var item in db.Meal.Where(x => x.loginUser == Login && x.day == thisDay.Date))
                        EatenUgl += item.ugl;
                    foreach (var item in db.Meal.Where(x => x.loginUser == Login && x.day == thisDay.Date))
                        EatenFats += item.fats;
                    foreach (var item in db.Meal.Where(x => x.loginUser == Login && x.day == thisDay.Date))
                        EatenBel += item.bel;

                }
                double[] positions = { 0, 1, 2, 3, 4, 5, 6 };
                string[] labels = { thisDay.AddDays(-6).ToString("d"), thisDay.AddDays(-5).ToString("d"), thisDay.AddDays(-4).ToString("d"), thisDay.AddDays(-3).ToString("d"), thisDay.AddDays(-2).ToString("d"), thisDay.AddDays(-1).ToString("d"), thisDay.ToString("d") };
                Chart.Plot.AddBar(values, positions);
                Chart.Plot.AddBar(values);

                Chart.Plot.XTicks(positions, labels);
                Chart.Plot.SetAxisLimits(yMin: 0);

                double[] values1 = { EatenUgl, EatenFats, EatenBel };
                var pie = Donut.Plot.AddPie(values1);
                pie.Explode = true;
                pie.DonutSize = .4;

                Chart.Refresh();
            }
        }
    }
}
