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
    /// Логика взаимодействия для AddShopView.xaml
    /// </summary>
    public partial class AddShopView : Window
    {
        public AddShopView()
        {
            InitializeComponent();
            DataContext = new AddShopViewModel(null);
        }

        public AddShopView(ShopApi shop)
        {
            InitializeComponent();
            DataContext = new AddShopViewModel(shop);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
