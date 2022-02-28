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
        public ProductApi SelectedSupplierProduct { get; set; }

        private List<ProductApi> selectedSupplierProducts = new List<ProductApi>();
        public List<ProductApi> SelectedSupplierProducts
        {
            get => selectedSupplierProducts;
            set
            {
                selectedSupplierProducts = value;
                SignalChanged();
            }
        }

        public SupplierApi AddSupplierVM { get; set; }

        public ProductApi SelectedProduct { get; set; }
        private List<ProductApi> product { get; set; }
        public List<ProductApi> Product
        {
            get => product;
            set
            {
                product = value;
                SignalChanged();
            }
        }

        public List<ProductSupplierApi> ProductSuppliers { get; set; }
        public List<SupplierApi> Suppliers { get; set; }
        public List<ProductApi> Products { get; set; }

        public CustomCommand AddProduct { get; set; }
        public CustomCommand EditProduct { get; set; }
        public CustomCommand RemoveProduct { get; set; }
        public CustomCommand SaveSupplier { get; set; }

        public AddSupplierViewModel(SupplierApi supplierApi)
        {
            Product = new List<ProductApi>();
            Suppliers = new List<SupplierApi>();
            GetProduct();
            if (supplierApi == null)
            {
                AddSupplierVM = new SupplierApi { Title = "Название", Rating = 100, Email = "почта@" };
            }

            else
            {
                AddSupplierVM = new SupplierApi
                {
                    Id = supplierApi.Id,
                    Title = supplierApi.Title,
                    Rating = supplierApi.Rating,
                    ProductSupplier = supplierApi.ProductSupplier,
                    Email = supplierApi.Email,
                    Phone = supplierApi.Phone
                };

                if(supplierApi.Products != null)
                {
                    SelectedSupplierProducts = new List<ProductApi>();
                }

            }

            AddProduct = new CustomCommand(() =>
            {
                if(SelectedProduct == null)
                {
                    MessageBox.Show("Ошибка");
                    return;
                }
                else if (SelectedSupplierProducts.Contains(SelectedProduct))
                {
                    MessageBox.Show("Этот уже есть");
                }
                else
                {
                    SelectedSupplierProducts.Add(SelectedProduct);
                    SignalChanged("SelectedSupplierProducts");
                }
            });
        }

        private async Task GetProduct()
        {
            Product = await Api.GetListAsync<List<ProductApi>>("Product");
        }
    }
}
