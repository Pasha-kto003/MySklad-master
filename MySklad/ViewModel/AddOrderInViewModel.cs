using ModelApi;
using MySklad.Core;
using MySklad.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MySklad.ViewModel
{
    public class AddOrderInViewModel : BaseViewModel
    {
        public OrderInApi AddOrderVM { get; set; }

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

        private IEnumerable<CrossProductOrderApi> selectedCrosses { get; set; }
        public IEnumerable<CrossProductOrderApi> SelectedCrosses 
        {
            get => selectedCrosses;
            set
            {
                selectedCrosses = value;
                SignalChanged();
            }
        }

        private CrossProductOrderApi selectedCross { get; set; }
        public CrossProductOrderApi SelectedCross
        {
            get => selectedCross;
            set
            {
                selectedCross = value;
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

        async Task GetOrders(OrderInApi orderInApi)
        {
            Suppliers = await Api.GetListAsync<List<SupplierApi>>("Supplier");
            Product = await Api.GetListAsync<List<ProductApi>>("Product");
            var cross = await Api.GetListAsync<List<CrossProductOrderApi>>("CrossIn");

            if(orderInApi == null)
            {
                SelectedSupplier = Suppliers.FirstOrDefault();
            }
            else
            {
                SelectedSupplier = Suppliers.FirstOrDefault(s => s.Id == orderInApi.SupplierId);
                SelectedCrosses = cross.Where(s => s.OrderInId == AddOrderVM.Id);
                //SelectedCross.Product = SelectedOrderProducts.FirstOrDefault(s => s.Id == SelectedCross.ProductId);
                foreach(CrossProductOrderApi crossProduct in SelectedCrosses)
                {
                    crossProduct.Product = SelectedOrderProducts.FirstOrDefault(s => s.Id == crossProduct.ProductId);
                }
            }
        }

        public List<CrossProductOrderApi> CrossProductOrders { get; set; }
        public List<CrossOrderOutApi> CrossOrderOuts { get; set; }

        async Task PostOrder(OrderInApi orderInApi)
        {
            var order = await Api.PostAsync<OrderInApi>(orderInApi,"OrderIn");
        }

        async Task EditOrder(OrderInApi orderInApi)
        {
            var orderedit = await Api.PutAsync<OrderInApi>(orderInApi, "OrderIn");
        }

        public AddOrderInViewModel(OrderInApi orderInApi)
        {
            GetSuppliers();
            GetProducts();

            if(orderInApi == null)
            {
                AddOrderVM = new OrderInApi { DateOrderIn = DateTime.Now, Status = "В норме" };
            }
            else
            {
                AddOrderVM = new OrderInApi
                {
                    Id = orderInApi.Id,
                    DateOrderIn = orderInApi.DateOrderIn,
                    Status = orderInApi.Status,
                    SupplierId = orderInApi.SupplierId
                };

                if (orderInApi.Products != null)
                {
                    SelectedOrderProducts = orderInApi.Products;
                }
            }
            
            GetOrders(AddOrderVM);

            AddProduct = new CustomCommand(() =>
            {
                if (SelectedProduct == null)
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
                    //EditProduction(SelectedOrderProduct);
                    SelectedOrderProducts.Add(SelectedProduct);
                    SignalChanged("SelectedOrderProducts");
                }
            });



            SaveOrder = new CustomCommand(() =>
            {
                
                AddOrderVM.Products = SelectedOrderProducts;
                SelectedOrderProduct.CountProducts = NewCross;
                if (AddOrderVM.Id == 0)
                {
                    AddOrderVM.SupplierId = SelectedSupplier.Id;
                    PostOrder(AddOrderVM);
                }
                else
                {
                    AddOrderVM.SupplierId = SelectedSupplier.Id;
                    EditOrder(AddOrderVM);
                }

                if (AddOrderVM.Products != null)
                {
                    SelectedOrderProduct.CountProducts = NewCross;
                    //AddOrderVM.CrossProductOrderApi.CountInOrder = SelectedOrderProduct.CountProducts;
                    EditOrder(AddOrderVM);
                    EditProduction(SelectedOrderProduct);
                    MessageBox.Show("Записано");
                }

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.DataContext == this)
                    {
                        CloseWindow(window);
                    }
                }
                SignalChanged("OrderIns");
            });
            EditProductCount = new CustomCommand(() =>
            {
                if(SelectedOrderProduct == null)
                {
                    MessageBox.Show("Выберите продукцию");
                    return;
                }
                else
                {
                    EditProductCount prod = new EditProductCount(SelectedOrderProduct);                
                    prod.ShowDialog();
                    EditProduction(SelectedOrderProduct);
                }
            });
        }
    }
}
