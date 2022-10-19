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
            if (PasswordBoxRegistration.Password == "")
            {
                MessageBox.Show("Введите пароль");
                return;
            }
            bool result = PasswordVerification(PasswordBoxRegistration);
            if(result == false)
            {
                MessageBox.Show("Пароль должен соответствовать следующим требованиям:\n 1.Длина не менее 7 символов \n 2.Cодержит только латинские буквы и цифры \n 3.Cодержит хотя бы 1 букву верхнего регистра \n 4.Cодержит хотя бы 1 букву нижнего регистра \n 5.Cодержит хотя бы 1 цифру");
                return;
            }
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

        private void LabelLinkToAuthorizationRegistration_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }


        
        private void ShowPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IsEyeClick(TextBoxShowPassword, PasswordBoxRegistration, ShowPassword, HidePassword);
        }
        
        public void IsEyeClick(TextBox textBox, PasswordBox password, Image showimage, Image hideimage)
        {
            if (textBox.Visibility == Visibility)
            {
                showimage.Visibility = Visibility;
                hideimage.Visibility = Visibility.Hidden;
                textBox.Visibility = Visibility.Hidden;
            }
            else
            {
                showimage.Visibility = Visibility.Hidden;
                hideimage.Visibility = Visibility;
                textBox.Visibility = Visibility;
                textBox.Text = password.Password;
            }
        }
    }
}
