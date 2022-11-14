using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace CALORY
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public static AddWindow? instance;
        public Product current;
        public string _time;
        private bool correct = false;

        public AddWindow(string time)
        {
            InitializeComponent();
            instance = this;
            TextBoxSelectedProduct.Text = Diary.instance.ComboBoxSearch.SelectedItem.ToString();
            _time = time;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (correct)
            {
                if (_time == "Breakfast")
                {
                    //var year = Diary.instance.CalendarPiker.;
                    //int day = Diary.instance.CalendarPiker.
                    //int month = Diary.instance.CalendarPiker.
                    Diary.instance.listBoxBreakfast.Items.Add(current.ToStringFull());
                    using (var db = new ApplicationContext())
                    {
                        db.Meal.Add(new Meal()
                        {
                            code = 0,
                            data = Diary.instance.CalendarPiker.SelectedDate,
                            ration = _time,
                            food = Diary.instance.ComboBoxSearch.SelectedItem.ToString(),
                            gram = Convert.ToInt16(grams.Text),
                            kkal = Convert.ToInt16(current.kkal),
                            loginUser = Constants.login
                        }) ;
                        db.SaveChanges();
                    }
                }
                if (_time == "Lunch") Diary.instance.listBoxLunch.Items.Add(current.ToStringFull());
                if (_time == "Diner") Diary.instance.listBoxDiner.Items.Add(current.ToStringFull());

                //Diary.instance.caloryTextBox.Text = $"{Math.Round(Convert.ToDouble(Diary.instance.caloryTextBox.Text), 2) - Math.Round(Convert.ToDouble(KalTextBox.Text),2)}";

                Close();
            }
            else
            {
                MessageBox.Show("Ошибка ввода!");
                grams.Text = "";
            }
        }
        private void grams_TextChanged_1(object sender, TextChangedEventArgs e)
        {           
            try
            {
                if (grams.Text != "" && grams.Text != "0")
                {
                    current = ((Product)Diary.instance.ComboBoxSearch.SelectedItem).Copy();
                    current.gramm = int.Parse(grams.Text);
                    current.Recalculate();
                    KalTextBox.Text = current.kkal.ToString();
                    BelTextBox.Text = current.bel.ToString();
                    FatTextBox.Text = current.fats.ToString();
                    UglTextBox.Text = current.ugl.ToString();
                    correct = true;
                }
                else
                {
                    correct = false;
                    KalTextBox.Text = BelTextBox.Text = FatTextBox.Text = UglTextBox.Text = "";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка ввода!");
                grams.Text = grams.Text.Remove(grams.Text.Length - 1);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
