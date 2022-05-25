using ModelApi;
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
        public List<OrderOutApi> searchResult { get; set; }
        public string SearchCountRows
        {
            get => searchCountRows;
            set
            {
                searchCountRows = value;
                SignalChanged();
            }
        }

        private string searchText = "";
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                Search();
            }
        }

        public List<string> ViewCountRows { get; set; }
        public string SelectedViewCountRows
        {
            get => selectedViewCountRows;
            set
            {
                selectedViewCountRows = value;
                paginationPageIndex = 0;
                Pagination();
            }
        }

        public List<string> SearchType { get; set; }
        private string selectedSearchType;
        public string SelectedSearchType
        {
            get => selectedSearchType;
            set
            {
                selectedSearchType = value;
                Search();
            }
        }

        private string selectedOrderType;
        public List<string> OrderType { get; set; }
        public string SelectedOrderType
        {
            get => selectedOrderType;
            set
            {
                selectedOrderType = value;
                Sort();
            }
        }

        public List<string> SortType { get; set; }
        private string selectedSortType;
        public string SelectedSortType
        {
            get => selectedSortType;

            set
            {
                selectedSortType = value;
                Sort();
            }
        }
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

        private int CountAll { get; set; }

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
            CountAll = Orders.Count;
            SignalChanged("Orders");
            InitPagination();
        }

        public CustomCommand CreateOrderOut { get; set; }
        public CustomCommand EditOrderOut { get; set; }
        public CustomCommand BackPage { get; set; }
        public CustomCommand ForwardPage { get; set; }
        public CustomCommand UpdateList { get; set; }

        int paginationPageIndex = 0;
        private string searchCountRows;
        private string selectedViewCountRows;
        public int rows = 0;
        public int CountPages = 0;

        private string pages;
        public string Pages
        {
            get => pages;
            set
            {
                pages = value;
                SignalChanged();
            }
        }

        public OrderOutViewModel()
        {
            Products = new List<ProductApi>();
            Suppliers = new List<SupplierApi>();
            Orders = new List<OrderOutApi>();
            Shops = new List<ShopApi>();
            GetOrders();

            ViewCountRows = new List<string>();
            ViewCountRows.AddRange(new string[] { "5", "все" });
            selectedViewCountRows = ViewCountRows.Last();

            SortType = new List<string>();
            SortType.AddRange(new string[] { "Дата" });
            selectedSortType = SortType.First();

            OrderType = new List<string>();
            OrderType.AddRange(new string[] { "По возрастанию", "По убыванию", "По умолчанию" });
            selectedOrderType = OrderType.Last();

            SearchType = new List<string>();
            SearchType.AddRange(new string[] { "Дата", "Поставщик", "Магазин", "Номер накладной" });
            selectedSearchType = SearchType.First();

            BackPage = new CustomCommand(() => {
                if (searchResult == null)
                    return;
                if (paginationPageIndex > 0)
                    paginationPageIndex--;
                Pagination();

            });

            ForwardPage = new CustomCommand(() =>
            {
                if (searchResult == null)
                    return;
                int.TryParse(SelectedViewCountRows, out int rowsOnPage);
                if (rowsOnPage == 0)
                    return;
                int countPage = searchResult.Count / rowsOnPage;
                CountPages = countPage;
                if (searchResult.Count() % rowsOnPage != 0)
                    countPage++;
                if (countPage > paginationPageIndex + 1)
                    paginationPageIndex++;
                Pagination();
            });

            UpdateList = new CustomCommand(() =>
            {
                GetOrders();
            });

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
            SignalChanged("Orders");
            Search();
            InitPagination();
            Pagination();
        }

        private void InitPagination()
        {
            SearchCountRows = $"Найдено записей: {searchResult.Count} из {CountAll}";
            paginationPageIndex = 0;
        }

        private void Pagination()
        {
            int rowsOnPage = 0;
            if (!int.TryParse(SelectedViewCountRows, out rowsOnPage))
            {
                Orders = searchResult;
            }
            else
            {
                Orders = searchResult.Skip(rowsOnPage * paginationPageIndex).Take(rowsOnPage).ToList();
                SignalChanged("Orders");
                int.TryParse(SelectedViewCountRows, out rows);
                CountPages = (searchResult.Count() - 1) / rows;
                Pages = $"{paginationPageIndex + 1} из {CountPages + 1}";
            }
        }

        internal void Sort()
        {
            if (SelectedOrderType == "По умолчанию")
                return;

            if (SelectedOrderType == "По убыванию")
            {
                if (SelectedSortType == "Дата")
                    searchResult.Sort((x, y) => y.DateOrderOut.ToString().CompareTo(x.DateOrderOut));
            }

            if (SelectedOrderType == "По возрастанию")
            {
                
                if (SelectedSortType == "Дата")
                    searchResult.Sort((x, y) => x.DateOrderOut.ToString().CompareTo(y.DateOrderOut));
            }
            paginationPageIndex = 0;

            Pagination();
            SignalChanged("Orders");
        }

        private void Search()
        {
            var search = SearchText.ToLower();
            if (SelectedSearchType == "Дата")
                searchResult = Orders
                    .Where(c => c.DateOrderOut.ToShortDateString().ToLower().Contains(search)).ToList();
            else if (SelectedSearchType == "Поставщик")
                searchResult = Orders
                        .Where(c => c.Supplier.FirstName.ToLower().Contains(search)).ToList();
            else if (SelectedSearchType == "Магазин")
                searchResult = Orders.Where(c => c.Shop.Name.ToLower().Contains(search)).ToList();
            else if (SelectedSearchType == "Номер накладной")
                searchResult = Orders.Where(c => c.Id.ToString().ToLower().Contains(search)).ToList();
            Sort();
            InitPagination();
            Pagination();
            SignalChanged("Orders");
        }
    }
}
