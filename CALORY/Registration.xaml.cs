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
            if (PasswordBoxRegistration.Password == "")
            {
                MessageBox.Show("Введите пароль");
                return;
            }

            if (PasswordBoxRegistration1.Password == "")
            {
                MessageBox.Show("Введите еще раз пароль");
                return;
            }
            if (PasswordBoxRegistration.Password != PasswordBoxRegistration1.Password)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }
        }
    }
}
