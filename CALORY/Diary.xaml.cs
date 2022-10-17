using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Newtonsoft.Json;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CALORY
{
    public class Product
    {
        public string name { get; set; }
        public double kkal { get; set; }
        public double ugl { get; set; }
        public double fats { get; set; }
        public double bel { get; set; }
        private double gram = 100;
        public double gramm
        {
            get
            {
                return gram;
            }
            set
            {
                if (value != 0)
                {
                    gram = value;
                }
            }
        }
        public void Recalculate()
        {
            kkal = Math.Round(kkal * gram / 100, 2);
            bel = Math.Round(bel * gram / 100, 2);
            ugl = Math.Round(ugl * gram / 100, 2);
            fats = Math.Round(fats * gram / 100, 2);
        }

        public Product Copy()
        {
            return new Product(name, gram.ToString(), kkal.ToString(), bel.ToString(), fats.ToString(), ugl.ToString());
        }
        public override string ToString()
        {
            return name;
        }

        public string ToStringFull()
        {
            return name + " " + gramm + " г. " + " - " + kkal + " ккал., " + bel + " г. бел., " + fats + " г. жир., " + ugl + " г. угл.\r\n__________________________________________________________\r";
        }

        public Product(string _name, string _gram, string _kkal, string _bel, string _fat, string _ugl)
        {
            if (_name != null)
            {
                name = _name;
                kkal = double.Parse(_kkal);
                bel = double.Parse(_bel);
                fats = double.Parse(_fat);
                ugl = double.Parse(_ugl);
                gram = double.Parse(_gram);
            }
            else
            {
                name = "";

            }
        }
    }
    public partial class Diary : Window
    {
        //public Product[] massiv;
        public List<Product> productsBase = new List<Product>();
        public static Diary instance;
        public Diary()
        {
            InitializeComponent();
            instance = this;
            using (StreamReader GroceryList = new StreamReader("..\\..\\..\\products.txt"))
            {
                var jsr = new JsonTextReader(GroceryList);
                productsBase = new JsonSerializer().Deserialize<List<Product>>(jsr);
            }
            //massiv = new Product[productsBase.Count()];
            ComboBoxSearch.ItemsSource = productsBase;
        }
        private void ComboBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ComboBoxSearch.Text.Length > 2)
            {

                var tb = (TextBox)e.OriginalSource;
                if (tb.SelectionStart != 0)
                {
                    ComboBoxSearch.SelectedItem = null; // Если набирается текст сбросить выбраный элемент
                }
                if (tb.SelectionStart == 0 && ComboBoxSearch.SelectedItem == null)
                {
                    ComboBoxSearch.IsDropDownOpen = false; // Если сбросили текст и элемент не выбран, сбросить фокус выпадающего списка
                }
                ComboBoxSearch.IsDropDownOpen = true;
                if (ComboBoxSearch.SelectedItem == null && ComboBoxSearch.Text.Length > 0)
                {
                    // Если элемент не выбран менять фильтр
                    CollectionView cv = (CollectionView)CollectionViewSource.GetDefaultView(ComboBoxSearch.ItemsSource);
                    cv.Filter = s => s.ToString().IndexOf(ComboBoxSearch.Text, StringComparison.CurrentCultureIgnoreCase) >= 0;
                }
            }
        }

        private void AddBreakfast_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxSearch.SelectedItem == null)
            {
                MessageBox.Show("Продукт не выбран");
            }
            else if (textBoxBreakfast.Text.Contains(ComboBoxSearch.SelectedItem.ToString()))
            {
                MessageBox.Show("Данный продукт уже есть в списке");
            }
            else
            {
                AddWindow SelectedProductsWindow = new AddWindow("Breakfast");
                SelectedProductsWindow.Show();
            }
        }

        private void AddLunch_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxSearch.SelectedItem == null)
            {
                MessageBox.Show("Продукт не выбран");
            }
            else if (textBoxLunch.Text.Contains(ComboBoxSearch.SelectedItem.ToString()))
            {
                MessageBox.Show("Данный продукт уже есть в списке");
            }
            else
            {
                AddWindow SelectedProductsWindow = new AddWindow("Lunch");
                SelectedProductsWindow.Show();
            }
        }

        private void AddDinner_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxSearch.SelectedItem == null)
            {
                MessageBox.Show("Продукт не выбран");
            }
            else if (textBoxDiner.Text.Contains(ComboBoxSearch.SelectedItem.ToString()))
            {
                MessageBox.Show("Данный продукт уже есть в списке");
            }
            else
            {
                AddWindow SelectedProductsWindow = new AddWindow("Diner");
                SelectedProductsWindow.Show();
            }
        }
    }
}
