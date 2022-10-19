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
                    Diary window = new Diary();
                    window.Show();
                    Close();
                }
            }
        }
        

        //private void LabelLinkToRegistrationAuthorization_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    Registration window = new Registration();
        //    window.Show();
        //    Close();
        //}

        private void ShowPasswordA_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsEyeClick(TextBoxShowPasswordA, PasswordBoxAuthorization, ShowPasswordA, HidePasswordA);
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

        private void ButtonLinkToRegistrationAuthorization_Click(object sender, RoutedEventArgs e)
        {
            Registration window = new Registration();
            window.Show();
            Close();
        }
    }
}
