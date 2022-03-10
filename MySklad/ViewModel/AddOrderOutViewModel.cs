﻿using ModelApi;
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
    public class AddOrderOutViewModel : BaseViewModel
    {
        public OrderOutApi AddOrderVM { get; set; }
        public int Cross { get; set; }
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

        public List<CrossProductOrderApi> CrossProductOrders { get; set; }
        public List<CrossOrderOutApi> CrossOrderOuts { get; set; }

        private List<ShopApi> shops { get; set; }
        public List<ShopApi> Shops
        {
            get => shops;
            set
            {
                shops = value;
                SignalChanged();
            }
        }

        private ShopApi selectedShop { get; set; }
        public ShopApi SelectedShop { get; set; }

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

        async Task GetShops()
        {
            var shops = await Api.GetListAsync<List<ShopApi>>("Shop");
            Shops = (List<ShopApi>)shops;
        }

        async Task GetOrders(OrderOutApi orderOutApi)
        {
            Suppliers = await Api.GetListAsync<List<SupplierApi>>("Supplier");
            Shops = await Api.GetListAsync<List<ShopApi>>("Shop");
            Product = await Api.GetListAsync<List<ProductApi>>("Product");
            CrossProductOrders = await Api.GetListAsync<List<CrossProductOrderApi>>("CrossIn");
            CrossOrderOuts = await Api.GetListAsync<List<CrossOrderOutApi>>("OrderOut");
            if (orderOutApi == null)
            {
                SelectedSupplier = Suppliers.FirstOrDefault();
                SelectedShop = Shops.FirstOrDefault();
            }
            else
            {
                SelectedSupplier = Suppliers.FirstOrDefault(s => s.Id == orderOutApi.SupplierId);
                SelectedShop = Shops.FirstOrDefault(s => s.Id == orderOutApi.ShopId);
            }
        }

        async Task PostOrder(OrderOutApi orderOut)
        {
            var order = await Api.PostAsync<OrderOutApi>(orderOut, "OrderOut");
        }

        async Task EditOrder(OrderOutApi orderOut)
        {
            var orderedit = await Api.PutAsync<OrderOutApi>(orderOut, "OrderOut");
        }

        public AddOrderOutViewModel(OrderOutApi orderOut)
        {
            GetSuppliers();
            GetProducts();
            GetShops();

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
                    SelectedOrderProduct.CountProductsOut = Cross;
                    EditProduction(SelectedOrderProduct);
                    SelectedOrderProduct.CrossProductOrderApi = CrossProductOrders.FirstOrDefault(s => s.ProductId == SelectedOrderProduct.Id);
                    if (SelectedOrderProduct.Status == "Удален")
                    {
                        MessageBox.Show("Ошибка, этот продукт находится в реестре удалений");
                        SelectedOrderProducts.Remove(SelectedOrderProduct);
                    }
                    else if (SelectedOrderProduct.CountProductsOut > SelectedOrderProduct.CrossProductOrderApi.CountInOrder || SelectedOrderProduct.CountProductsOut > SelectedOrderProduct.CountInStock)
                    {
                        MessageBox.Show("Ошибка, нельзя увезти товаров больше чем его привезли");
                        SelectedOrderProducts.Remove(SelectedOrderProduct);
                        return;
                    }                   
                    SelectedOrderProducts.Add(SelectedProduct);
                    SignalChanged("SelectedOrderProducts");
                }
            });

            SaveOrder = new CustomCommand(() =>
            {
                AddOrderVM.Products = SelectedOrderProducts;
                SelectedOrderProduct.CountProductsOut = Cross;
                
                if (AddOrderVM.Id == 0)
                {
                    AddOrderVM.SupplierId = SelectedSupplier.Id;
                    AddOrderVM.ShopId = SelectedShop.Id;
                    PostOrder(AddOrderVM);
                }
                else
                {
                    AddOrderVM.SupplierId = SelectedSupplier.Id;
                    AddOrderVM.ShopId = SelectedShop.Id;
                    EditOrder(AddOrderVM);
                }

                if(AddOrderVM.Products != null)
                {
                    SelectedOrderProduct.CountProductsOut = Cross;
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
                SignalChanged("OrderOuts");
            });
            EditProductCount = new CustomCommand(() =>
            {
                if (SelectedOrderProduct == null)
                {
                    MessageBox.Show("Выберите продукцию");
                    return;
                }
                else
                {
                    EditProductCountOut prod = new EditProductCountOut(SelectedOrderProduct);
                    prod.ShowDialog();
                    EditProduction(SelectedOrderProduct);
                }
            });
        }
    }
}