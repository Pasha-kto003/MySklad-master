﻿using ModelApi;
using MySklad.Core;
using MySklad.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySklad.ViewModel
{
    public class OrderOutViewModel : BaseViewModel
    {
        private List<OrderOutApi> orders { get; set; }
        public List<OrderOutApi> Orders
        {
            get => orders;
            set
            {
                orders = value;
                SignalChanged();
            }
        }

        private OrderOutApi selectedOrderOut { get; set; }
        public OrderOutApi SelectedOrderOut
        {
            get => selectedOrderOut;
            set
            {
                selectedOrderOut = value;
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

        private async Task GetOrders()
        {
            Products = await Api.GetListAsync<List<ProductApi>>("Product");
            Suppliers = await Api.GetListAsync<List<SupplierApi>>("Supplier");
            Orders = await Api.GetListAsync<List<OrderOutApi>>("OrderOut");
            Shops = await Api.GetListAsync<List<ShopApi>>("Shop");
            foreach (OrderOutApi order in Orders)
            {
                order.Supplier = Suppliers.First(s => s.Id == order.SupplierId);
                order.Shop = Shops.First(s => s.Id == order.ShopId);
            }
        }

        public CustomCommand CreateOrderOut { get; set; }
        public CustomCommand EditOrderOut { get; set; }

        public OrderOutViewModel()
        {
            Products = new List<ProductApi>();
            Suppliers = new List<SupplierApi>();
            Orders = new List<OrderOutApi>();
            Shops = new List<ShopApi>();
            

            CreateOrderOut = new CustomCommand(() =>
            {
                AddOrderOutView addOrder = new AddOrderOutView();
                addOrder.ShowDialog();
                GetOrders();
            });

            EditOrderOut = new CustomCommand(() =>
            {
                if (SelectedOrderOut == null) return;
                AddOrderOutView addOrder = new AddOrderOutView(SelectedOrderOut);
                addOrder.ShowDialog();
                GetOrders();
            });
            GetOrders();
        }
    }
}