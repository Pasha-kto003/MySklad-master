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
    public class AddOrderOutViewModel : BaseViewModel
    {
        public OrderOutApi AddOrderVM { get; set; }
        public int NewCross { get; set; }
        private ProductApi selectedOrderProduct { get; set; }

        public ProductApi SelectedOrderProduct
        {
            get => selectedOrderProduct;
            set
            {
                selectedOrderProduct = value;
                SignalChanged();
            }
        }

        private List<ProductApi> selectedOrderProducts = new List<ProductApi>();
        public List<ProductApi> SelectedOrderProducts
        {
            get => selectedOrderProducts;
            set
            {
                selectedOrderProducts = value;
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

        public CustomCommand SaveOrder { get; set; }
        public CustomCommand AddProduct { get; set; }
        public CustomCommand EditProduct { get; set; }
        public CustomCommand RemoveProduct { get; set; }
        public CustomCommand EditProductCount { get; set; }

        public void CloseWindow(object obj)
        {
            Window window = obj as Window;
            window.Close();
        }

        async Task GetSuppliers()
        {
            var supplier = await Api.GetListAsync<List<SupplierApi>>("Supplier");
            Suppliers = (List<SupplierApi>)supplier;
        }

        async Task GetProducts()
        {
            var products = await Api.GetListAsync<List<ProductApi>>("Product");
            Product = (List<ProductApi>)products;
        }

        async Task EditProduction(ProductApi productApi)
        {
            var prod = Api.PutAsync<ProductApi>(productApi, "Product");
        }

        async Task GetOrders(OrderOutApi orderOutApi)
        {
            Suppliers = await Api.GetListAsync<List<SupplierApi>>("Supplier");
            Product = await Api.GetListAsync<List<ProductApi>>("Product");
            if (orderOutApi == null)
            {
                SelectedSupplier = Suppliers.FirstOrDefault();
            }
            else
            {
                SelectedSupplier = Suppliers.FirstOrDefault(s => s.Id == orderOutApi.SupplierId);
            }
        }

        async Task PostOrder(OrderInApi orderInApi)
        {
            var order = await Api.PostAsync<OrderInApi>(orderInApi, "OrderIn");
        }

        async Task EditOrder(OrderInApi orderInApi)
        {
            var orderedit = await Api.PutAsync<OrderInApi>(orderInApi, "OrderIn");
        }

        public AddOrderOutViewModel(OrderOutApi orderOut)
        {
            GetSuppliers();
            GetProducts();

            if(orderOut == null)
            {
                AddOrderVM = new OrderOutApi { DateOrderOut = DateTime.Now, Status = "В норме" };
            }
            else
            {
                AddOrderVM = new OrderOutApi
                {
                    Id = orderOut.Id,
                    DateOrderOut = orderOut.DateOrderOut,
                    Status = orderOut.Status,
                    ShopId = orderOut.ShopId,
                    SupplierId = orderOut.SupplierId
                };

                if(orderOut.Products != null)
                {
                    SelectedOrderProducts = orderOut.Products;
                }
            }

            GetOrders(AddOrderVM);

            AddProduct = new CustomCommand(() =>
            {
                if(SelectedProduct == null)
                {
                    MessageBox.Show("Выберите продукцию!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else if (SelectedOrderProducts.Contains(SelectedProduct))
                {
                    MessageBox.Show("Данная продукция уже есть в заказе!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    SelectedOrderProduct = SelectedProduct;
                    SelectedOrderProduct.CountProducts = NewCross;
                    EditProduction(SelectedOrderProduct);
                    SelectedOrderProducts.Add(SelectedProduct);
                    SignalChanged("SelectedOrderProducts");
                }
            });

            SaveOrder = new CustomCommand(() =>
            {
                AddOrderVM.Products = SelectedOrderProducts;
                //SelectedOrderProduct
            });
        }
    }
}
