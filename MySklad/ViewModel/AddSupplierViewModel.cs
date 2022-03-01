using ModelApi;
using MySklad.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MySklad.ViewModel
{
    public class AddSupplierViewModel : BaseViewModel
    {
        public CustomCommand SaveSupplier { get; set; }

        public SupplierApi AddSupplierVM { get; set; }

        private List<SupplierApi> suppliers { get; set; }
        public List<SupplierApi> Suppliers
        {
            get => suppliers;
            set
            {
                suppliers = value;
                SignalChanged();
            }
        }

        public void CloseWindow(object obj)
        {
            Window window = obj as Window;
            window.Close();
        }

        private async Task GetSuppliers()
        {
            var suppliers = await Api.GetListAsync<SupplierApi>("Supplier");
        }

        private async Task PostSupplier(SupplierApi supplierApi)
        {
            var supplier = await Api.PostAsync<SupplierApi>(supplierApi, "Supplier");
        }
        private async Task EditSupplier(SupplierApi supplierApi)
        {
            var supplier = await Api.PutAsync<SupplierApi>(supplierApi, "Supplier");
        }

        public AddSupplierViewModel(SupplierApi supplierApi)
        {
            Suppliers = new List<SupplierApi>();
            if(supplierApi == null)
            {
                AddSupplierVM = new SupplierApi { Title = "Название компании", Email = "Email", Rating = 100 };
            }
            else
            {
                AddSupplierVM = new SupplierApi
                {
                    Id = supplierApi.Id,
                    Title = supplierApi.Title,
                    Email = supplierApi.Email,
                    Rating = supplierApi.Rating,
                    Phone = supplierApi.Phone
                };
            }

            SaveSupplier = new CustomCommand(() =>
            {
                if(AddSupplierVM.Id == 0)
                {
                    PostSupplier(AddSupplierVM);
                }
                else
                {
                    EditSupplier(AddSupplierVM);
                }
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.DataContext == this)
                    {
                        CloseWindow(window);
                    }
                }
                SignalChanged("Shops");
            });
        }
    }
}