using System.Globalization;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Input;

namespace CALORY
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ButtonComeInAuthorization_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxLoginAuthorization.Text == "")
            {
                MessageBox.Show("Введите логин", "Ошибка ввода");
                return;
            }
            if (PasswordBoxAuthorization.Password == "")
            {
                MessageBox.Show("Введите пароль", "Ошибка ввода");
                return;
            }
            using (var db = new ApplicationContext())
            {
                var user = db.Users.FirstOrDefault(item => item.login == TextBoxLoginAuthorization.Text && item.password == Crypt.GetHashPassword(PasswordBoxAuthorization.Password));
                if (user == null)
                    MessageBox.Show("Не правильно введен логин или пароль", "Ошибка ввода");
                else
                {
                    using (OverrideCursor cursor = new OverrideCursor(Cursors.Wait))
                    {
                        Diary window = new Diary(TextBoxLoginAuthorization.Text);
                        window.Show();
                        Close();
                    }
                }
            }
        }

        private void ShowPasswordA_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ShowPasswordA.Visibility = Visibility.Hidden;
            HidePasswordA.Visibility = Visibility;
            TextBoxShowPasswordA.Visibility = Visibility;
            TextBoxShowPasswordA.Text = PasswordBoxAuthorization.Password;
            PasswordBoxAuthorization.IsEnabled = false;
        }
        private void HidePasswordA_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ShowPasswordA.Visibility = Visibility;
            HidePasswordA.Visibility = Visibility.Hidden;
            TextBoxShowPasswordA.Visibility = Visibility.Hidden;
            PasswordBoxAuthorization.IsEnabled = true;
            PasswordBoxAuthorization.Focus();
        }
        private void HidePasswordA_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ShowPasswordA.Visibility = Visibility;
            HidePasswordA.Visibility = Visibility.Hidden;
            TextBoxShowPasswordA.Visibility = Visibility.Hidden;
            PasswordBoxAuthorization.IsEnabled = true;
            PasswordBoxAuthorization.Focus();
        }
        private void ButtonLinkToRegistrationAuthorization_Click(object sender, RoutedEventArgs e)
        {
            Registration window = new Registration();
            window.Show();
            Close();
        }

        private void TextBoxLoginAuthorization_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
