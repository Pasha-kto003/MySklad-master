using ModelApi;
using MySklad.Core;
using MySklad.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MySklad.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        
        public CustomCommand OpenProduct { get; set; }
        public CustomCommand OpenDelete { get; set; }
        public CustomCommand OpenRack { get; set; }
        public CustomCommand OpenSupplier { get; set; }
        public CustomCommand OpenOrderIn { get; set; }
        public CustomCommand OpenOrderOut { get; set; }
        public CustomCommand OpenPersonal { get; set; }
        public CustomCommand OpenShop { get; set; }
        public CustomCommand OpenCatalog { get; set; }

        private Page _currentView;

        public Page CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; SignalChanged("CurrentView"); }
        }

        public MainViewModel()
        {
            OpenCatalog = new CustomCommand(() =>
            {
                CurrentView = new CatalogView();
            });
            OpenShop = new CustomCommand(() =>
            {
                CurrentView = new ShopView();
            });
            OpenProduct = new CustomCommand(() =>
            {
                CurrentView = new ProductView();
            });
            OpenDelete = new CustomCommand(() =>
            {
                CurrentView = new WriteOffRegisterView();
            });

            OpenPersonal = new CustomCommand(() =>
            {
                CurrentView = new PersonalView();
            });

            OpenSupplier = new CustomCommand(() =>
            {
                CurrentView = new SupplierView();
            });
        }
    }
}
