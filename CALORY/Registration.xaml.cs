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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            #region examination
            if (TextBoxNameRegistration.Text == "")
            {
                MessageBox.Show("Введите имя");
                return;
            }
            if (TextBoxLoginRegistration.Text == "")
            {
                MessageBox.Show("Введите логин");
                return;
            }
            if (IsUserExists())
                return;
            bool resultLogin = LoginVerification(TextBoxLoginRegistration);
            if (resultLogin == false)
            {
                MessageBox.Show("Логин должен соответствовать следующим требованиям:\n 1.Длина не менее 5 символов \n 2.Cодержит только латинские буквы и цифры");
                return;
            }
            if (PasswordBoxRegistration.Password == "")
            {
                MessageBox.Show("Введите пароль");
                return;
            }
            bool resultPassword = PasswordVerification(PasswordBoxRegistration);
            if(resultPassword == false)
            {
                MessageBox.Show("Пароль должен соответствовать следующим требованиям:\n 1.Длина не менее 7 символов \n 2.Cодержит только латинские буквы и цифры \n 3.Cодержит хотя бы 1 букву верхнего регистра \n 4.Cодержит хотя бы 1 букву нижнего регистра \n 5.Cодержит хотя бы 1 цифру");
                return;
            }
            
            #endregion
            using (var db = new ApplicationContext())
            {
                db.Users.Add(new User()
                {
                    id = 0,
                    name = TextBoxNameRegistration.Text,
                    login = TextBoxLoginRegistration.Text,
                    password = Crypt.GetHashPassword(PasswordBoxRegistration.Password)
                });
                db.SaveChanges();
            }
            Diary window = new Diary();
            window.Show();
            Close();

        }
        public static char[] Numbers = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        public static char[] Uppercase = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        public static char[] Lowercase = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        static bool LoginVerification(TextBox LoginBox)
        {
            string login = LoginBox.Text;
            char[] CharLogin = login.ToCharArray();
            bool result = true;
            int countLogin = 0;
            foreach (var item in CharLogin)
            {
                countLogin++;
                if (Numbers.Contains(item) == false && Uppercase.Contains(item) == false && Lowercase.Contains(item) == false)
                    result = false;
            }
            if (result && countLogin > 4)
                return result = true;
            return result;

        }
        static bool PasswordVerification(PasswordBox passwordBox)
        {
            string password = passwordBox.Password;
            bool result = false;
            bool presenceNumber = false;
            bool presenceUppercase = false;
            bool presenceLowercase = false;
            char[] CharPassword = password.ToCharArray();
            int countPassword = 0;
            bool othersymbol = true;
            foreach (var item in CharPassword)
            {
                bool num = CharVerification(item, Numbers);
                if (num == true) presenceNumber = true;
                bool up = CharVerification(item, Uppercase);
                if (up == true) presenceUppercase = true;
                bool low = CharVerification(item, Lowercase);
                if (low == true) presenceLowercase = true;
                countPassword++;
                if (Numbers.Contains(item)==false && Uppercase.Contains(item) == false && Lowercase.Contains(item) == false)
                    othersymbol = false;
            }
            if (countPassword > 6 && othersymbol && presenceNumber && presenceUppercase && presenceLowercase)
                result = true;
            return result;
        }
        static bool CharVerification(char symbol, char[] symbolArray)
        {
            bool result = false;
            
            foreach (var item in symbolArray)
            {
                if (symbol == item)
                    result = true;
            }
            return result;
        }
        /// <summary>
        /// Метод проверяет есть ли пользователь с таким логином в БД
        /// </summary>
        /// <returns></returns>
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
    }
}
