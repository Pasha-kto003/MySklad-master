using ModelApi;
using MySklad.Core;
using MySklad.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MySklad.ViewModel
{
    public class ProductViewModel : BaseViewModel
    {
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

        private List<UnitApi> units { get; set; }
        public List<UnitApi> Units
        {
            get => units;
            set
            {
                units = value;
                SignalChanged();
            }
        }

        private List<ProductTypeApi> productTypes { get; set; }
        public List<ProductTypeApi> ProductTypes
        {
            get => productTypes;
            set
            {
                productTypes = value;
                SignalChanged();
            }
        }

        private ProductApi selectedProduct { get; set; }
        public ProductApi SelectedProduct
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                SignalChanged();
            }
        }

        public CustomCommand CreateProduct { get; set; }
        public CustomCommand EditProduct { get; set; }

        public ProductViewModel()
        {          
            Units = new List<UnitApi>();
            ProductTypes = new List<ProductTypeApi>();
            Products = new List<ProductApi>();
            GetProducts();
            
            CreateProduct = new CustomCommand(() =>
            {
                AddProduct addProduct = new AddProduct();
                addProduct.ShowDialog();
                GetProducts();
            });

            EditProduct = new CustomCommand(() =>
            {
                if (SelectedProduct == null)
                {
                    MessageBox.Show("Выберите продукт из списка продукций");
                    return;
                }
                else
                {
                    AddProduct addProduct = new AddProduct(SelectedProduct);
                    addProduct.ShowDialog();
                    GetProducts();
                }
            });
            SignalChanged("Products");
        }


        private async Task GetProducts()
        {
            Units = await Api.GetListAsync<List<UnitApi>>("Unit");
            ProductTypes = await Api.GetListAsync<List<ProductTypeApi>>("ProductType");
            Products = await Api.GetListAsync<List<ProductApi>>("Product");
            foreach (ProductApi product in Products)
            {
                product.Unit = Units.First(s => s.Id == product.UnitId);
                product.ProductType = ProductTypes.First(s => s.Id == product.ProductTypeId);
            }
            SignalChanged("Products");
        }
    }
}
