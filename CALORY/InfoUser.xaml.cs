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
        //bool? gender = null;

        //User virable
        bool? userGender = null;
        DateTime? userBirthDate = null;
        short userWeight = -1;
        short userHeight = -1;
        short userActivity = -1;
        short userPurpose = -1;
        public InfoUser()
        {
            InitializeComponent();
            numStage = 0;
            //gender = null;
            //add view windows            
            tb.Visibility = Visibility.Hidden;
            label.Visibility = Visibility.Visible;
            label.Content = "Enter gender";
            GenderGrid.Visibility = Visibility.Visible;
            DateGrid.Visibility = Visibility.Hidden;
            AvtivityGrid.Visibility = Visibility.Hidden;
            PurposeGrid.Visibility = Visibility.Hidden;
        }        

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            char[] Numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            bool isOk = true;
            switch (numStage)
            {
                case 0:
                    // пол (bool) 0 - ж 1 - м
                    if (userGender != null)
                    {
                        numStage++;
                        //userGender = gender.Value;
                        GenderGrid.Visibility = Visibility.Hidden;
                        DateGrid.Visibility = Visibility.Visible;
                        label.Content = "Enter your date";                        
                    }
                    else
                    {
                        MessageBox.Show("Выберите пол");
                        //chose gender
                    }
                    break;
                case 1:
                    // дата рождения (data) проверка на разумность
                    if (DateUser.SelectedDate.HasValue)
                    {
                        numStage++;
                        userBirthDate = DateUser.SelectedDate.Value;
                        DateGrid.Visibility = Visibility.Hidden;
                        tb.Visibility = Visibility.Visible;
                        tb.Text = "от 20 до 400";
                        label.Content = "Enter your weight";
                    }
                    else
                    {
                        //enter date
                        MessageBox.Show("Выберите дату рождения");
                    }
                    break;
                case 2:
                    // вес (short) 20-400                    
                    isOk = false;                    
                    if (tb.Text.Length < 4)
                    {
                        isOk = true;
                        foreach (var item in tb.Text)
                        {
                            if (!Numbers.Contains(item)) {
                                isOk = false;
                                break;
                            }
                        }
                    }
                    if (isOk) userWeight = short.Parse(tb.Text);
                    if (isOk && userWeight <= 400 && userWeight >= 20)
                    {
                        numStage++;                        
                        tb.Text = "от 15 до 300";
                        label.Content = "Enter your height";
                    }
                    else
                    {
                        //enter weight 20-400
                        MessageBox.Show("Введите вес (от 20 до 400кг)");
                    }
                    break;
                case 3:
                    // рост (short) 15-300                    
                    isOk = false;
                    if (tb.Text.Length < 4)
                    {
                        isOk = true;
                        foreach (var item in tb.Text)
                        {
                            if (!Numbers.Contains(item))
                            {
                                isOk = false;
                                break;
                            }
                        }
                    }
                    if (isOk) userHeight = short.Parse(tb.Text);
                    if (isOk && userHeight <= 300 && userHeight >= 15)
                    {
                        numStage++;
                        userHeight = short.Parse(tb.Text);
                        tb.Visibility = Visibility.Hidden;
                        AvtivityGrid.Visibility = Visibility.Visible;
                        label.Content = "Enter your activity";
                    }
                    else
                    {
                        //enter height 15-300
                        MessageBox.Show("Введите рост (от 15 до 300см)");
                    }
                    break;
                case 4:
                    // активность                     
                    if (userActivity != -1)
                    {
                        numStage++;
                        AvtivityGrid.Visibility = Visibility.Hidden;
                        PurposeGrid.Visibility = Visibility.Visible;
                        label.Content = "Enter your purpose";
                    }
                    else
                    {
                        //chose activity
                        MessageBox.Show("Выберите активность");
                    }                    
                    break;
                case 5:
                    //цель
                    if (userPurpose != -1)
                    {
                        numStage++;                        
                        PurposeGrid.Visibility = Visibility.Hidden;


                        //Сохранение в бд
                        Diary window = new Diary();
                        window.Show();
                        Close();
                    }
                    else
                    {
                        //chose Purpose
                        MessageBox.Show("Выберите цель");
                    }
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
        }

        private void buttonWoman_Click(object sender, RoutedEventArgs e)
        {
            userGender = false;
        }

        private void Activity1_Click(object sender, RoutedEventArgs e)
        {
            userActivity = 1;
        }

        private void Activity2_Click(object sender, RoutedEventArgs e)
        {
            userActivity = 2;
        }

        private void Activity3_Click(object sender, RoutedEventArgs e)
        {
            userActivity = 3;
        }

        private void Activity4_Click(object sender, RoutedEventArgs e)
        {
            userActivity = 4;
        }

        private void Purpose1_Click(object sender, RoutedEventArgs e)
        {
            userPurpose = 1;
        }

        private void Purpose2_Click(object sender, RoutedEventArgs e)
        {
            userPurpose = 2;
        }

        private void Purpose3_Click(object sender, RoutedEventArgs e)
        {
            userPurpose = 3;
        }
        #endregion
    }
}
