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
    /// Логика взаимодействия для AddOrderIn.xaml
    /// </summary>
    public partial class AddOrderIn : Window
    {
        public List<OrderInApi> Orders = new List<OrderInApi>();
        public AddOrderIn()
        {
            InitializeComponent();
            DataContext = new AddOrderInViewModel(null);
        }

        public AddOrderIn(OrderInApi orderInApi)
        {
            InitializeComponent();
            DataContext = new AddOrderInViewModel(orderInApi);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DataGrid_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}
