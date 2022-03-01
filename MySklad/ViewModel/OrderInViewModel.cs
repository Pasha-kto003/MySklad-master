using ModelApi;
using MySklad.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySklad.ViewModel
{
    public class OrderInViewModel : BaseViewModel
    {
        private List<OrderInApi> orders { get; set; }
        public List<OrderInApi> Orders
        {
            get => orders;
            set
            {
                orders = value;
                SignalChanged();
            }
        }

        private OrderInApi selectedOrderIn { get; set; }
        public OrderInApi SelectedOrderIn
        {
            get => selectedOrderIn;
            set
            {
                selectedOrderIn = value;
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

        public CrossProductOrderApi CrossProductOrder { get; set; }

        public CustomCommand EditOrderIn { get; set; }
        public CustomCommand CreateOrderIn { get; set; }

        private async Task GetOrders()
        {
            Products = await Api.GetListAsync<List<ProductApi>>("Product");
            Suppliers = await Api.GetListAsync<List<SupplierApi>>("Supplier");
            Orders = await Api.GetListAsync<List<OrderInApi>>("OrderIn");
            foreach(OrderInApi orderIn in Orders)
            {
                orderIn.Supplier = Suppliers.First(s => s.Id == orderIn.SupplierId);
                //orderIn.CrossProductOrderApi.CountInOrder = CrossProductOrder.CountInOrder;
            }
        }

        public OrderInViewModel()
        {
            Products = new List<ProductApi>();
            Suppliers = new List<SupplierApi>();
            Orders = new List<OrderInApi>();
            GetOrders();
        }

    }   
}
