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
    /// Логика взаимодействия для AddPersonalView.xaml
    /// </summary>
    public partial class AddPersonalView : Window
    {
        public AddPersonalView()
        {
            InitializeComponent();
            DataContext = new AddPersonalViewModel(null);
        }

        public AddPersonalView(PersonalApi personalApi)
        {
            InitializeComponent();
            DataContext = new AddPersonalViewModel(personalApi);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
