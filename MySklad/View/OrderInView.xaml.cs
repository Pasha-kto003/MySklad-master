﻿using MySklad.ViewModel;
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

namespace MySklad.View
{
    /// <summary>
    /// Логика взаимодействия для OrderInView.xaml
    /// </summary>
    public partial class OrderInView : Page
    {
        public OrderInView()
        {
            InitializeComponent();
            DataContext = new OrderInViewModel();
        }
    }
}