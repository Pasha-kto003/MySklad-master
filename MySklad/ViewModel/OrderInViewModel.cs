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
    public class OrderInViewModel : BaseViewModel
    {

        public List<OrderInApi> searchResult { get; set; }
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

        public List<CrossProductOrderApi> CrossProductOrder { get; set; }

        public CustomCommand EditOrderIn { get; set; }
        public CustomCommand CreateOrderIn { get; set; }
        public CustomCommand BackPage { get; set; }
        public CustomCommand ForwardPage { get; set; }

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

        

        public OrderInViewModel()
        {
            Products = new List<ProductApi>();
            Suppliers = new List<SupplierApi>();
            Orders = new List<OrderInApi>();
            GetOrders();

            ViewCountRows = new List<string>();
            ViewCountRows.AddRange(new string[] { "5", "все" });
            selectedViewCountRows = ViewCountRows.Last();

            SortType = new List<string>();
            SortType.AddRange(new string[] { "Дата"});
            selectedSortType = SortType.First();

            OrderType = new List<string>();
            OrderType.AddRange(new string[] { "По возрастанию", "По убыванию", "По умолчанию" });
            selectedOrderType = OrderType.Last();

            SearchType = new List<string>();
            SearchType.AddRange(new string[] { "Дата", "Поставщик" });
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

            CreateOrderIn = new CustomCommand(() =>
            {
                AddOrderIn addOrderIn = new AddOrderIn();
                addOrderIn.ShowDialog();
                GetOrders();
            });
            EditOrderIn = new CustomCommand(() =>
            {
                if (SelectedOrderIn == null) return;
                AddOrderIn addOrderIn = new AddOrderIn(SelectedOrderIn);
                addOrderIn.ShowDialog();
                GetOrders();
            });
            SignalChanged("Orders");
            Search();
            InitPagination();
            Pagination();
        }

        private void InitPagination()
        {
            SearchCountRows = $"Найдено записей: {searchResult.Count} из {Orders.Count}";
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

        private async Task GetOrders()
        {
            Products = await Api.GetListAsync<List<ProductApi>>("Product");
            Suppliers = await Api.GetListAsync<List<SupplierApi>>("Supplier");
            Orders = await Api.GetListAsync<List<OrderInApi>>("OrderIn");
            CrossProductOrder = await Api.GetListAsync<List<CrossProductOrderApi>>("CrossIn");
            foreach (OrderInApi orderIn in Orders)
            {
                orderIn.Supplier = Suppliers.First(s => s.Id == orderIn.SupplierId);
                orderIn.CrossProductOrders = CrossProductOrder.First(s => s.OrderInId == orderIn.Id);
            }
            SignalChanged("Orders");
            SignalChanged("Suppliers");

            InitPagination();
        }

        internal void Sort()
        {
            if (SelectedOrderType == "По умолчанию")
                return;

            if (SelectedOrderType == "По убыванию")
            {
                if (SelectedSortType == "Дата")
                    searchResult.Sort((x, y) => y.DateOrderIn.ToString().CompareTo(x.DateOrderIn));
            }

            if (SelectedOrderType == "По возрастанию")
            {
                if (SelectedSortType == "Дата")
                    searchResult.Sort((x, y) => x.DateOrderIn.ToString().CompareTo(y.DateOrderIn));
            }
            paginationPageIndex = 0;

            Pagination();
            SignalChanged("Orders");
        }

        private void Search()
        {
            foreach (OrderInApi orderIn in Orders)
            {
                orderIn.Supplier = Suppliers.First(s => s.Id == orderIn.SupplierId);
            }
            var search = SearchText.ToLower();
            if (SelectedSearchType == "Поставщик")
                searchResult = Orders.Where(s => s.Supplier.Title.ToString().ToLower().Contains(search)).ToList();
            else if (SelectedSearchType == "Дата")
                searchResult = Orders
                    .Where(c => c.DateOrderIn.ToShortDateString().ToLower().Contains(search)).ToList(); 
            Sort();
            InitPagination();
            Pagination();
            SignalChanged("Orders");
        }
    }
}
