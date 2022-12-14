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
using LibraryConnectingDB;


namespace CALORY
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        private IConnectDB dbconnect;
        public static AddWindow? instance;
        public Repast current;
        public string _time;
        private bool correct = false;
        private string Login;
        //private IConnectDB dbconnect;

        public AddWindow(string time, string login)
        {
            Login = login;
            InitializeComponent();
            instance = this;
            TextBoxSelectedProduct.Text = Diary.instance.ComboBoxSearch.SelectedItem.ToString();
            _time = time;

        }
        public void AddDB(string _time)
        {

            dbconnect = new ConnectingBD();

            dbconnect.AddMealToBD(new LibraryConnectingDB.Repast()
            {
                code = 0,
                day = Diary.instance.CalendarPiker.SelectedDate,
                ration = _time,
                name = TextBoxSelectedProduct.Text,
                gram = Convert.ToInt16(grams.Text),
                kkal = current.kkal,
                loginUser = Login,
                bel = current.bel,
                fats = current.fats,
                ugl = current.ugl
            });
            //dbconnect.SaveChanges();

        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (correct)
            {
                if (_time == "Breakfast")
                {
                    Diary.instance.listBoxBreakfast.Items.Add(current.ToStringFull());
                    Diary.instance.productsBreakfast.Add(current);
                    AddDB(_time);
                    Close();
                }
                if (_time == "Lunch")
                {
                    Diary.instance.listBoxLunch.Items.Add(current.ToStringFull());
                    Diary.instance.productsLunch.Add(current);
                    AddDB(_time);
                    Close();
                }
                if (_time == "Diner")
                {
                    Diary.instance.listBoxDiner.Items.Add(current.ToStringFull());
                    Diary.instance.productsDiner.Add(current);
                    AddDB(_time);
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Некорректно введены граммы", "Ошибка ввода");
                grams.Text = "";
            }
        }
        private void grams_TextChanged(object sender, TextChangedEventArgs e)
        {           
            try
            {
                if (grams.Text != "" && grams.Text != "0" && grams.Text != "00" && grams.Text != "000" && grams.Text != "0000")
                {
                    current = ((Repast)Diary.instance.ComboBoxSearch.SelectedItem).Copy();
                    current.gram = Int16.Parse(grams.Text);
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
                MessageBox.Show("Некорректно введены граммы", "Ошибка ввода");
                grams.Text = grams.Text.Remove(grams.Text.Length - 1);
            }
        }

        private void grams_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
    }
}
