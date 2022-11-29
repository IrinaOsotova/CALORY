﻿using System;
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
        Int16 Id = 0;
        string? Name;
        string? Login;
        string? Password;
        public InfoUser()
        {
            InitializeComponent();
            numStage = 0;                       
            tb1.Visibility = Visibility.Hidden;
            tb2.Visibility = Visibility.Hidden;
            label1.Visibility = Visibility.Visible;
            label2.Visibility = Visibility.Visible;
            label1.Content = "Выберите пол";
            label2.Content = "Выберите дату рождения";
            GenderDateGrid.Visibility = Visibility.Visible;          
            AvtivityGrid.Visibility = Visibility.Hidden;
            PurposeGrid.Visibility = Visibility.Hidden;
            DateUser.DisplayDateEnd = DateTime.Now.AddYears(-14);
        }
        public InfoUser(Int16 _id, string? _name, string? _login, string? _password)
        {
            InitializeComponent();
            numStage = 0;
            tb1.Visibility = Visibility.Hidden;
            tb2.Visibility = Visibility.Hidden;
            label1.Visibility = Visibility.Visible;
            label2.Visibility = Visibility.Visible;
            label1.Content = "Выберите пол";
            label2.Content = "Выберите дату рождения";
            GenderDateGrid.Visibility = Visibility.Visible;
            AvtivityGrid.Visibility = Visibility.Hidden;
            PurposeGrid.Visibility = Visibility.Hidden;
            DateUser.DisplayDateEnd = DateTime.Now.AddYears(-14);
            Id = _id;
            Name = _name;
            Login = _login;
            Password = _password;
        }        

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            char[] Numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            bool isOk = true;
            switch (numStage)
            {
                case 0:
                    // пол (bool) 0 - ж 1 - м
                    // дата рождения (data) проверка на разумность                    
                    if (!userGender.HasValue && !DateUser.SelectedDate.HasValue)
                    {
                        MessageBox.Show("Выберите пол и дату рождения");
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
                            label1.Content = "Введите ваш вес (от 20 до 250)";
                            label2.Content = "Введите ваш рост (от 20 до 250)";
                        }
                        else
                        {                            
                            MessageBox.Show("Выберите дату рождения");
                        }                                             
                    }
                    else
                    {
                        MessageBox.Show("Выберите пол");                        
                    }
                    break;
                case 1:
                    if (tb1.Text != "") userWeight = int.Parse(tb1.Text);
                    else userWeight = null;
                    if (tb2.Text != "") userHeight = int.Parse(tb2.Text);
                    else userHeight = null;
                    if (!userWeight.HasValue && !userHeight.HasValue)
                    {
                        MessageBox.Show("Введите ваш вес и рост");
                        break;
                    }
                    if (userWeight.HasValue)
                    {
                        if(userWeight.Value < 20 || userWeight.Value > 250)
                        {
                            MessageBox.Show("Допустимый вес от 20 до 250");
                            tb1.Text = "";
                            break;
                        }
                        if (userHeight.HasValue)
                        {
                            if (userHeight.Value < 20 || userHeight.Value > 250)
                            {
                                MessageBox.Show("Допустимый рост от 20 до 250");
                                tb2.Text = "";
                                break;
                            }
                            numStage++;
                            AvtivityGrid.Visibility = Visibility.Visible;
                            tb1.Visibility= Visibility.Hidden;
                            tb2.Visibility= Visibility.Hidden;
                            label1.Content = "Выберите активность";
                            label2.Visibility = Visibility.Hidden;                            
                        }
                        else
                        {
                            MessageBox.Show("Введите ваш рост");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введите ваш вес");
                    }
                    break;                    
                case 2:
                    // активность                     
                    if (userActivity.HasValue)
                    {
                        numStage++;
                        AvtivityGrid.Visibility = Visibility.Hidden;
                        PurposeGrid.Visibility = Visibility.Visible;
                        label1.Content = "Выберите цель";
                    }
                    else
                    {                        
                        MessageBox.Show("Выберите активность");
                    }                    
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
                        using (var db = new ApplicationContext())
                        {
                            db.Users.Add(new User()
                            {
                                id = Id,
                                name = Name,
                                login = Login,
                                password = Password,
                                activity = userActivity.Value,
                                growth = (byte)userHeight.Value,
                                male = (byte)((userGender.Value) ? 1 : 0),
                                Birth = userBirthDate.Value,
                                goal = userPurpose.Value,
                                weight = (byte)userWeight.Value,
                                rsk = CalculateRSK(),
                                age = (byte)(DateTime.Now.Year - userBirthDate.Value.Year)
                            });
                            db.SaveChanges();
                        }

                        Diary window = new Diary(Login);
                        window.Show();
                        Close();
                    }
                    else
                    {                        
                        MessageBox.Show("Выберите цель");
                    }
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
                    label1.Visibility = Visibility.Visible;
                    label2.Visibility = Visibility.Visible;
                    label1.Content = "Выберите пол";
                    label2.Content = "Выберите дату рождения";
                    GenderDateGrid.Visibility = Visibility.Visible;
                    AvtivityGrid.Visibility = Visibility.Hidden;
                    PurposeGrid.Visibility = Visibility.Hidden;
                    break;
                case 1:                    
                    AvtivityGrid.Visibility = Visibility.Hidden;
                    tb1.Visibility = Visibility.Visible;
                    tb2.Visibility = Visibility.Visible;
                    label1.Visibility = Visibility.Visible;
                    label2.Visibility = Visibility.Visible;                    
                    label1.Content = "Введите ваш вес (от 20 до 250)";
                    label2.Content = "Введите ваш рост (от 20 до 250)";
                    break;
                case 2:
                    PurposeGrid.Visibility = Visibility.Hidden;
                    AvtivityGrid.Visibility = Visibility.Visible;
                    tb1.Visibility = Visibility.Hidden;
                    tb2.Visibility = Visibility.Hidden;
                    label1.Content = "Выберите активность";
                    label2.Visibility = Visibility.Hidden;
                    break;
                default:
                    //Error
                    break;
            }
        }

        private Int16 CalculateRSK()
        {
            double Weight = userWeight.Value;
            double Age = DateTime.Now.Year - userBirthDate.Value.Year;
            double Height = userHeight.Value;
            double result = 10.0 * Weight + 6.25 * Height - 5.0 * Age;
            if(userGender.Value) result += 5.0;
            else result -= 161.0;
            if (userActivity.Value == 1) result *= 1.2;
            if (userActivity.Value == 2) result *= 1.375;
            if (userActivity.Value == 3) result *= 1.725;
            if (userActivity.Value == 4) result *= 1.9;
            if (userPurpose.Value == 1) result += 250.0;
            if (userPurpose.Value == 3) result -= 250.0;
            return (Int16)Math.Round(result);
        }

        #region ButtonEvent
        private void buttonMan_Click(object sender, RoutedEventArgs e)
        {
            userGender = true;
            buttonMan.Content = "М*";
            buttonWoman.Content = "Ж";
        }

        private void buttonWoman_Click(object sender, RoutedEventArgs e)
        {
            userGender = false;
            buttonMan.Content = "М";
            buttonWoman.Content = "Ж*";
        }

        private void Activity1_Click(object sender, RoutedEventArgs e)
        {
            userActivity = 1;
            Activity1.Content = "Activity1*";
            Activity2.Content = "Activity2";
            Activity3.Content = "Activity3";
            Activity4.Content = "Activity4";
        }

        private void Activity2_Click(object sender, RoutedEventArgs e)
        {
            userActivity = 2;
            Activity1.Content = "Activity1";
            Activity2.Content = "Activity2*";
            Activity3.Content = "Activity3";
            Activity4.Content = "Activity4";
        }

        private void Activity3_Click(object sender, RoutedEventArgs e)
        {
            userActivity = 3;
            Activity1.Content = "Activity1";
            Activity2.Content = "Activity2";
            Activity3.Content = "Activity3*";
            Activity4.Content = "Activity4";
        }

        private void Activity4_Click(object sender, RoutedEventArgs e)
        {
            userActivity = 4;
            Activity1.Content = "Activity1";
            Activity2.Content = "Activity2";
            Activity3.Content = "Activity3";
            Activity4.Content = "Activity4*";
        }

        private void Purpose1_Click(object sender, RoutedEventArgs e)
        {
            userPurpose = 1;
            Purpose1.Content = "Purpose1*";
            Purpose2.Content = "Purpose2";
            Purpose3.Content = "Purpose3";
        }

        private void Purpose2_Click(object sender, RoutedEventArgs e)
        {
            userPurpose = 2;
            Purpose1.Content = "Purpose1";
            Purpose2.Content = "Purpose2*";
            Purpose3.Content = "Purpose3";
        }

        private void Purpose3_Click(object sender, RoutedEventArgs e)
        {
            userPurpose = 3;
            Purpose1.Content = "Purpose1";
            Purpose2.Content = "Purpose2";
            Purpose3.Content = "Purpose3*";
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
