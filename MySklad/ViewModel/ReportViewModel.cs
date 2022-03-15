using ModelApi;
using MySklad.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySklad.ViewModel
{
    public class ReportViewModel : BaseViewModel
    {
        private string selectedType { get; set; }
        public string SelectedType
        {
            get => selectedType;
            set
            {
                selectedType = value;
            }
        }
        public List<string> Types { get; set; }

        public DateTime SelectedAfterDate { get; set; }
        public DateTime SelectedBeforeDate { get; set; }
        public DateTime SelectedDateStartPeriod { get; set; }
        public DateTime SelectedDateEndPeriod { get; set; }

        public int OrderInCount { get; set; }
        public int OrderOutCount { get; set; }
        public int ProductInOrderIn { get; set; }
        public int ProductInOrderOut { get; set; }
        public int RackCount { get; set; }
        public int ProductCount { get; set; }

        public ProductApi AddProductVM { get; set; }

        private List<OrderInApi> ordersIn { get; set; }
        public List<OrderInApi> OrdersIn
        {
            get => ordersIn;
            set
            {
                ordersIn = value;
                SignalChanged();
            }
        }

        private List<OrderOutApi> ordersOut { get; set; }
        public List<OrderOutApi> OrdersOut
        {
            get => ordersOut;
            set
            {
                ordersOut = value;
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

        private List<CrossProductOrderApi> crossProductOrders { get; set; }
        public List<CrossProductOrderApi> CrossProductOrders
        {
            get => crossProductOrders;
            set
            {
                crossProductOrders = value;
                SignalChanged();
            }
        }

        private List<CrossOrderOutApi> crossProductOrdersOut { get; set; }
        public List<CrossOrderOutApi> CrossProductOrdersOut
        {
            get => crossProductOrdersOut;
            set
            {
                crossProductOrdersOut = value;
                SignalChanged();
            }
        }

        private List<RackApi> racks { get; set; }
        public List<RackApi> Racks
        {
            get => racks;
            set
            {
                racks = value;
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

        public CustomCommand CountAll { get; set; }
        public CustomCommand EditReport { get; set; }

        async Task GetProducts()
        {
            var product = await Api.GetListAsync<List<ProductApi>>("Product");
            Products = (List<ProductApi>)product;

        }
        async Task GetOrderIn()
        {
            //var order = await Api.GetListAsync<List<OrderInApi>>("OrderIn");
            OrdersIn = await Api.GetListAsync<List<OrderInApi>>("OrderIn");
            Suppliers = await Api.GetListAsync<List<SupplierApi>>("Supplier");
            foreach(OrderInApi orderIn in OrdersIn)
            {
                orderIn.Supplier = Suppliers.First(s => s.Id == orderIn.SupplierId);
            }
            //OrdersIn = (List<OrderInApi>)order;
        }
        async Task GetCrossIn()
        {
            var cross = await Api.GetListAsync<List<CrossProductOrderApi>>("CrossIn");
            CrossProductOrders = (List<CrossProductOrderApi>)cross;
        }
        async Task GetCrossOut()
        {
            var crosses = await Api.GetListAsync<List<CrossOrderOutApi>>("CrossOut");
            CrossProductOrdersOut = (List<CrossOrderOutApi>)crosses;
        }
        async Task GetOrderOut()
        {
            var orders = await Api.GetListAsync<List<OrderOutApi>>("OrderOut");
            OrdersOut = (List<OrderOutApi>)orders;
        }
        async Task GetRacks()
        {
            var rack = await Api.GetListAsync<List<RackApi>>("Rack");
            Racks = (List<RackApi>)rack;
        }
        //async Task GetSuppliers()
        //{
        //    var suppliers = await Api.GetListAsync<List<SupplierApi>>("Supplier");
        //}

        public ReportViewModel()
        {
            GetProducts();
            GetOrderIn();
            GetOrderOut();
            GetRacks();
            GetCrossIn();
            GetCrossOut();

            Types = new List<string>
            {
                "Поставщик",
                "Период"
            };

            CountAll = new CustomCommand(() =>
            {
                OrderInCount = OrdersIn.FindAll(s => s.Id == s.Id).Where
                (
                    s => s.DateOrderIn <= SelectedAfterDate && s.DateOrderIn >= SelectedBeforeDate
                ).Count();

                OrderOutCount = OrdersOut.FindAll(s => s.Id == s.Id).Where
                (
                    s=> s.DateOrderOut >= SelectedAfterDate && s.DateOrderOut <= SelectedBeforeDate
                ).Count();
                //foreach(ProductApi productApi in Products)
                //{
                //    ProductCount += productApi.CountInStock;
                //}
                //ProductInOrderIn = CrossProductOrders.FindAll(s => s.ProductId == s.ProductId).Count();
                foreach (CrossProductOrderApi cross in CrossProductOrders.Where(s => s.ProductId != 0))
                {
                    ProductInOrderIn += (int)cross.CountInOrder / 2;
                }
                foreach (CrossOrderOutApi cross in CrossProductOrdersOut.FindAll(s => s.ProductId != 0))
                {
                    ProductInOrderOut += (int)cross.CountOutOrder / 2;
                }

                ProductCount = ProductInOrderIn - ProductInOrderOut;

                RackCount = Racks.FindAll(s => s.Id == s.Id).Where
                (
                    s=> s.PlacementDate >= SelectedAfterDate && s.PlacementDate <= SelectedBeforeDate
                ).Count();

                SignalChanged(nameof(OrderInCount));
                SignalChanged(nameof(OrderOutCount));
                SignalChanged(nameof(ProductCount));
                SignalChanged(nameof(ProductInOrderIn));
                SignalChanged(nameof(ProductInOrderOut));
                SignalChanged(nameof(RackCount));
            });
        }
    }
}
