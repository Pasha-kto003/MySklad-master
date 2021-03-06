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
    /// Логика взаимодействия для AddStatusView.xaml
    /// </summary>
    public partial class AddStatusView : Window
    {
        public AddStatusView()
        {
            InitializeComponent();
            DataContext = new AddStatusViewModel(null);
        }

        public AddStatusView(StatusApi statusApi)
        {
            InitializeComponent();
            DataContext = new AddStatusViewModel(statusApi);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }        
    }
}
