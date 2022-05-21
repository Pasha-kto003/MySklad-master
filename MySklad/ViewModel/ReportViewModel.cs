using ModelApi;
using MySklad.Core;
using Spire.Xls;
using Spire.Xls.Charts;
using Spire.Xls.Core;
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

        private string selectedTypeRack { get; set; }
        public string SelectedTypeRack
        {
            get => selectedTypeRack;
            set
            {
                selectedTypeRack = value;
                SignalChanged();
            }
        }

        public List<string> Types { get; set; }
        public List<string> TypesOut { get; set; }
        public List<string> TypesProduct { get; set; }
        public List<string> TypesWriteOff { get; set; }
        public List<string> TypesRack { get; set; }

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
        public DateTime SelectedDateStartRack { get; set; }
        public DateTime SelectedDateEndRack { get; set; }

        public int OrderInCount { get; set; }
        public int OrderOutCount { get; set; }
        public int ProductInOrderIn { get; set; }
        public int ProductInOrderOut { get; set; }
        public int RackCount { get; set; }
        public int ProductCount { get; set; }
        public int ProductDeleteCount { get; set; }
        public double PersonalCount { get; set; }
        public double PersonalSick { get; set; }
        public double PersonalSickCount { get; set; }
        public string PersonalSickProcent { get; set; }
        public double MidleCountValue { get; set; }
        public double MidleCountValueOut { get; set; }

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

        private List<PersonalApi> personals { get; set; }
        public List<PersonalApi> Personals
        {
            get => personals;
            set
            {
                personals = value;
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

        private List<CrossProductRackApi> crossProductRacks { get; set; }
        public List<CrossProductRackApi> CrossProductRacks
        {
            get => crossProductRacks;
            set
            {
                crossProductRacks = value;
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

        private CrossProductRackApi selectedRack { get; set; }
        public CrossProductRackApi SelectedRack
        {
            get => selectedRack;
            set
            {
                selectedRack = value;
                SignalChanged();
            }
        }

        public List<OrderInApi> Orders { get; set; }
        public List<OrderOutApi> OrderOuts { get; set; }

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

        private List<StatusApi> statuses { get; set; }
        public List<StatusApi> Statuses
        {
            get => statuses;
            set
            {
                statuses = value;
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

        private CrossProductOrderApi selectedOrderIn { get; set; }
        public CrossProductOrderApi SelectedOrderIn
        {
            get => selectedOrderIn;
            set
            {
                selectedOrderIn = value;
                SignalChanged();
            }
        }

        private CrossOrderOutApi selectedOrderOut { get; set; }
        public CrossOrderOutApi SelectedOrderOut
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

        private string maxValueCount;
        public string MaxValueCount
        {
            get => maxValueCount;
            set
            {
                maxValueCount = value;
                SignalChanged();
            }
        }

        public CustomCommand CountAll { get; set; }
        public CustomCommand EditReport { get; set; }
        public CustomCommand EditReportOut { get; set; }
        public CustomCommand EditReportProduct { get; set; }
        public CustomCommand EditReportWriteOff { get; set; }
        public CustomCommand EditReportRack { get; set; }
        public CustomCommand CountAllProduct { get; set; }

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
            Products = await Api.GetListAsync<List<ProductApi>>("Product");
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
            CrossProductOrders = await Api.GetListAsync<List<CrossProductOrderApi>>("CrossIn");
            OrdersIn = await Api.GetListAsync<List<OrderInApi>>("OrderIn");
            Suppliers = await Api.GetListAsync<List<SupplierApi>>("Supplier");
            Products = await Api.GetListAsync<List<ProductApi>>("Product");
            foreach(CrossProductOrderApi crossProduct in CrossProductOrders)
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
            Shops = await Api.GetListAsync<List<ShopApi>>("Shop");
            Companies = await Api.GetListAsync<List<CompanyApi>>("Company");
            foreach (CrossOrderOutApi crossProduct in CrossProductOrdersOut)
            {
                crossProduct.OrderOut = OrdersOut.First(s => s.Id == crossProduct.OrderOutId);
                crossProduct.OrderOut.Supplier = Suppliers.First(s => s.Id == crossProduct.OrderOut.SupplierId);
                crossProduct.Product = Products.First(s => s.Id == crossProduct.ProductId);
                crossProduct.OrderOut.Shop = Shops.First(s => s.Id == crossProduct.OrderOut.ShopId);
            }
        }

        async Task GetPersonals()
        {
            Personals = await Api.GetListAsync<List<PersonalApi>>("Personal");
            Statuses = await Api.GetListAsync<List<StatusApi>>("Status");
            foreach(PersonalApi personal in Personals)
            {
                personal.Status = Statuses.First(s => s.Id == personal.StatusId);
            }
        }

        async Task GetStatuses()
        {
            var statuses = await Api.GetListAsync<List<StatusApi>>("Status");
            Statuses = (List<StatusApi>)statuses;
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
            Racks = await Api.GetListAsync<List<RackApi>>("Rack");
            Products = await Api.GetListAsync<List<ProductApi>>("Product");
            Personals = await Api.GetListAsync<List<PersonalApi>>("Personal");
            foreach(RackApi rackApi in Racks)
            {
                rackApi.Personal = Personals.First(s => s.Id == rackApi.PersonalId);
            }
        }

        async Task GetCrossRack()
        {
            CrossProductRacks = await Api.GetListAsync<List<CrossProductRackApi>>("CrossRack");
            Racks = await Api.GetListAsync<List<RackApi>>("Rack");
            Products = await Api.GetListAsync<List<ProductApi>>("Product");
            Personals = await Api.GetListAsync<List<PersonalApi>>("Personal");
            foreach(CrossProductRackApi crossProduct in CrossProductRacks)
            {
                crossProduct.Rack = Racks.First(s => s.Id == crossProduct.RackId);
                crossProduct.Rack.Personal = Personals.First(s => s.Id == crossProduct.Rack.PersonalId);
                crossProduct.Rack.Personal.Status = Statuses.First(s => s.Id == crossProduct.Rack.Personal.StatusId);
                crossProduct.Product = Products.First(s => s.Id == crossProduct.ProductId);
            }
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
            GetCrossRack();
            GetCrossIn();
            GetCrossOut();
            GetCompany();
            GetWriteOffRegisters();
            GetPersonals();
            GetStatuses();

            Types = new List<string>
            {
                "Поставщик",
                "Период",
                "Заказ"
            };

            TypesOut = new List<string>
            {
                "Магазин",
                "Поставщик",
                "Период",
                "Заказ"
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

            TypesRack = new List<string>
            {
                "Период",
                "Стеллаж",
                "Сотрудник"
            };


            CountAll = new CustomCommand(() =>
            {
                OrderInCount = OrdersIn.Where
                (
                    s => s.DateOrderIn >= SelectedAfterDate && s.DateOrderIn <= SelectedBeforeDate
                ).Count();

                OrderOutCount = OrdersOut.FindAll(s => s.Id == s.Id).Where
                (
                    s => s.DateOrderOut >= SelectedAfterDate && s.DateOrderOut <= SelectedBeforeDate
                ).Count();

                foreach (CrossProductOrderApi cross in CrossProductOrders.Where(s => s.ProductId != 0 && s.OrderIn.DateOrderIn >= SelectedAfterDate && s.OrderIn.DateOrderIn <= SelectedBeforeDate))
                {
                    if(cross.CountInOrder != null)
                    {
                        ProductInOrderIn += (int)cross.CountInOrder / 2;
                    }

                    MidleCountValue = ProductInOrderIn / CrossProductOrders.Count;
                    SignalChanged(nameof(MidleCountValue));
                }
                foreach (CrossOrderOutApi cross in CrossProductOrdersOut.FindAll(s => s.ProductId != 0 && s.OrderOut.DateOrderOut >= SelectedAfterDate && s.OrderOut.DateOrderOut <= SelectedBeforeDate))
                {
                    ProductInOrderOut += (int)cross.CountOutOrder / 2;

                    MidleCountValueOut = ProductInOrderOut / CrossProductOrdersOut.Count;
                    SignalChanged(nameof(MidleCountValueOut));
                }

                ProductCount = ProductInOrderIn - ProductInOrderOut;

                Products.Sort((x, y) => x.CountInStock.CompareTo(y.CountInStock));
                MaxValueCount = Products.Last().Title.ToString();
                SignalChanged(MaxValueCount);

                PersonalCount = Personals.FindAll(s => s.Id == s.Id).Where
                (
                    s=> s.DateStartWork >= SelectedAfterDate && s.DateEndWork <= SelectedBeforeDate
                ).Count();


                PersonalSick = Personals.FindAll(s => s.Id == s.Id).Where
                (
                    s=> s.Status.Title == "Болеет"
                ).Count();

                PersonalSickCount = PersonalSick * 100 / PersonalCount;

                PersonalSickCount = Math.Round(PersonalSickCount, 2);

                PersonalSickProcent = $"{PersonalSickCount} %";

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
                SignalChanged(nameof(PersonalCount));
                SignalChanged(nameof(PersonalSick));
                SignalChanged(nameof(PersonalSickCount));
                SignalChanged(nameof(PersonalSickProcent));
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
                    case "Заказ":
                        ConvertReportOrderInXLSByOrderIn(SelectedOrderIn.OrderIn);
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
                    case "Заказ":
                        ConvertReportOrderOutXLSByOrderOut(SelectedOrderOut.OrderOut);
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

            EditReportRack = new CustomCommand(() =>
            {
                switch (SelectedTypeRack)
                {
                    case "Период":
                        ReportRackByPeriod(SelectedDateStartRack, SelectedDateEndRack);
                        break;
                    case "Стеллаж":
                        ConvertReportRackByRack(SelectedRack.Rack);
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
            sheet.Range["C4"].Value = "Статус накладной";
            sheet.Range["G4"].Value = "Товары";

            int index = 5;
            int count = 1;

            List<CrossProductOrderApi> OrderByPeriod = CrossProductOrders.Where
                (
                    s => s.OrderIn.DateOrderIn >= firstDate && s.OrderIn.DateOrderIn <= lastDate &&
                    s.OrderIn.SupplierId == s.OrderIn.Supplier.Id
                ).ToList();

            foreach (var order in OrderByPeriod)
            {
                DateTime date = (DateTime)order.OrderIn.DateOrderIn;
                sheet.Range[$"A{index}"].NumberValue = count++;
                sheet.Range[$"B{index}"].Value = date.ToShortDateString();
                sheet.Range[$"C{index}"].Value = order.OrderIn.Status;
                sheet.Range[$"F{index}"].Value = order.OrderIn.Supplier.FirstName;
                sheet.Range[$"G{index}"].Value = order.Product.Title;

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

        public void ConvertReportToXLSBySupplier(CrossProductOrderApi orderIn)
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

            List<CrossProductOrderApi> OrderBySupplier = CrossProductOrders.Where
                (
                    s => s.OrderIn.SupplierId == SelectedOrderIn.OrderIn.Supplier.Id
                ).ToList();
            int index = 5;
            int count = 1;

            foreach (var supp in OrderBySupplier)
            {
                sheet.Range[$"A{index}"].NumberValue = count++;
                sheet.Range[$"B{index}"].Value = supp.OrderIn.Supplier.FirstName;
                sheet.Range[$"C{index}"].Value = supp.OrderIn.Id.ToString();
                sheet.Range[$"D{index}"].Value = supp.OrderIn.Supplier.Email;
                sheet.Range[$"E{index}"].Value = supp.OrderIn.Supplier.Phone;
                sheet.Range[$"F{index}"].Value = supp.OrderIn.Supplier.CompanyId.ToString();
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

        public void ConvertReportOrderInXLSByOrderIn(OrderInApi orderIn)
        {
            GetCrossIn();
            GetOrderIn();
            var workBook = new Workbook();
            var sheet = workBook.Worksheets[0];

            sheet.Range["B1"].Value = "Дата заказа";
            sheet.Range["C1"].Value = "Продукт";
            sheet.Range["D1"].Value = "Количество привезенной продукции";

            int index = 5;
            int count = 1;

            sheet.Range[$"B{index}"].Value = orderIn.DateOrderIn.ToShortDateString();
            foreach (var product in orderIn.Products)
            {
                sheet.Range[$"C{index}"].Value = product.Title;
                sheet.Range[$"D{index}"].Value2 = SelectedOrderIn.CountInOrder;
                index++;
            }

            Chart chart = sheet.Charts.Add(ExcelChartType.PieExploded);
            chart.SeriesDataFromRange = false;

            chart.LeftColumn = 1;
            chart.TopRow = 12;
            chart.RightColumn = 8;
            chart.BottomRow = 26;

            chart.Width = 400;
            chart.Height = 400;

            chart.ChartArea.Border.Weight = ChartLineWeightType.Medium;
            chart.ChartArea.Border.Color = System.Drawing.Color.SandyBrown;

            chart.ChartTitle = "Racks";
            chart.ChartTitleArea.FontName = "Calibri";
            chart.ChartTitleArea.Size = 10;
            chart.ChartTitleArea.IsBold = true;

            chart.DataRange = sheet.Range[$"D5:D{orderIn.Products.Count + 4}"];
            chart.HasLegend = true;
            chart.Legend.Position = LegendPositionType.Right;
            chart.Legend.HasDataTable = true;

            ChartSerie cs = chart.Series[0];
            cs.CategoryLabels = sheet.Range[$"C5:C{orderIn.Products.Count + 4}"];
            cs.Values = sheet.Range[$"D5:D{orderIn.Products.Count + 4}"];

            IChartSerie serie1 = chart.Series[0];

            serie1.SerieType = ExcelChartType.PieExploded;
            serie1.DataPoints.DefaultDataPoint.DataLabels.HasValue = true;

            sheet.Range[$"A1:D1"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A1:D1"].BorderAround(LineStyleType.Medium);
            sheet.Range[$"A4:L{index - 1}"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A4:L{index - 1}"].BorderAround(LineStyleType.Medium);
            sheet.AllocatedRange.AutoFitColumns();
            workBook.SaveToFile("testOrderInByOrderIn.xls");
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(Environment.CurrentDirectory + "/testOrderInByOrderIn.xls")
            {
                UseShellExecute = true
            };
            p.Start();
        }

        public void ReportRackByPeriod(DateTime firstDate, DateTime lastDate)
        {
            GetCrossRack();
            GetRacks();
            var workBook = new Workbook();
            var sheet = workBook.Worksheets[0];
            sheet.Range["A1"].Value = $"С ";
            sheet.Range["B1"].Value = $" { firstDate.Date.ToShortDateString()}";
            sheet.Range["D1"].Value = $"{lastDate.Date.ToShortDateString()}";
            sheet.Range["C1"].Value = $"По ";

            sheet.Range["B4"].Value = "Номер стеллажа";
            sheet.Range["C4"].Value = "Дата добавления продукта";
            sheet.Range["D4"].Value = "Товары";
            sheet.Range["E4"].Value = "Количество товаров";
            sheet.Range["F4"].Value = "Сотрудник данного стеллажа";
            sheet.Range["G4"].Value = "Дата изменения стеллажа";
            sheet.Range["H4"].Value = "Дата удаления стеллажа";

            int index = 5;
            int count = 1;

            List<CrossProductRackApi> RackByPeriod = CrossProductRacks.FindAll(s => s.RackId == s.RackId).Where
                (
                    s => s.DateProductPlacement >= firstDate && s.DateProductPlacement <= lastDate && s.Rack.PersonalId == s.Rack.Personal.Id
                ).ToList();

            foreach(var rack in RackByPeriod)
            {
                DateTime date = (DateTime)rack.DateProductPlacement;
                sheet.Range[$"A{index}"].NumberValue = count++;
                sheet.Range[$"B{index}"].Value = rack.Rack.Name;
                sheet.Range[$"C{index}"].Value = date.ToShortDateString();
                sheet.Range[$"D{index}"].Value = rack.Product.Title;
                sheet.Range[$"E{index}"].Value = rack.Product.CountInStock.ToString();
                sheet.Range[$"F{index}"].Value = rack.Rack.Personal.FirstName;
                sheet.Range[$"G{index}"].Value = rack.Rack.ChangedDate.ToShortDateString();
                sheet.Range[$"H{index}"].Value = rack.Rack.DeletionDate.ToString();

                index++;
            }
            sheet.Range[$"B1:E2"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"B1:E2"].BorderAround(LineStyleType.Medium);
            sheet.Range[$"A4:G{index - 1}"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A4:G{index - 1}"].BorderAround(LineStyleType.Medium);
            sheet.AllocatedRange.AutoFitColumns();
            workBook.SaveToFile("testRackPeriod.xls");
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(Environment.CurrentDirectory + "/testRackPeriod.xls")
            {
                UseShellExecute = true
            };
            p.Start();
        }

        public void ConvertReportRackByPersonal(PersonalApi personal)
        {
            GetCrossRack();
            GetRacks();
            var workBook = new Workbook();
            var sheet = workBook.Worksheets[0];

            sheet.Range["B1"].Value = "Фамилия сотрудника";
            sheet.Range["C1"].Value = "Имя сотрудника";
            sheet.Range["D1"].Value = "Отчество сотрудника";
            sheet.Range["E1"].Value = "Статус сотрудника";
            sheet.Range["F1"].Value = "Стеллаж";

            int index = 5;
            int count = 1;

            List<CrossProductRackApi> RackByPeriod = CrossProductRacks.FindAll(s => s.RackId == s.RackId).Where
                (
                    s => s.Rack.PersonalId == SelectedRack.Rack.PersonalId
                ).ToList();

            foreach(var rack in RackByPeriod)
            {
                DateTime date = (DateTime)rack.DateProductPlacement;
                sheet.Range[$"A{index}"].NumberValue = count++;
                sheet.Range[$"B{index}"].Value = rack.Rack.Personal.LastName;
                sheet.Range[$"C{index}"].Value = rack.Rack.Personal.FirstName;
                sheet.Range[$"D{index}"].Value = rack.Rack.Personal.Patronimyc;
                sheet.Range[$"E{index}"].Value = rack.Rack.Personal.Status.Title;
                sheet.Range[$"F{index}"].Value = rack.Rack.Name;

                index++;
            }

            sheet.Range[$"B1:F2"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"B1:F2"].BorderAround(LineStyleType.Medium);
            sheet.Range[$"A4:G{index - 1}"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A4:G{index - 1}"].BorderAround(LineStyleType.Medium);
            sheet.AllocatedRange.AutoFitColumns();
            workBook.SaveToFile("testsupp.xls");
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(Environment.CurrentDirectory + "/testRackSupp.xls")
            {
                UseShellExecute = true
            };
            p.Start();
        }

        public void ConvertReportRackByRack(RackApi rack)
        {
            GetRacks();
            GetCrossRack();
            var workBook = new Workbook();
            var sheet = workBook.Worksheets[0];

            sheet.Range["B1"].Value = "Стеллаж";
            sheet.Range["C1"].Value = "Продукт";
            sheet.Range["D1"].Value = "Количество";

            int index = 5;
            int count = 1;

            sheet.Range[$"B{index}"].Value = rack.Name;
            foreach(var product in rack.Products)
            {
                sheet.Range[$"C{index}"].Value = product.Title;
                sheet.Range[$"D{index}"].Value = product.CountInStock.ToString();

                index++;
            }

            Chart chart = sheet.Charts.Add(ExcelChartType.PieExploded);
            chart.SeriesDataFromRange = false;

            chart.LeftColumn = 1;
            chart.TopRow = 12;
            chart.RightColumn = 8;
            chart.BottomRow = 26;

            chart.Width = 400;
            chart.Height = 400;

            chart.ChartArea.Border.Weight = ChartLineWeightType.Medium;
            chart.ChartArea.Border.Color = System.Drawing.Color.SandyBrown;

            chart.ChartTitle = "Racks";
            chart.ChartTitleArea.FontName = "Calibri";
            chart.ChartTitleArea.Size = 10;
            chart.ChartTitleArea.IsBold = true;

            chart.DataRange = sheet.Range[$"D5:D{rack.Products.Count + 4}"];
            chart.HasLegend = true;
            chart.Legend.Position = LegendPositionType.Right;
            chart.Legend.HasDataTable = true;

            ChartSerie cs = chart.Series[0];
            cs.CategoryLabels = sheet.Range[$"C5:C{rack.Products.Count + 4}"];
            cs.Values = sheet.Range[$"D5:D{rack.Products.Count + 4}"];

            IChartSerie serie1 = chart.Series[0];

            serie1.SerieType = ExcelChartType.PieExploded;
            serie1.DataPoints.DefaultDataPoint.DataLabels.HasValue = true;

            sheet.Range[$"A1:D1"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A1:D1"].BorderAround(LineStyleType.Medium);
            sheet.Range[$"A4:L{index - 1}"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A4:L{index - 1}"].BorderAround(LineStyleType.Medium);
            sheet.AllocatedRange.AutoFitColumns();

            workBook.SaveToFile("testRackByRack.xls");

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(Environment.CurrentDirectory + "/" + "testRackByRack.xls")
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

            List<CrossOrderOutApi> OrderOutByPeriod = CrossProductOrdersOut.FindAll(s => s.OrderOutId == s.OrderOutId).Where
                (
                    s => s.OrderOut.DateOrderOut >= firstDate && s.OrderOut.DateOrderOut <= lastDate &&
                    s.OrderOut.SupplierId == s.OrderOut.Supplier.Id && s.OrderOut.ShopId == s.OrderOut.Shop.Id
                ).ToList();

            foreach (var order in OrderOutByPeriod)
            {
                DateTime date = (DateTime)order.OrderOut.DateOrderOut;
                sheet.Range[$"A{index}"].NumberValue = count++;
                sheet.Range[$"B{index}"].Value = date.ToShortDateString();
                sheet.Range[$"C{index}"].Value = order.OrderOut.Status;
                sheet.Range[$"D{index}"].Value = order.OrderOut.Supplier.FirstName;
                sheet.Range[$"E{index}"].Value = order.OrderOut.Shop.Name;
                sheet.Range[$"F5{index}"].Value = order.Product.Title;

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

        public void ConvertReportOutToXLSBySupplier(CrossOrderOutApi orderOut)
        {
            var workBook = new Workbook();
            var sheet = workBook.Worksheets[0];
            GetProducts();
            GetOrderOut();
            sheet.Range["B1"].Value = "Имя поставщика";
            sheet.Range["C1"].Value = "Фамилия поставщика";
            sheet.Range["D1"].Value = "Отчество поставщика";
            sheet.Range["E1"].Value = "Заказ";
            sheet.Range["F1"].Value = "Почта";
            sheet.Range["G1"].Value = "Телефон";
            sheet.Range["H1"].Value = "Рейтинг";
            sheet.Range["I4"].Value = "Наименование компании";
            sheet.Range["J4"].Value = "Увезенный продукт";

            List<CrossOrderOutApi> OrderOutByPeriod = CrossProductOrdersOut.FindAll(s => s.OrderOutId == s.OrderOutId).Where
                (
                    s => s.OrderOut.SupplierId == SelectedOrderOut.OrderOut.Supplier.Id
                ).ToList();
            int index = 5;
            int count = 1;

            foreach (var supp in OrderOutByPeriod)
            {
                sheet.Range[$"A{index}"].NumberValue = count++;
                sheet.Range[$"B{index}"].Value = supp.OrderOut.Supplier.FirstName;
                sheet.Range[$"C{index}"].Value = supp.OrderOut.Supplier.LastName;
                sheet.Range[$"D{index}"].Value = supp.OrderOut.Supplier.Patronimyc;
                sheet.Range[$"E{index}"].Value = supp.OrderOut.Id.ToString();
                sheet.Range[$"F{index}"].Value = supp.OrderOut.Supplier.Email;
                sheet.Range[$"G{index}"].Value = supp.OrderOut.Supplier.Phone;
                sheet.Range[$"H{index}"].Value = supp.OrderOut.Supplier.Rating.ToString();
                sheet.Range[$"I{index}"].Value = supp.OrderOut.Supplier.Company.NameOfCompany;
                sheet.Range[$"J{index}"].Value = supp.Product.Title;
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

        public void ConvertReportOutToXLSByShop(CrossOrderOutApi orderOutApi)
        {
            var workBook = new Workbook();
            var sheet = workBook.Worksheets[0];
            GetProducts();
            GetOrderOut();
            sheet.Range["B1"].Value = "Наименование магазина";
            sheet.Range["C1"].Value = "Заказ";
            sheet.Range["D1"].Value = "Почта";
            sheet.Range["E1"].Value = "Телефон";

            List<CrossOrderOutApi> OrderOutByPeriod = CrossProductOrdersOut.FindAll(s => s.OrderOutId == s.OrderOutId).Where
                (
                     s => s.OrderOut.ShopId == SelectedOrderOut.OrderOut.Shop.Id
                ).ToList();
            int index = 5;
            int count = 1;

            foreach (var supp in OrderOutByPeriod)
            {
                sheet.Range[$"A{index}"].NumberValue = count++;
                sheet.Range[$"B{index}"].Value = supp.OrderOut.Shop.Name;
                sheet.Range[$"C{index}"].Value = supp.OrderOut.Id.ToString();
                sheet.Range[$"D{index}"].Value = supp.OrderOut.Shop.Email;
                sheet.Range[$"E{index}"].Value = supp.OrderOut.Shop.Phone;
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
            GetProducts();
            GetOrderOut();

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
                    s => s.BestBeforeDateStart >= firstDate && s.BestBeforeDateStart <= lastDate
                ).ToList();

            foreach (var product in ProductByPeriod)
            {
                DateTime date = (DateTime)product.BestBeforeDateStart;
                DateTime dateEnd = (DateTime)product.BestBeforeDateEnd;
                sheet.Range[$"A{index}"].NumberValue = count++;
                sheet.Range[$"B{index}"].Value = product.Title;
                sheet.Range[$"C{index}"].Value = product.Description;
                sheet.Range[$"D{index}"].Value = date.ToShortDateString();
                sheet.Range[$"E{index}"].Value = dateEnd.ToShortDateString();
                sheet.Range[$"F{index}"].Value = product.CountInStock.ToString();
                sheet.Range[$"G{index}"].Value = product.ProductTypeId.ToString();
                sheet.Range[$"H{index}"].Value = product.UnitId.ToString();
                sheet.Range[$"I{index}"].Value = product.Status;

                index++;
            }

            sheet.Range[$"A1:D1"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A1:D1"].BorderAround(LineStyleType.Medium);
            sheet.Range[$"A4:L{index - 1}"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A4:L{index - 1}"].BorderAround(LineStyleType.Medium);
            sheet.AllocatedRange.AutoFitColumns();

            Chart chart = sheet.Charts.Add(ExcelChartType.PieExploded);
            chart.SeriesDataFromRange = false;

            chart.LeftColumn = 1;
            chart.TopRow = 12;
            chart.RightColumn = 8;
            chart.BottomRow = 26;

            chart.Width = 400;
            chart.Height = 400;

            chart.ChartArea.Border.Weight = ChartLineWeightType.Medium;
            chart.ChartArea.Border.Color = System.Drawing.Color.SandyBrown;

            chart.ChartTitle = "Products";
            chart.ChartTitleArea.FontName = "Calibri";
            chart.ChartTitleArea.Size = 10;
            chart.ChartTitleArea.IsBold = true;
            chart.DataRange = sheet.Range[$"F5:F{Products.Count}"];
            chart.HasLegend = true;
            chart.Legend.Position = LegendPositionType.Right;
            chart.Legend.HasDataTable = true;

            ChartSerie cs = chart.Series[0];
            cs.CategoryLabels = sheet.Range[$"B5:B{Products.Count}"];
            cs.Values = sheet.Range[$"F5:F{Products.Count}"];

            IChartSerie serie1 = chart.Series[0];

            serie1.SerieType = ExcelChartType.PieExploded;
            serie1.DataPoints.DefaultDataPoint.DataLabels.HasValue = true;

            workBook.SaveToFile("testProductOut1.xls");

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(Environment.CurrentDirectory + "/" + "testProductOut1.xls")
            {
                UseShellExecute = true
            };
            p.Start();
        }

        public void ConvertReportOrderOutXLSByOrderOut(OrderOutApi orderOut)
        {
            GetCrossOut();
            GetOrderOut();
            var workBook = new Workbook();
            var sheet = workBook.Worksheets[0];

            sheet.Range["B1"].Value = "Дата заказа";
            sheet.Range["C1"].Value = "Продукт";
            sheet.Range["D1"].Value = "Количество привезенной продукции";

            int index = 5;
            int count = 1;

            sheet.Range[$"B{index}"].Value = orderOut.DateOrderOut.ToShortDateString();
            foreach (var product in orderOut.Products)
            {
                sheet.Range[$"C{index}"].Value = product.Title;
                sheet.Range[$"D{index}"].Value = SelectedOrderOut.CountOutOrder.ToString();
                index++;
            }

            Chart chart = sheet.Charts.Add(ExcelChartType.PieExploded);
            chart.SeriesDataFromRange = false;

            chart.LeftColumn = 1;
            chart.TopRow = 12;
            chart.RightColumn = 8;
            chart.BottomRow = 26;

            chart.Width = 400;
            chart.Height = 400;

            chart.ChartArea.Border.Weight = ChartLineWeightType.Medium;
            chart.ChartArea.Border.Color = System.Drawing.Color.SandyBrown;

            chart.ChartTitle = "Racks";
            chart.ChartTitleArea.FontName = "Calibri";
            chart.ChartTitleArea.Size = 10;
            chart.ChartTitleArea.IsBold = true;

            chart.DataRange = sheet.Range[$"D5:D{orderOut.Products.Count + 4}"];
            chart.HasLegend = true;
            chart.Legend.Position = LegendPositionType.Right;
            chart.Legend.HasDataTable = true;

            ChartSerie cs = chart.Series[0];
            cs.CategoryLabels = sheet.Range[$"C5:C{orderOut.Products.Count + 4}"];
            cs.Values = sheet.Range[$"D5:D{orderOut.Products.Count + 4}"];

            IChartSerie serie1 = chart.Series[0];

            serie1.SerieType = ExcelChartType.PieExploded;
            serie1.DataPoints.DefaultDataPoint.DataLabels.HasValue = true;

            sheet.Range[$"A1:D1"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A1:D1"].BorderAround(LineStyleType.Medium);
            sheet.Range[$"A4:L{index - 1}"].BorderInside(LineStyleType.Thin);
            sheet.Range[$"A4:L{index - 1}"].BorderAround(LineStyleType.Medium);
            sheet.AllocatedRange.AutoFitColumns();
            workBook.SaveToFile("testOrderOutByOrderOut.xls");
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(Environment.CurrentDirectory + "/testOrderOutByOrderOut.xls")
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
