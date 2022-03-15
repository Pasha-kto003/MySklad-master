using ModelApi;
using MySklad.Core;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private string selectedTypeOut { get; set; }
        public string SelectedTypeOut
        {
            get => selectedTypeOut;
            set
            {
                selectedTypeOut = value;
                SignalChanged();
            }
        }

        public List<string> Types { get; set; }
        public List<string> TypesOut { get; set; }

        public DateTime SelectedAfterDate { get; set; }
        public DateTime SelectedBeforeDate { get; set; }
        public DateTime SelectedDateStartPeriod { get; set; }
        public DateTime SelectedDateEndPeriod { get; set; }
        public DateTime SelectedDateStartOut { get; set; }
        public DateTime SelectedDateEndOut { get; set; }

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

        private SupplierApi selectedSupplier { get; set; }

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

        public CustomCommand CountAll { get; set; }
        public CustomCommand EditReport { get; set; }
        public CustomCommand EditReportOut { get; set; }

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

        async Task GetOrderOut()
        {
            OrdersOut = await Api.GetListAsync<List<OrderOutApi>>("OrderOut");
            Suppliers = await Api.GetListAsync<List<SupplierApi>>("Supplier");
            Shops = await Api.GetListAsync<List<ShopApi>>("Shop");
            foreach (OrderOutApi orderOut in OrdersOut)
            {
                orderOut.Supplier = Suppliers.First(s => s.Id == orderOut.SupplierId);
                orderOut.Shop = Shops.First(s => s.Id == orderOut.ShopId);
            }
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
        //async Task GetOrderOut()
        //{
        //    var orders = await Api.GetListAsync<List<OrderOutApi>>("OrderOut");
        //    OrdersOut = (List<OrderOutApi>)orders;
        //}
        async Task GetCompany()
        {
            var company = await Api.GetListAsync<List<CompanyApi>>("Company");
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
            GetCompany();

            Types = new List<string>
            {
                "Поставщик",
                "Период"
            };

            TypesOut = new List<string>
            {
                "Магазин",
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
                
                foreach (CrossProductOrderApi cross in CrossProductOrders.Where(s => s.ProductId != 0))
                {
                    ProductInOrderIn += (int)cross.CountInOrder;
                }
                foreach (CrossOrderOutApi cross in CrossProductOrdersOut.FindAll(s => s.ProductId != 0))
                {
                    ProductInOrderOut += (int)cross.CountOutOrder;
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

            EditReport = new CustomCommand(() =>
            {
                switch (SelectedType)
                {
                    case "Период":
                        ConvertReportToXLSByPeriod(SelectedDateStartPeriod, SelectedDateEndPeriod);
                        break;
                    case "Поставщик":
                        ConvertReportToXLSBySupplier(SelectedOrderIn);
                        break;
                }
            });

            EditReportOut = new CustomCommand(() =>
            {

            });
        }

        public void ConvertReportToXLSByPeriod(DateTime firstDate, DateTime lastDate)
        {
            GetProducts();
            GetOrderIn();
            GetCompany();
            var workBook = new Workbook();
            var sheet = workBook.Worksheets[0];
            sheet.Range["A1"].Value = $"С ";
            sheet.Range["B1"].Value = $" { firstDate.Date.ToShortDateString()}";
            sheet.Range["D1"].Value = $"{lastDate.Date.ToShortDateString()}";
            sheet.Range["C1"].Value = $"По ";

            sheet.Range["B4"].Value = "Дата накладной";
            sheet.Range["F4"].Value = "Поставщик выполнивший заказ";
            sheet.Range["C4"].Value = "Статус накалдной";
            sheet.Range["G4"].Value = "Товары";

            int index = 5;
            int count = 1;
            

            List<OrderInApi> OrderByPeriod = OrdersIn.FindAll(s => s.Id == s.Id).Where
                (
                    s => s.DateOrderIn >= firstDate && s.DateOrderIn <= lastDate &&
                    s.SupplierId == s.Supplier.Id
                ).ToList();

            foreach(var order in OrderByPeriod)
            {
                DateTime date = (DateTime)order.DateOrderIn;
                sheet.Range[$"A{index}"].NumberValue = count++;
                sheet.Range[$"B{index}"].Value = date.ToShortDateString();
                sheet.Range[$"C{index}"].Value = order.Status;
                sheet.Range[$"F{index}"].Value = order.Supplier.Title;
                sheet.Range[$"G5:AC5"].Value = order.Products.ToString();

                index++;
            }
            sheet.Range[$"A1:D1"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A1:D1"].BorderAround(LineStyleType.Medium);
            sheet.Range[$"A4:L{index - 1}"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A4:L{index - 1}"].BorderAround(LineStyleType.Medium);
            sheet.AllocatedRange.AutoFitColumns();
            workBook.SaveToFile("test.xls");

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(Environment.CurrentDirectory + "/" + "test.xls")
            {
                UseShellExecute = true
            };
            p.Start();
        }

        public void ConvertReportToXLSBySupplier(OrderInApi orderIn)
        {
            var workBook = new Workbook();
            var sheet = workBook.Worksheets[0];
            GetProducts();
            GetOrderIn();
            GetCompany();
            sheet.Range["B1"].Value = "Наименование поставщика";
            sheet.Range["C1"].Value = "Заказ";
            sheet.Range["D1"].Value = "Почта";
            sheet.Range["E1"].Value = "Телефон";
            sheet.Range["F1"].Value = "Рейтинг";
            sheet.Range["G4"].Value = "Наименование компании";

            List<OrderInApi> OrderBySupplier = OrdersIn.FindAll(s => s.Id == s.Id).Where
                (
                    s => s.Supplier == orderIn.Supplier && s.SupplierId == s.Supplier.Id
                ).ToList();
            int index = 5;
            int count = 1;
            foreach (var supp in OrderBySupplier)
            {
                sheet.Range[$"A{index}"].NumberValue = count++;
                sheet.Range[$"B{index}"].Value = supp.Supplier.Title;
                sheet.Range[$"C{index}"].Value = supp.Id.ToString();
                sheet.Range[$"D{index}"].Value = supp.Supplier.Email;
                sheet.Range[$"E{index}"].Value = supp.Supplier.Phone;
                sheet.Range[$"F{index}"].Value = supp.Supplier.CompanyId.ToString();
                index++;
            }
            sheet.Range[$"B1:E2"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"B1:E2"].BorderAround(LineStyleType.Medium);
            sheet.Range[$"A4:G{index - 1}"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A4:G{index - 1}"].BorderAround(LineStyleType.Medium);
            sheet.AllocatedRange.AutoFitColumns();
            workBook.SaveToFile("testsupp.xls");
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(Environment.CurrentDirectory + "/testsupp.xls")
            {
                UseShellExecute = true
            };
            p.Start();
        }

        public void ConvertReportOutToXLSByPeriod(DateTime firstDate, DateTime lastDate)
        {

        }
    }

    
}
