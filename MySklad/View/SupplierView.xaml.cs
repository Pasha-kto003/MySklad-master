using ModelApi;
using MySklad.Core;
using MySklad.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MySklad.View
{
    /// <summary>
    /// Логика взаимодействия для SupplierView.xaml
    /// </summary>
    public partial class SupplierView : Page
    {
        public SupplierView()
        {
            InitializeComponent();
            DataContext = new SupplierViewModel();   
        }
    }
}
