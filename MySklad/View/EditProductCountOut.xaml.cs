using ModelApi;
using MySklad.ViewModel;
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

namespace MySklad.View
{
    /// <summary>
    /// Логика взаимодействия для EditProductCountOut.xaml
    /// </summary>
    public partial class EditProductCountOut : Window
    {
        public EditProductCountOut()
        {
            InitializeComponent();
            DataContext = new EditProductCountOutViewModel(null);
        }

        public EditProductCountOut(ProductApi productApi)
        {
            InitializeComponent();
            DataContext = new EditProductCountOutViewModel(productApi);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
