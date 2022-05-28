using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using ModelApi;
using MySklad.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySklad.ViewModel
{
    public class DiagramViewModel : BaseViewModel
    {
        public List<CrossProductOrderApi> CrossProductOrders { get; set; }
        public List<CrossOrderOutApi> CrossProductOrdersOut { get; set; }
        public List<OrderInApi> OrdersIn { get; set; }
        public List<OrderOutApi> OrdersOut { get; set; }
        public List<SupplierApi> Suppliers { get; set; }
        public List<ProductApi> Products { get; set; }
        public int CountInOrderIn { get; set; }
        public int CountInOrderOut { get; set; }

        public ISeries[] Series { get; set; }

        async Task GetCrossIn()
        {
            CrossProductOrders = await Api.GetListAsync<List<CrossProductOrderApi>>("CrossIn");
            OrdersIn = await Api.GetListAsync<List<OrderInApi>>("OrderIn");
            Suppliers = await Api.GetListAsync<List<SupplierApi>>("Supplier");
            Products = await Api.GetListAsync<List<ProductApi>>("Product");
            foreach (CrossProductOrderApi crossProduct in CrossProductOrders)
            {
                crossProduct.OrderIn = OrdersIn.First(s => s.Id == crossProduct.OrderInId);
                crossProduct.OrderIn.Supplier = Suppliers.First(s => s.Id == crossProduct.OrderIn.SupplierId);
                crossProduct.Product = Products.First(s => s.Id == crossProduct.ProductId);
            }
        }

        async Task GetCrossOut()
        {
            CrossProductOrdersOut = await Api.GetListAsync<List<CrossOrderOutApi>>("CrossOut");
            OrdersOut = await Api.GetListAsync<List<OrderOutApi>>("OrderOut");
            Suppliers = await Api.GetListAsync<List<SupplierApi>>("Supplier");
            Products = await Api.GetListAsync<List<ProductApi>>("Product");
            foreach (CrossOrderOutApi crossProduct in CrossProductOrdersOut)
            {
                crossProduct.OrderOut = OrdersOut.First(s => s.Id == crossProduct.OrderOutId);
                crossProduct.OrderOut.Supplier = Suppliers.First(s => s.Id == crossProduct.OrderOut.SupplierId);
                crossProduct.Product = Products.First(s => s.Id == crossProduct.ProductId);
            }
        }

        public async Task Counted()
        {
            CrossProductOrders = await Api.GetListAsync<List<CrossProductOrderApi>>("CrossIn");
            CrossProductOrdersOut = await Api.GetListAsync<List<CrossOrderOutApi>>("CrossOut");
            foreach (CrossProductOrderApi cross in CrossProductOrders.Where(s => s.ProductId != 0))
            {
                if (cross.CountInOrder != null)
                {
                    CountInOrderIn += (int)cross.CountInOrder / 2;
                }
            }
            foreach (CrossOrderOutApi cross in CrossProductOrdersOut.FindAll(s => s.ProductId != 0))
            {
                CountInOrderOut += (int)cross.CountOutOrder / 2;
            }
        }

        public DiagramViewModel()
        {
            GetCrossIn();
            GetCrossOut();

            Counted();
            Series = new ISeries[]
            {
                new PieSeries<int> {Name = "Товары", Values = new int[] { CountInOrderIn } },
                new PieSeries<int> {Name = "Товары", Values = new int[] { CountInOrderOut } }
            };
        }


        
    }
}
