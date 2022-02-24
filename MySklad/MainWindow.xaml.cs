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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MySklad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Test();

            DataContext = new MainViewModel();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //async Task Test()
        //{

        //var unit = await Api.PostAsync<UnitApi>(new UnitApi { Title = "Еще одна единица" }, "Unit");
        //var unit = await Api.GetListAsync<UnitApi[]>("Unit");
        //}
    }
}
