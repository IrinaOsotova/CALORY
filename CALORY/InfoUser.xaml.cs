using LibraryConnectingDB;
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
    /// Логика взаимодействия для InfoUser.xaml
    /// </summary>
    public partial class InfoUser : Window
    {
        int numStage = 0;       

        //User virable
        bool? userGender = null;
        DateTime? userBirthDate = null;
        int? userWeight = null;
        int? userHeight = null;
        byte? userActivity = null;
        byte? userPurpose = null;
        private string? Name;
        private string? Login;
        private string? Password;
        private IConnectDB dbconnect;
        public InfoUser(string? _name, string? _login, string? _password)
        {
            InitializeComponent();
            numStage = 0;
            tb1.Visibility = Visibility.Hidden;
            tb2.Visibility = Visibility.Hidden;
            ImageLenta.Visibility = Visibility.Hidden;
            label1.Visibility = Visibility.Visible;
            label2.Visibility = Visibility.Visible;
            label1.Content = "Выберите пол:";
            label2.Content = "Выберите дату рождения:";
            GenderDateGrid.Visibility = Visibility.Visible;
            AvtivityGrid.Visibility = Visibility.Hidden;
            PurposeGrid.Visibility = Visibility.Hidden;
            DateUser.DisplayDateStart = DateTime.Now.AddYears(-100);
            DateUser.DisplayDateEnd = DateTime.Now.AddYears(-14);
            Name = _name;
            Login = _login;
            Password = _password;
        }
        public static Int16 CalculateRSK(double Weight, double Age, double Height, bool Gender, int Activity, int Purpose)
        {
            double result = 10.0 * Weight + 6.25 * Height - 5.0 * Age;
            if (Gender) result += 5.0;
            else result -= 161.0;
            if (Activity == 1) result *= 1.2;
            if (Activity == 2) result *= 1.375;
            if (Activity == 3) result *= 1.725;
            if (Activity == 4) result *= 1.9;
            if (Purpose == 1) result += 250.0;
            if (Purpose == 3) result -= 250.0;
            return (Int16)Math.Round(result);
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            switch (numStage)
            {
                case 0:                  
                    if (!userGender.HasValue && !DateUser.SelectedDate.HasValue)
                    {
                        MessageBox.Show("Выберите пол и дату рождения", "Ошибка ввода");
                        break;
                    }
                    if (userGender.HasValue)
                    {                        
                        if (DateUser.SelectedDate.HasValue)
                        {
                            numStage++;
                            userBirthDate = DateUser.SelectedDate.Value;
                            GenderDateGrid.Visibility = Visibility.Hidden;

                            tb1.Visibility = Visibility.Visible;
                            tb2.Visibility = Visibility.Visible;      
                            ImageLenta.Visibility = Visibility.Visible;
                            label1.Content = "Введите ваш вес (от 30 до 250 кг):";
                            label2.Content = "Введите ваш рост (от 100 до 250 см):";
                        }
                        else MessageBox.Show("Выберите дату рождения", "Ошибка ввода");                                          
                    }
                    else MessageBox.Show("Выберите пол", "Ошибка ввода");     
                    break;
                case 1:
                    BackButton.Visibility = Visibility.Visible;
                    if (tb1.Text != "") userWeight = int.Parse(tb1.Text);
                    else userWeight = null;
                    if (tb2.Text != "") userHeight = int.Parse(tb2.Text);
                    else userHeight = null;
                    if (!userWeight.HasValue && !userHeight.HasValue)
                    {
                        MessageBox.Show("Введите ваш вес и рост", "Ошибка ввода");
                        break;
                    }
                    if (userWeight.HasValue)
                    {
                        if(userWeight.Value < 30 || userWeight.Value > 250)
                        {
                            MessageBox.Show("Допустимый вес от 30 до 250 кг", "Ошибка ввода");
                            tb1.Text = "";
                            break;
                        }
                        if (userHeight.HasValue)
                        {
                            if (userHeight.Value < 100 || userHeight.Value > 250)
                            {
                                MessageBox.Show("Допустимый рост от 100 до 250 см", "Ошибка ввода");
                                tb2.Text = "";
                                break;
                            }
                            numStage++;
                            AvtivityGrid.Visibility = Visibility.Visible;
                            tb1.Visibility= Visibility.Hidden;
                            tb2.Visibility= Visibility.Hidden;
                            label1.Content = "Выберите активность:";
                            label2.Visibility = Visibility.Hidden;
                            ImageLenta.Visibility = Visibility.Hidden;
                        }
                        else MessageBox.Show("Введите ваш рост", "Ошибка ввода");
                    }
                    else MessageBox.Show("Введите ваш вес", "Ошибка ввода");
                    break;                    
                case 2:
                    // активность                     
                    if (userActivity.HasValue)
                    {
                        numStage++;
                        AvtivityGrid.Visibility = Visibility.Hidden;
                        PurposeGrid.Visibility = Visibility.Visible;
                        label1.Content = "Выберите цель:";
                    }
                    else MessageBox.Show("Выберите активность", "Ошибка ввода");           
                    break;
                case 3:
                    //цель
                    if (userPurpose.HasValue)
                    {
                        numStage++;                        
                        PurposeGrid.Visibility = Visibility.Hidden;
                        label1.Visibility = Visibility.Hidden;
                        NextButton.IsEnabled = false;
                        BackButton.IsEnabled = false;

                        //Сохранение в бд
                        //using (var db = new ApplicationContext())
                        //{
                        dbconnect = new ConnectingBD();

                        dbconnect.AddUserToBD(new LibraryConnectingDB.User()
                            {
                                id = 0,
                                name = Name,
                                login = Login,
                                password = Password,
                                activity = userActivity.Value,
                                growth = (byte)userHeight.Value,
                                male = (byte)((userGender.Value) ? 1 : 0),
                                Birth = userBirthDate.Value,
                                goal = userPurpose.Value,
                                weight = (byte)userWeight.Value,
                                rsk = CalculateRSK(userWeight.Value, DateTime.Now.Year - userBirthDate.Value.Year, userHeight.Value, userGender.Value, userActivity.Value, userPurpose.Value),
                                age = (byte)(DateTime.Now.Year - userBirthDate.Value.Year)
                            });
                            //db.SaveChanges();
                        //}
                        using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
                        {
                            Diary window = new Diary(Login);
                            window.Show();
                            Close();
                        }
                    }
                    else MessageBox.Show("Выберите цель", "Ошибка ввода"); 
                    break;
                default:
                    //Error
                    break;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {            
            if (numStage > 0) numStage--;
            switch (numStage)
            {
                case 0:
                    //DateUser.SelectedDate = userBirthDate;
                    
                    tb1.Visibility = Visibility.Hidden;
                    tb2.Visibility = Visibility.Hidden;
                    ImageLenta.Visibility = Visibility.Hidden;
                    label1.Visibility = Visibility.Visible;
                    label2.Visibility = Visibility.Visible;
                    label1.Content = "Выберите пол:";
                    label2.Content = "Выберите дату рождения:";
                    if (GenderDateGrid.Visibility == Visibility.Visible)
                    {
                        Registration window = new Registration(Name, Login);
                        window.Show();
                        Close();
                    }
                    else
                    {
                        GenderDateGrid.Visibility = Visibility.Visible;
                        AvtivityGrid.Visibility = Visibility.Hidden;
                        PurposeGrid.Visibility = Visibility.Hidden;
                    }
                    
                    break;
                case 1:
                    AvtivityGrid.Visibility = Visibility.Hidden;
                    tb1.Visibility = Visibility.Visible;
                    tb2.Visibility = Visibility.Visible;
                    ImageLenta.Visibility = Visibility.Visible;
                    label1.Visibility = Visibility.Visible;
                    label2.Visibility = Visibility.Visible;                    
                    label1.Content = "Введите ваш вес (от 30 до 250):";
                    label2.Content = "Введите ваш рост (от 100 до 250):";
                    break;
                case 2:
                    PurposeGrid.Visibility = Visibility.Hidden;
                    AvtivityGrid.Visibility = Visibility.Visible;
                    tb1.Visibility = Visibility.Hidden;
                    tb2.Visibility = Visibility.Hidden;
                    label1.Content = "Выберите активность:";
                    label2.Visibility = Visibility.Hidden;
                    ImageLenta.Visibility = Visibility.Hidden;

                    break;
                default:
                    //Error
                    break;
            }
        }
        #region ButtonEvent
        private void buttonMan_Click(object sender, RoutedEventArgs e)
        {
            userGender = true;
            buttonMan.BorderBrush = new SolidColorBrush(Color.FromRgb(179, 145, 212));
            buttonMan.Opacity = 1;
            buttonWoman.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 164, 164));
            buttonWoman.Opacity = 0.5;
        }

        private void buttonWoman_Click(object sender, RoutedEventArgs e)
        {
            userGender = false;
            buttonWoman.BorderBrush = new SolidColorBrush(Color.FromRgb(179, 145, 212));
            buttonWoman.Opacity = 1;
            buttonMan.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 164, 164));
            buttonMan.Opacity = 0.5;
        }

        private void Activity1_Click(object sender, RoutedEventArgs e)
        {
            userActivity = 1;
            Activity1.BorderBrush = new SolidColorBrush(Color.FromRgb(179, 145, 212));
            Activity1.Opacity = 1;
            Activity2.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 164, 164));
            Activity2.Opacity = 0.5;
            Activity3.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 164, 164));
            Activity3.Opacity = 0.5;
            Activity4.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 164, 164));
            Activity4.Opacity = 0.5;
        }

        private void Activity2_Click(object sender, RoutedEventArgs e)
        {
            userActivity = 2;
            Activity2.BorderBrush = new SolidColorBrush(Color.FromRgb(179, 145, 212));
            Activity2.Opacity = 1;
            Activity1.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 164, 164));
            Activity1.Opacity = 0.5;
            Activity3.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 164, 164));
            Activity3.Opacity = 0.5;
            Activity4.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 164, 164));
            Activity4.Opacity = 0.5;
        }

        private void Activity3_Click(object sender, RoutedEventArgs e)
        {
            userActivity = 3;
            Activity3.BorderBrush = new SolidColorBrush(Color.FromRgb(179, 145, 212));
            Activity3.Opacity = 1;
            Activity2.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 164, 164));
            Activity2.Opacity = 0.5;
            Activity1.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 164, 164));
            Activity1.Opacity = 0.5;
            Activity4.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 164, 164));
            Activity4.Opacity = 0.5;
        }

        private void Activity4_Click(object sender, RoutedEventArgs e)
        {
            userActivity = 4;
            Activity4.BorderBrush = new SolidColorBrush(Color.FromRgb(179, 145, 212));
            Activity4.Opacity = 1;
            Activity2.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 164, 164));
            Activity2.Opacity = 0.5;
            Activity3.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 164, 164));
            Activity3.Opacity = 0.5;
            Activity1.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 164, 164));
            Activity1.Opacity = 0.5;
        }

        private void Purpose1_Click(object sender, RoutedEventArgs e)
        {
            userPurpose = 1;
            Purpose1.BorderBrush = new SolidColorBrush(Color.FromRgb(179, 145, 212));
            Purpose1.Opacity = 1;
            Purpose2.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 164, 164));
            Purpose2.Opacity = 0.5;
            Purpose3.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 164, 164));
            Purpose3.Opacity = 0.5;
        }

        private void Purpose2_Click(object sender, RoutedEventArgs e)
        {
            userPurpose = 2;
            Purpose2.BorderBrush = new SolidColorBrush(Color.FromRgb(179, 145, 212));
            Purpose2.Opacity = 1;
            Purpose1.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 164, 164));
            Purpose1.Opacity = 0.5;
            Purpose3.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 164, 164));
            Purpose3.Opacity = 0.5;
        }

        private void Purpose3_Click(object sender, RoutedEventArgs e)
        {
            userPurpose = 3;
            Purpose3.BorderBrush = new SolidColorBrush(Color.FromRgb(179, 145, 212));
            Purpose3.Opacity = 1;
            Purpose2.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 164, 164));
            Purpose2.Opacity = 0.5;
            Purpose1.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 164, 164));
            Purpose1.Opacity = 0.5;
        }
        #endregion

        private void tb1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void tb2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
    }
}
