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
        public int CountAllProducts { get; set; }

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

        public string TimeWait { get; set; }
        public DateTime date { get; set; } = DateTime.Now;

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
        //public CustomCommand EditProduct { get; set; }
        public CustomCommand RemoveProduct { get; set; }
        public CustomCommand EditProductCount { get; set; }
        public CustomCommand EditProduct { get; set; }

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

        async Task GetCrosses()
        {
            var cross = await Api.GetListAsync<List<CrossProductOrderApi>>("CrossIn");
            SelectedCrosses = cross.Where(s => s.OrderInId == AddOrderVM.Id);
            //SelectedCross.Product = SelectedOrderProducts.FirstOrDefault(s => s.Id == SelectedCross.ProductId);
            foreach (CrossProductOrderApi crossProduct in SelectedCrosses)
            {
                crossProduct.Product = SelectedOrderProducts.FirstOrDefault(s => s.Id == crossProduct.ProductId);
                crossProduct.Product.ProductType = ProductTypes.FirstOrDefault(s => s.Id == crossProduct.Product.ProductTypeId);
                CountAllProducts += (int)crossProduct.CountInOrder;
                SignalChanged(nameof(CountAllProducts));
            }
        }

        async Task GetProducts()
        {
            var products = await Api.GetListAsync<List<ProductApi>>("Product");
            Product = (List<ProductApi>)products;
        }

        async Task EditProduction(ProductApi productApi)
        {
            var prod = await Api.PutAsync<ProductApi>(productApi, "Product");
        }

        async Task EditSupplier(SupplierApi supplier)
        {
            var supplierApi = await Api.PutAsync<SupplierApi>(supplier, "Supplier");
        }

        async Task GetOrders(OrderInApi orderInApi)
        {
            Suppliers = await Api.GetListAsync<List<SupplierApi>>("Supplier");
            Product = await Api.GetListAsync<List<ProductApi>>("Product");
            ProductTypes = await Api.GetListAsync<List<ProductTypeApi>>("ProductType");
            Units = await Api.GetListAsync<List<UnitApi>>("Unit");
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
                    crossProduct.Product.ProductType = ProductTypes.FirstOrDefault(s => s.Id == crossProduct.Product.ProductTypeId);
                    crossProduct.Product.Unit = Units.FirstOrDefault(s => s.Id == crossProduct.Product.UnitId);
                    CountAllProducts += (int)crossProduct.CountInOrder;
                    SignalChanged(nameof(CountAllProducts));
                }
            }
            //SignalChanged(nameof(SelectedCrosses));
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

            if (orderInApi == null)
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

            var time = AddOrderVM.DateOrderIn.Day - date.Day;
            TimeWait = $"{time * 24} часов";
            SignalChanged("TimeWait");
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
                    MessageBox.Show($"Продукт {SelectedProduct.Title} добавлен в накладную");
                    SelectedCrosses = AddOrderVM.CrossProductOrders;
                    SignalChanged("SelectedCrosses");
                }
            });

            SaveOrder = new CustomCommand(() =>
            {   
                if(AddOrderVM.Status == null)
                {
                    MessageBox.Show("Введите статус");
                    return;
                }
                AddOrderVM.Products = SelectedOrderProducts;
                SelectedProduct.CountProducts = NewCross;
                if (AddOrderVM.Id == 0)
                {
                    if(AddOrderVM.Products == null)
                    {
                        MessageBox.Show("Выберите продукцию для накладной");
                        return;
                    }
                    if(SelectedSupplier == null)
                    {
                        MessageBox.Show("Выберите поставщика");
                        return;
                    }
                    if(SelectedSupplier.Status == "Удален")
                    {
                        MessageBox.Show("Выберите другого поставщика");
                        return;
                    }
                    AddOrderVM.SupplierId = SelectedSupplier.Id;
                    if (AddOrderVM.SupplierId != 0)
                    {
                        MessageBoxResult result = MessageBox.Show($"Поставщик {SelectedSupplier.FirstName} Успешно доставил груз! Поднять ему рейтинг?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes)
                        {
                            SelectedSupplier.Rating += 10;
                            EditSupplier(SelectedSupplier);
                            MessageBox.Show($"Поставщику {SelectedSupplier.FirstName} был поднят рейтинг!");
                        }
                    }
                    PostOrder(AddOrderVM);
                }
                else
                {
                    if (SelectedSupplier.Status == "Удален")
                    {
                        MessageBox.Show("Выберите другого поставщика");
                        return;
                    }
                    AddOrderVM.SupplierId = SelectedSupplier.Id;
                    if(AddOrderVM.SupplierId != 0)
                    {
                        MessageBoxResult result = MessageBox.Show($"Поставщик {SelectedSupplier.FirstName} Успешно доставил груз! Поднять ему рейтинг?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes)
                        {
                            SelectedSupplier.Rating += 10;
                            EditSupplier(SelectedSupplier);
                            MessageBox.Show($"Поставщику {SelectedSupplier.FirstName} был поднят рейтинг!");
                        }
                    }
                    EditOrder(AddOrderVM);
                }

                if (AddOrderVM.Products != null)
                {
                    SelectedOrderProduct.CountProducts = NewCross;
                    //AddOrderVM.CrossProductOrderApi.CountInOrder = SelectedOrderProduct.CountProducts;
                    EditOrder(AddOrderVM);
                    EditProduction(SelectedProduct);
                    MessageBox.Show("Записано");
                }

                //foreach (Window window in Application.Current.Windows)
                //{
                //    if (window.DataContext == this)
                //    {
                //        CloseWindow(window);
                //    }
                //}
                SelectedCrosses = AddOrderVM.CrossProductOrders;
                SignalChanged("SelectedCrosses");
            });

            EditProduct = new CustomCommand(() =>
            {
                if (SelectedCross == null) return;
                else
                {
                    AddProduct product = new AddProduct(SelectedCross.Product);
                    product.ShowDialog();
                    MessageBox.Show("Переход на редактирование продукта");

                }
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
