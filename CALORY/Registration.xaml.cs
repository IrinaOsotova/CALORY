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
using System.Text.RegularExpressions;
using LibraryConnectDB;

namespace CALORY
{
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }
        public Registration(string Name, string Login)
        {
            InitializeComponent();
            TextBoxNameRegistration.Text = Name;
            TextBoxLoginRegistration.Text = Login;
        }
        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!Verification(TextBoxNameRegistration))
            {
                MessageBox.Show("Имя должно соответствовать следующим требованиям:\n 1.Длина не менее 3 символов \n 2.Cодержит только латинские буквы и цифры", "Ошибка ввода");
                return;
            }
            if (!Verification(TextBoxLoginRegistration))
            {
                MessageBox.Show("Логин должен соответствовать следующим требованиям:\n 1.Длина не менее 3 символов \n 2.Cодержит только латинские буквы и цифры", "Ошибка ввода");
                return;
            }
            if (IsUserExists())
                return;
            if (!PasswordVerification(PasswordBoxRegistration))
            {
                MessageBox.Show("Пароль должен соответствовать следующим требованиям:\n 1.Длина не менее 7 символов \n 2.Cодержит только латинские буквы и цифры \n 3.Cодержит хотя бы 1 букву верхнего регистра \n 4.Cодержит хотя бы 1 букву нижнего регистра \n 5.Cодержит хотя бы 1 цифру", "Ошибка ввода");
                return;
            }
            
            InfoUser window = new(TextBoxNameRegistration.Text, TextBoxLoginRegistration.Text, Crypt.GetHashPassword(PasswordBoxRegistration.Password));
            window.Show();
            Close();
        }
        public static bool Verification(TextBox temp)
        {
            string regex = "^[0-9A-zА-я]{3,}$";
            return Regex.IsMatch(temp.Text, regex);
        }
        static bool PasswordVerification(PasswordBox passwordBox)
        {
            string regex = "^(?=.*[0-9])(?=.*[A-Z])(?=.*[a-z])[0-9A-Za-z]{7,}$";
            return Regex.IsMatch(passwordBox.Password, regex);
        }
        public Boolean IsUserExists()
        {
            using (var db = new ApplicationContext())
            {
                var user = db.Users.FirstOrDefault(item => item.login == TextBoxLoginRegistration.Text);
                if (user != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует ");
                    return true;
                }
                else
                    return false;
            }
        }
        private void ButtonLinkToRegistrationAuthorization_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }
        private void ShowPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowPassword.Visibility = Visibility.Hidden;
            HidePassword.Visibility = Visibility;
            TextBoxShowPassword.Visibility = Visibility;
            TextBoxShowPassword.Text = PasswordBoxRegistration.Password;
            PasswordBoxRegistration.IsEnabled = false;
        }
        private void HidePassword_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ShowPassword.Visibility = Visibility;
            HidePassword.Visibility = Visibility.Hidden;
            TextBoxShowPassword.Visibility = Visibility.Hidden;
            PasswordBoxRegistration.IsEnabled = true;
            PasswordBoxRegistration.Focus();
        }
        private void HidePassword_MouseLeave(object sender, MouseEventArgs e)
        {
            ShowPassword.Visibility = Visibility;
            HidePassword.Visibility = Visibility.Hidden;
            TextBoxShowPassword.Visibility = Visibility.Hidden;
            PasswordBoxRegistration.IsEnabled = true;
            PasswordBoxRegistration.Focus();
        }
    }
}
