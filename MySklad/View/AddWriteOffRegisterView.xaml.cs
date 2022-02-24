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
    /// Логика взаимодействия для AddWriteOffRegisterView.xaml
    /// </summary>
    public partial class AddWriteOffRegisterView : Window
    {
        public AddWriteOffRegisterView()
        {
            InitializeComponent();
            DataContext = new AddWriteOffRegisterViewModel(null);
        }

        public AddWriteOffRegisterView(WriteOffRegisterApi registerApi)
        {
            InitializeComponent();
            DataContext = new AddWriteOffRegisterViewModel(registerApi);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
