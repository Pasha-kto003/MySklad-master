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
    public class ShopViewModel : BaseViewModel
    {

        private ShopApi selectedShop { get; set; }
        public ShopApi SelectedShop
        {
            get => selectedShop;
            set
            {
                selectedShop = value;
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

        public List<ShopApi> searchResult;

        int paginationPageIndex = 0;
        private string searchCountRows;
        private string selectedViewCountRows;
        public int rows = 0;
        public int CountPages = 0;
        private int CountAll { get; set; }

        public CustomCommand BackPage { get; set; }
        public CustomCommand ForwardPage { get; set; }
        public CustomCommand AddShop { get; set; }
        public CustomCommand EditShop { get; set; }
        public CustomCommand UpdateList { get; set; }

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

        async Task ShopsApi()
        {
            var shop = await Api.GetListAsync<List<ShopApi>>("Shop");
            Shops = (List<ShopApi>)shop;
            CountAll = Shops.Count;
        }


        public ShopViewModel()
        {
            Shops = new List<ShopApi>();
            ShopsApi();

            ViewCountRows = new List<string>();
            ViewCountRows.AddRange(new string[] { "4", "все" });
            selectedViewCountRows = ViewCountRows.Last();

            SortType = new List<string>();
            SortType.AddRange(new string[] { "Наименование"});
            selectedSortType = SortType.First();

            OrderType = new List<string>();
            OrderType.AddRange(new string[] { "По возрастанию", "По убыванию", "По умолчанию" });
            selectedOrderType = OrderType.Last();

            SearchType = new List<string>();
            SearchType.AddRange(new string[] { "Наименование", "Email" });
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
                ShopsApi();
            });

            AddShop = new CustomCommand(() =>
            {
                AddShopView addShop = new AddShopView();
                addShop.ShowDialog();
                ShopsApi();
            });

            EditShop = new CustomCommand(() =>
            {
                if (SelectedShop == null) return;
                AddShopView addShop = new AddShopView(SelectedShop);
                addShop.ShowDialog();
                ShopsApi();
            });
            Search();
            ShopsApi();
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
                Shops = searchResult;
            }
            else
            {
                Shops = searchResult.Skip(rowsOnPage * paginationPageIndex).Take(rowsOnPage).ToList();
                SignalChanged("Shops");
                int.TryParse(SelectedViewCountRows, out rows);
                CountPages = searchResult.Count() / rows;
                Pages = $"{paginationPageIndex + 1} из {CountPages + 1}";
            }
        }

        private void Search()
        {
            var search = SearchText.ToLower();         
            if (SelectedSearchType == "Наименование")
                searchResult = Shops
                    .Where(c => c.Name.ToLower().Contains(search)).ToList();
            else if (SelectedSearchType == "Email")
                searchResult = Shops
                        .Where(c => c.Email.ToLower().Contains(search)).ToList();          
            Sort();
            InitPagination();
            Pagination();         
            SignalChanged("Shops");
        }

        internal void Sort()
        {
            if (SelectedOrderType == "По умолчанию")
                return;

            if (SelectedOrderType == "По убыванию")
            {
                if (SelectedSortType == "Наименование")
                    searchResult.Sort((x, y) => y.Name.CompareTo(x.Name));
            }

            if (SelectedOrderType == "По возрастанию")
            {
                if (SelectedSortType == "Наименование")
                    searchResult.Sort((x, y) => x.Name.CompareTo(y.Name));
            }
            paginationPageIndex = 0;
            Pagination();
            SignalChanged("Shops");
        }
    }
}
