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
    /// Логика взаимодействия для AddSupplierView.xaml
    /// </summary>
    public partial class AddSupplierView : Window
    {
        public AddSupplierView()
        {
            InitializeComponent();
            DataContext = new AddSupplierViewModel(null);
        }
        public AddSupplierView(SupplierApi supplierApi)
        {
            InitializeComponent();
            DataContext = new AddSupplierViewModel(supplierApi);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
