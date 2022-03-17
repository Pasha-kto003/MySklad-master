using ModelApi;
using MySklad.Core;
using MySklad.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MySklad.ViewModel
{
    public class ProductViewModel : BaseViewModel
    {
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

        public List<StatusApi> StatusFilter { get; set; }
        public StatusApi SelectedStatusFilter
        {
            get => selectedStatusFilter;
            set
            {
                selectedStatusFilter = value;
                Search();
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

        private ProductApi selectedProduct { get; set; }
        public ProductApi SelectedProduct
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                SignalChanged();
            }
        }

        public CustomCommand CreateProduct { get; set; }
        public CustomCommand EditProduct { get; set; }
        public CustomCommand BackPage { get; set; }
        public CustomCommand ForwardPage { get; set; }

        public List<ProductApi> searchResult;

        int paginationPageIndex = 0;
        private string searchCountRows;
        private string selectedViewCountRows;
        public int rows = 0;
        public int CountPages = 0;
        private StatusApi selectedStatusFilter;

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

        public ProductViewModel()
        {          
            Units = new List<UnitApi>();
            ProductTypes = new List<ProductTypeApi>();
            Products = new List<ProductApi>();
            GetProducts();

            ViewCountRows = new List<string>();
            ViewCountRows.AddRange(new string[] { "5", "все" });
            selectedViewCountRows = ViewCountRows.Last();

            SortType = new List<string>();
            SortType.AddRange(new string[] { "Остаток", "Телефон" });
            selectedSortType = SortType.First();

            OrderType = new List<string>();
            OrderType.AddRange(new string[] { "По возрастанию", "По убыванию", "По умолчанию" });
            selectedOrderType = OrderType.Last();

            SearchType = new List<string>();
            SearchType.AddRange(new string[] { "Название", "Описание" });
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

            CreateProduct = new CustomCommand(() =>
            {
                AddProduct addProduct = new AddProduct();
                addProduct.ShowDialog();
                GetProducts();
            });

            EditProduct = new CustomCommand(() =>
            {
                if (SelectedProduct == null)
                {
                    MessageBox.Show("Выберите продукт из списка продукций");
                    return;
                }
                else
                {
                    AddProduct addProduct = new AddProduct(SelectedProduct);
                    addProduct.ShowDialog();
                    GetProducts();
                }
            });
            SignalChanged("Products");
            Search();
            //GetProducts();
            InitPagination();
            Pagination();
        }

        private void InitPagination()
        {
            SearchCountRows = $"Найдено записей: {searchResult.Count} из {Products.Count()}";
            paginationPageIndex = 0;
        }

        private void Pagination()
        {
            int rowsOnPage = 0;
            if (!int.TryParse(SelectedViewCountRows, out rowsOnPage))
            {
                Products = searchResult;
            }
            else
            {
                Products = searchResult.Skip(rowsOnPage * paginationPageIndex).Take(rowsOnPage).ToList();
                SignalChanged("Personals");
                int.TryParse(SelectedViewCountRows, out rows);
                CountPages = (searchResult.Count() - 1) / rows;
                Pages = $"{paginationPageIndex + 1} из {CountPages + 1}";
            }
        }

        private async Task GetProducts()
        {
            Units = await Api.GetListAsync<List<UnitApi>>("Unit");
            ProductTypes = await Api.GetListAsync<List<ProductTypeApi>>("ProductType");
            Products = await Api.GetListAsync<List<ProductApi>>("Product");
            foreach (ProductApi product in Products)
            {
                product.Unit = Units.First(s => s.Id == product.UnitId);
                product.ProductType = ProductTypes.First(s => s.Id == product.ProductTypeId);
            }
            SignalChanged("Products");
        }

        internal void Sort()
        {
            if (SelectedOrderType == "По умолчанию")
                return;

            if (SelectedOrderType == "По убыванию")
            {
                if (SelectedSortType == "Остаток")
                    searchResult.Sort((x, y) => ((Int32)y.CountInStock).CompareTo(x.CountInStock));
            }

            if (SelectedOrderType == "По возрастанию")
            {
                if (SelectedSortType == "Остаток")
                    searchResult.Sort((x, y) => ((Int32)x.CountInStock).CompareTo(y.CountInStock));
            }
            paginationPageIndex = 0;

            Pagination();
            SignalChanged("Products");
        }

        private void Search()
        {
            var search = SearchText.ToLower();
            if (SelectedSearchType == "Название")
                searchResult = Products
                    .Where(c => c.Title.ToLower().Contains(search)).ToList();
            else if (SelectedSearchType == "Описание")
                searchResult = Products
                        .Where(c => c.Description.ToLower().Contains(search)).ToList();
            Sort();
            InitPagination();
            Pagination();
            SignalChanged("Products");
        }
    }
}
