using ModelApi;
using MySklad.Core;
using MySklad.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySklad.ViewModel
{
    public class SupplierViewModel : BaseViewModel
    {
        private List<ProductSupplierApi> productSuppliers { get; set; }
        public List<ProductSupplierApi> ProductSuppliers
        {
            get => productSuppliers;
            set
            {
                productSuppliers = value;
                SignalChanged();
            }
        }
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

        private List<ProductApi> products { get; set; }
        public List<ProductApi> Products
        {
            get => products;
            set
            {
                products = value;
                SignalChanged();
            }
        }

        private SupplierApi selectedSupplier { get; set; }
        public SupplierApi SelectedSupplier
        {
            get => selectedSupplier;
            set
            {
                selectedSupplier = value;
                SignalChanged();
            }
        }

        public CustomCommand AddSupplier { get; set; }
        public CustomCommand EditSupplier { get; set; }

        public SupplierViewModel()
        {
            
            Suppliers = new List<SupplierApi>();
            GetSuppliers();

            AddSupplier = new CustomCommand(() =>
            {
                AddSupplier addSupplier = new AddSupplier();
                addSupplier.ShowDialog();
                GetSuppliers();
                SignalChanged("Suppliers");
            });

            EditSupplier = new CustomCommand(() =>
            {
                if (SelectedSupplier == null) return;
                AddSupplier addSupplier = new AddSupplier(SelectedSupplier);
                addSupplier.ShowDialog();
                GetSuppliers();
                SignalChanged("Suppliers");
            });
            SignalChanged("Suppliers");
        }

        private async Task GetSuppliers()
        {
            Suppliers = await Api.GetListAsync<List<SupplierApi>>("Supplier");
            //Products = await Api.GetListAsync<List<ProductApi>>("Product");
            //Suppliers.Select(s =>
            //{
            //    var products = ProductSuppliers.Where(t => t.SupplierId == s.Id).Select(t => t.Product).ToList();
            //    return products;
            //});
            //SignalChanged("Suppliers");
        }
    }
}
