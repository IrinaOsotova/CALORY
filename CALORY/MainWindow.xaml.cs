using System.Linq;
using System.Windows;

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
    }
}
