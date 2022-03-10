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
    /// Логика взаимодействия для AddRack.xaml
    /// </summary>
    public partial class AddRack : Window
    {
        public AddRack()
        {
            InitializeComponent();
            DataContext = new AddOrderViewModel(null);
        }

        public AddRack(RackApi rackApi)
        {
            InitializeComponent();
            DataContext = new AddOrderViewModel(rackApi);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
