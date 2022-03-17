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

        private string selectedTypeProduct { get; set; }
        public string SelectedTypeProduct
        {
            get => selectedTypeProduct;
            set
            {
                selectedTypeProduct = value;
                SignalChanged();
            }
        }

        private string selectedTypeWriteOff { get; set; }
        public string SelectedTypeWriteOff
        {
            get => selectedTypeWriteOff;
            set
            {
                selectedTypeWriteOff = value;
                SignalChanged();
            }
        }

        public List<string> Types { get; set; }
        public List<string> TypesOut { get; set; }
        public List<string> TypesProduct { get; set; }
        public List<string> TypesWriteOff { get; set; }

        public DateTime SelectedAfterDate { get; set; }
        public DateTime SelectedBeforeDate { get; set; }
        public DateTime SelectedDateStartPeriod { get; set; }
        public DateTime SelectedDateEndPeriod { get; set; }
        public DateTime SelectedDateStartOut { get; set; }
        public DateTime SelectedDateEndOut { get; set; }
        public DateTime SelectedDateStartProduct { get; set; }
        public DateTime SelectedDateEndProduct { get; set; }
        public DateTime SelectedDateStartWriteOff { get; set; }
        public DateTime SelectedDateEndWriteOff { get; set; }

        public int OrderInCount { get; set; }
        public int OrderOutCount { get; set; }
        public int ProductInOrderIn { get; set; }
        public int ProductInOrderOut { get; set; }
        public int RackCount { get; set; }
        public int ProductCount { get; set; }
        public int ProductDeleteCount { get; set; }

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

        private List<WriteOffRegisterApi> writeOffRegisters { get; set; }
        public List<WriteOffRegisterApi> WriteOffRegisters
        {
            get => writeOffRegisters;
            set
            {
                writeOffRegisters = value;
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

        private List<CompanyApi> companies { get; set; }
        public List<CompanyApi> Companies
        {
            get => companies;
            set
            {
                companies = value;
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

        private WriteOffRegisterApi selectedWriteOffRegister { get; set; }
        public WriteOffRegisterApi SelectedWriteOffRegister
        {
            get => selectedWriteOffRegister;
            set
            {
                selectedWriteOffRegister = value;
                SignalChanged();
            }
        }

        public CustomCommand CountAll { get; set; }
        public CustomCommand EditReport { get; set; }
        public CustomCommand EditReportOut { get; set; }
        public CustomCommand EditReportProduct { get; set; }
        public CustomCommand EditReportWriteOff { get; set; }

        async Task GetProducts()
        {
            Products = await Api.GetListAsync<List<ProductApi>>("Product");
            Units = await Api.GetListAsync<List<UnitApi>>("Unit");
            ProductTypes = await Api.GetListAsync<List<ProductTypeApi>>("ProductType");
            foreach(ProductApi product in Products)
            {
                product.Unit = Units.First(s => s.Id == product.UnitId);
                product.ProductType = ProductTypes.First(s => s.Id == product.ProductTypeId);
            }
        }

        async Task GetOrderIn()
        {
            //var order = await Api.GetListAsync<List<OrderInApi>>("OrderIn");
            OrdersIn = await Api.GetListAsync<List<OrderInApi>>("OrderIn");
            Suppliers = await Api.GetListAsync<List<SupplierApi>>("Supplier");
            foreach (OrderInApi orderIn in OrdersIn)
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
            Companies = await Api.GetListAsync<List<CompanyApi>>("Company");
            foreach (OrderOutApi orderOut in OrdersOut)
            {
                orderOut.Supplier = Suppliers.First(s => s.Id == orderOut.SupplierId);
                orderOut.Shop = Shops.First(s => s.Id == orderOut.ShopId);
                orderOut.Supplier.Company = Companies.First(s => s.Id == orderOut.Supplier.CompanyId);
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

        async Task GetWriteOffRegisters()
        {
            WriteOffRegisters = await Api.GetListAsync<List<WriteOffRegisterApi>>("WriteOffRegister");
            Products = await Api.GetListAsync<List<ProductApi>>("Product");
            foreach(WriteOffRegisterApi writeOffRegister in WriteOffRegisters)
            {
                writeOffRegister.Product = Products.First(s => s.Id == writeOffRegister.ProductId);
            }
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
            GetWriteOffRegisters();

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

            TypesProduct = new List<string>
            {
                "Период"
            };

            TypesWriteOff = new List<string>
            {
                "Период",
                "Продукт"
            };

            CountAll = new CustomCommand(() =>
            {
                OrderInCount = OrdersIn.FindAll(s => s.Id == s.Id).Where
                (
                    s => s.DateOrderIn >= SelectedAfterDate && s.DateOrderIn <= SelectedBeforeDate
                ).Count();

                OrderOutCount = OrdersOut.FindAll(s => s.Id == s.Id).Where
                (
                    s => s.DateOrderOut >= SelectedAfterDate && s.DateOrderOut <= SelectedBeforeDate
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
                    s => s.PlacementDate >= SelectedAfterDate && s.PlacementDate <= SelectedBeforeDate
                ).Count();

                foreach(WriteOffRegisterApi writeOffRegister in WriteOffRegisters.FindAll(s=> s.ProductId != 0))
                {
                    ProductDeleteCount += writeOffRegister.Product.CountInStock;
                }

                //ProductDeleteCount = WriteOffRegisters.FindAll(s => s.Id == s.Id).Where
                //(
                //    s=> s.DeletedAt >= SelectedAfterDate && s.DeletedAt <= SelectedBeforeDate
                //).Count();

                SignalChanged(nameof(OrderInCount));
                SignalChanged(nameof(OrderOutCount));
                SignalChanged(nameof(ProductCount));
                SignalChanged(nameof(ProductDeleteCount));
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
                switch (SelectedTypeOut)
                {
                    case "Период":
                        ConvertReportOutToXLSByPeriod(SelectedDateStartOut, SelectedDateEndOut);
                        break;
                    case "Поставщик":
                        ConvertReportOutToXLSBySupplier(SelectedOrderOut);
                        break;
                    case "Магазин":
                        ConvertReportOutToXLSByShop(SelectedOrderOut);
                        break;
                }
            });

            EditReportProduct = new CustomCommand(() =>
            {
                switch (SelectedTypeProduct)
                {
                    case "Период":
                        ConvertProductToXLSByPeriod(SelectedDateStartProduct, SelectedDateEndProduct);
                        break;
                }
            });

            EditReportWriteOff = new CustomCommand(() =>
            {
                switch (SelectedTypeWriteOff)
                {
                    case "Период":
                        ConvertWriteOffToXLSByPeriod(SelectedDateStartWriteOff, SelectedDateEndWriteOff);
                        break;
                    case "Продукт":
                        ConvertWriteOffToXLSByProduct(SelectedWriteOffRegister);
                        break;
                }
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

            foreach (var order in OrderByPeriod)
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
            GetProducts();
            GetOrderOut();

            var workBook = new Workbook();
            var sheet = workBook.Worksheets[0];
            sheet.Range["A1"].Value = $"С ";
            sheet.Range["B1"].Value = $" { firstDate.Date.ToShortDateString()}";
            sheet.Range["D1"].Value = $"{lastDate.Date.ToShortDateString()}";
            sheet.Range["C1"].Value = $"По ";

            sheet.Range["B4"].Value = "Дата расходной";
            sheet.Range["D4"].Value = "Поставщик выполнивший заказ";
            sheet.Range["C4"].Value = "Статус расходной";
            sheet.Range["E4"].Value = "Точка доставки";
            sheet.Range["F4"].Value = "Товары";

            int index = 5;
            int count = 1;

            List<OrderOutApi> OrderOutByPeriod = OrdersOut.FindAll(s => s.Id == s.Id).Where
                (
                    s => s.DateOrderOut >= firstDate && s.DateOrderOut <= lastDate &&
                    s.SupplierId == s.Supplier.Id && s.ShopId == s.Shop.Id
                ).ToList();

            foreach (var order in OrderOutByPeriod)
            {
                DateTime date = (DateTime)order.DateOrderOut;
                sheet.Range[$"A{index}"].NumberValue = count++;
                sheet.Range[$"B{index}"].Value = date.ToShortDateString();
                sheet.Range[$"C{index}"].Value = order.Status;
                sheet.Range[$"D{index}"].Value = order.Supplier.Title;
                sheet.Range[$"E{index}"].Value = order.Shop.Name;
                sheet.Range[$"F5:AC5"].Value = order.Products.ToString();

                index++;
            }
            sheet.Range[$"A1:D1"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A1:D1"].BorderAround(LineStyleType.Medium);
            sheet.Range[$"A4:L{index - 1}"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A4:L{index - 1}"].BorderAround(LineStyleType.Medium);
            sheet.AllocatedRange.AutoFitColumns();

            workBook.SaveToFile("testOrderOut.xls");

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(Environment.CurrentDirectory + "/" + "testOrderOut.xls")
            {
                UseShellExecute = true
            };
            p.Start();
        }

        public void ConvertReportOutToXLSBySupplier(OrderOutApi orderOut)
        {
            var workBook = new Workbook();
            var sheet = workBook.Worksheets[0];
            GetProducts();
            GetOrderOut();
            sheet.Range["B1"].Value = "Наименование поставщика";
            sheet.Range["C1"].Value = "Заказ";
            sheet.Range["D1"].Value = "Почта";
            sheet.Range["E1"].Value = "Телефон";
            sheet.Range["F1"].Value = "Рейтинг";
            sheet.Range["G4"].Value = "Наименование компании";

            List<OrderInApi> OrderOutByPeriod = OrdersIn.FindAll(s => s.Id == s.Id).Where
                (
                    s => s.Supplier == orderOut.Supplier && s.SupplierId == s.Supplier.Id
                ).ToList();
            int index = 5;
            int count = 1;

            foreach (var supp in OrderOutByPeriod)
            {
                sheet.Range[$"A{index}"].NumberValue = count++;
                sheet.Range[$"B{index}"].Value = supp.Supplier.Title;
                sheet.Range[$"C{index}"].Value = supp.Id.ToString();
                sheet.Range[$"D{index}"].Value = supp.Supplier.Email;
                sheet.Range[$"E{index}"].Value = supp.Supplier.Phone;
                sheet.Range[$"F{index}"].Value = supp.Supplier.Rating.ToString();
                sheet.Range[$"G{index}"].Value = supp.Supplier.Company.NameOfCompany;
                index++;
            }
            sheet.Range[$"B1:E2"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"B1:E2"].BorderAround(LineStyleType.Medium);
            sheet.Range[$"A4:G{index - 1}"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A4:G{index - 1}"].BorderAround(LineStyleType.Medium);
            sheet.AllocatedRange.AutoFitColumns();
            workBook.SaveToFile("testOrderOutSupp.xls");
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(Environment.CurrentDirectory + "/testOrderOutSupp.xls")
            {
                UseShellExecute = true
            };
            p.Start();
        }

        public void ConvertReportOutToXLSByShop(OrderOutApi orderOutApi)
        {
            var workBook = new Workbook();
            var sheet = workBook.Worksheets[0];
            GetProducts();
            GetOrderOut();
            sheet.Range["B1"].Value = "Наименование магазина";
            sheet.Range["C1"].Value = "Заказ";
            sheet.Range["D1"].Value = "Почта";
            sheet.Range["E1"].Value = "Телефон";

            List<OrderOutApi> OrderOutByPeriod = OrdersOut.FindAll(s => s.Id == s.Id).Where
                (
                    s => s.Supplier == orderOutApi.Supplier && s.SupplierId == s.Supplier.Id &&
                    s.Shop == orderOutApi.Shop && s.ShopId == s.Shop.Id
                ).ToList();
            int index = 5;
            int count = 1;

            foreach (var supp in OrderOutByPeriod)
            {
                sheet.Range[$"A{index}"].NumberValue = count++;
                sheet.Range[$"B{index}"].Value = supp.Shop.Name;
                sheet.Range[$"C{index}"].Value = supp.Id.ToString();
                sheet.Range[$"D{index}"].Value = supp.Shop.Email;
                sheet.Range[$"E{index}"].Value = supp.Shop.Phone;
                index++;
            }
            sheet.Range[$"B1:E2"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"B1:E2"].BorderAround(LineStyleType.Medium);
            sheet.Range[$"A4:G{index - 1}"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A4:G{index - 1}"].BorderAround(LineStyleType.Medium);
            sheet.AllocatedRange.AutoFitColumns();
            workBook.SaveToFile("testOrderOutShop.xls");
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(Environment.CurrentDirectory + "/testOrderOutShop.xls")
            {
                UseShellExecute = true
            };
            p.Start();
        }

        public void ConvertProductToXLSByPeriod(DateTime firstDate, DateTime lastDate)
        {
            var workBook = new Workbook();
            var sheet = workBook.Worksheets[0];
            GetProducts();
            sheet.Range["A1"].Value = $"С ";
            sheet.Range["B1"].Value = $" { firstDate.Date.ToShortDateString()}";
            sheet.Range["D1"].Value = $"{lastDate.Date.ToShortDateString()}";
            sheet.Range["C1"].Value = $"По ";

            sheet.Range["B4"].Value = "Название продукции";
            sheet.Range["C4"].Value = "Описание продукции";
            sheet.Range["D4"].Value = "Дата начала срока годности";
            sheet.Range["E4"].Value = "Конец срока годности";
            sheet.Range["F4"].Value = "Остаток";
            sheet.Range["G4"].Value = "Тип продукции";
            sheet.Range["H4"].Value = "Единиица измерения";
            sheet.Range["I4"].Value = "Статус";

            int index = 5;
            int count = 1;

            List<ProductApi> ProductByPeriod = Products.FindAll(s => s.Id == s.Id).Where
                (
                    s => s.BestBeforeDateStart >= firstDate && s.BestBeforeDateStart <= lastDate &&
                    s.ProductTypeId == s.ProductType.Id && s.UnitId == s.Unit.Id
                ).ToList();

            foreach(var product in ProductByPeriod)
            {
                DateTime date = (DateTime)product.BestBeforeDateStart;
                DateTime dateEnd = (DateTime)product.BestBeforeDateEnd;
                sheet.Range[$"A{index}"].NumberValue = count++;
                sheet.Range[$"B{index}"].Value = product.Title;
                sheet.Range[$"C{index}"].Value = product.Description;
                sheet.Range[$"D{index}"].Value = date.ToShortDateString();
                sheet.Range[$"E{index}"].Value = dateEnd.ToShortDateString();
                sheet.Range[$"F{index}"].Value = product.CountInStock.ToString();
                sheet.Range[$"G{index}"].Value = product.ProductType.Title;
                sheet.Range[$"H{index}"].Value = product.Unit.Title;
                sheet.Range[$"I{index}"].Value = product.Status;

                index++;
            }

            sheet.Range[$"A1:D1"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A1:D1"].BorderAround(LineStyleType.Medium);
            sheet.Range[$"A4:L{index - 1}"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A4:L{index - 1}"].BorderAround(LineStyleType.Medium);
            sheet.AllocatedRange.AutoFitColumns();
            workBook.SaveToFile("testProduct.xls");

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(Environment.CurrentDirectory + "/" + "testProduct.xls")
            {
                UseShellExecute = true
            };
            p.Start();
        }

        public void ConvertWriteOffToXLSByPeriod(DateTime firstDate, DateTime lastDate)
        {
            GetProducts();
            GetWriteOffRegisters();

            var workBook = new Workbook();
            var sheet = workBook.Worksheets[0];
            sheet.Range["A1"].Value = $"С ";
            sheet.Range["B1"].Value = $" { firstDate.Date.ToShortDateString()}";
            sheet.Range["D1"].Value = $"{lastDate.Date.ToShortDateString()}";
            sheet.Range["C1"].Value = $"По ";

            sheet.Range["B4"].Value = "Название";
            sheet.Range["C4"].Value = "Причина удаления";
            sheet.Range["D4"].Value = "Дата удаления";
            sheet.Range["E4"].Value = "Продукция";

            int index = 5;
            int count = 1;

            List<WriteOffRegisterApi> WriteOffRegisterByPeriod = WriteOffRegisters.FindAll(s => s.Id == s.Id).Where
                (
                    s => s.DeletedAt >= firstDate && s.DeletedAt <= lastDate &&
                    s.ProductId == s.ProductId
                ).ToList();

            foreach(var writeOffRegister in WriteOffRegisterByPeriod)
            {
                DateTime date = (DateTime)writeOffRegister.DeletedAt;
                sheet.Range[$"A{index}"].NumberValue = count++;
                sheet.Range[$"B{index}"].Value = writeOffRegister.Title;
                sheet.Range[$"C{index}"].Value = writeOffRegister.ReasonDelete;
                sheet.Range[$"D{index}"].Value = date.ToShortDateString();
                sheet.Range[$"E{index}"].Value = writeOffRegister.Product.Title;

                index++;
            }

            sheet.Range[$"A1:D1"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A1:D1"].BorderAround(LineStyleType.Medium);
            sheet.Range[$"A4:L{index - 1}"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A4:L{index - 1}"].BorderAround(LineStyleType.Medium);
            sheet.AllocatedRange.AutoFitColumns();
            workBook.SaveToFile("testWriteOffRegister.xls");

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(Environment.CurrentDirectory + "/" + "testWriteOffRegister.xls")
            {
                UseShellExecute = true
            };
            p.Start();
        }

        public void ConvertWriteOffToXLSByProduct(WriteOffRegisterApi writeOffRegister)
        {
            var workBook = new Workbook();
            var sheet = workBook.Worksheets[0];
            GetWriteOffRegisters();
            sheet.Range["B1"].Value = "Наименование товара";
            sheet.Range["C1"].Value = "Название удаления";
            sheet.Range["D1"].Value = "Дата удаления";
            sheet.Range["E1"].Value = "Причина удаления";

            List<WriteOffRegisterApi> WriteOffRegisterByProduct = WriteOffRegisters.FindAll(s => s.Id == s.Id).Where
                (
                    s => s.Product == writeOffRegister.Product && s.ProductId == s.Product.Id
                ).ToList();

            int index = 5;
            int count = 1;

            foreach(var product in WriteOffRegisterByProduct)
            {
                DateTime date = (DateTime)product.DeletedAt;
                sheet.Range[$"A{index}"].NumberValue = count++;
                sheet.Range[$"B{index}"].Value = product.Product.Title;
                sheet.Range[$"C{index}"].Value = product.Title;
                sheet.Range[$"D{index}"].Value = date.ToShortDateString();
                sheet.Range[$"E{index}"].Value = product.ReasonDelete;

                index++;
            }
            sheet.Range[$"B1:E2"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"B1:E2"].BorderAround(LineStyleType.Medium);
            sheet.Range[$"A4:G{index - 1}"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A4:G{index - 1}"].BorderAround(LineStyleType.Medium);
            sheet.AllocatedRange.AutoFitColumns();
            workBook.SaveToFile("testWriteOffProduct.xls");
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(Environment.CurrentDirectory + "/testWriteOffProduct.xls")
            {
                UseShellExecute = true
            };
            p.Start();
        }
    }
}
