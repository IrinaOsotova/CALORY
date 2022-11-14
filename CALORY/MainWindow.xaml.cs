using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
                MessageBox.Show("Введите логин");
                return;
            }
            if (PasswordBoxAuthorization.Password == "")
            {
                MessageBox.Show("Введите пароль");
                return;
            }
            using (var db = new ApplicationContext())
            {
                var user = db.Users.FirstOrDefault(item => item.login == TextBoxLoginAuthorization.Text && item.password == Crypt.GetHashPassword(PasswordBoxAuthorization.Password));
                if (user == null)
                {
                    MessageBox.Show("Не правильно введен логин или пароль");
                }
                else
                {
                    Constants.login = user.login;
                    Diary window = new Diary();
                    window.Show();
                    Close();
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
        private void ButtonLinkToRegistrationAuthorization_Click(object sender, RoutedEventArgs e)
        {
            Registration window = new Registration();
            window.Show();
            Close();
        }

       
    }
}
