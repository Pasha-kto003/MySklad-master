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
    /// Логика взаимодействия для AddUnitView.xaml
    /// </summary>
    public partial class AddUnitView : Window
    {
        public AddUnitView()
        {
            InitializeComponent();
            DataContext = new AddUnitViewModel(null);
        }

        public AddUnitView(UnitApi unitApi)
        {
            InitializeComponent();
            DataContext = new AddUnitViewModel(unitApi);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
