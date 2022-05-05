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
    public class SupplierViewModel : BaseViewModel
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

        private IEnumerable<OrderInApi> orders { get; set; }
        public IEnumerable<OrderInApi> Orders
        {
            get => orders;
            set
            {
                orders = value;
                SignalChanged();
            }
        }

        private IEnumerable<OrderOutApi> ordersOut { get; set; }
        public IEnumerable<OrderOutApi> OrdersOut
        {
            get => ordersOut;
            set
            {
                ordersOut = value;
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

        private SupplierApi selectedSupplier { get; set; }
        public SupplierApi SelectedSupplier
        {
            get => selectedSupplier;
            set
            {
                selectedSupplier = value;
                SignalChanged();
            }
        }

        public List<SupplierApi> searchResult;

        int paginationPageIndex = 0;
        private string searchCountRows;
        private string selectedViewCountRows;
        public int rows = 0;
        public int CountPages = 0;
        private StatusApi selectedStatusFilter;

        public CustomCommand BackPage { get; set; }
        public CustomCommand ForwardPage { get; set; }
        public CustomCommand AddSupplier { get; set; }
        public CustomCommand EditSupplier { get; set; }
        public CustomCommand RemoveSupplier { get; set; }

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

        public SupplierViewModel()
        {
            Suppliers = new List<SupplierApi>();
            Companies = new List<CompanyApi>();
            GetSuppliers();
            GetOrders();

            ViewCountRows = new List<string>();
            ViewCountRows.AddRange(new string[] { "5", "все" });
            selectedViewCountRows = ViewCountRows.Last();

            SortType = new List<string>();
            SortType.AddRange(new string[] { "Рейтинг" });
            selectedSortType = SortType.First();


            SearchType = new List<string>();
            SearchType.AddRange(new string[] { "Название поставщика", "Рейтинг"});
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

            RemoveSupplier = new CustomCommand(() =>
            {
                foreach (OrderInApi orderIn in Orders)
                {
                    var id = SelectedSupplier.Id;
                    var search = Orders.Where(s=> s.SupplierId == id);
                    if(search == null)
                    {
                        MessageBox.Show("Можно удалить данного поставщика");
                        return;
                    }
                    if (search != null)
                    {
                        MessageBox.Show("Невозможно удалить данного поставщика");
                        return;
                    }
                    //if (orderIn.SupplierId == SelectedSupplier.Id)
                    //{
                    //    MessageBox.Show("Невозможно удалить данного поставщика");
                    //    return;
                    //}

                    //else if (orderIn.SupplierId != SelectedSupplier.Id)
                    //{
                    //    MessageBox.Show("Можно удалить данного поставщика");
                    //    DeleteSupplier(SelectedSupplier);
                    //}
                } 
            });

            AddSupplier = new CustomCommand(() =>
            {
                AddSupplier addSupplier = new AddSupplier();
                addSupplier.ShowDialog();
                GetSuppliers();
                SignalChanged("Suppliers");
            });

            EditSupplier = new CustomCommand(() =>
            {
                if (SelectedSupplier == null) return;
                AddSupplier addSupplier = new AddSupplier(SelectedSupplier);
                addSupplier.ShowDialog();
                GetSuppliers();
                SignalChanged("Suppliers");
            });
            SignalChanged("Suppliers");

            Search();
            GetSuppliers();
            InitPagination();
            Pagination();
        }

        private async Task GetSuppliers()
        {
            Companies = await Api.GetListAsync<List<CompanyApi>>("Company");
            Suppliers = await Api.GetListAsync<List<SupplierApi>>("Supplier");
            foreach(SupplierApi supplier in Suppliers)
            {
                supplier.Company = Companies.First(s => s.Id == supplier.CompanyId);
            }
            SignalChanged("Suppliers");
        }

        private async Task GetOrders()
        {
            var order = await Api.GetListAsync<List<OrderInApi>>("OrderIn");
            var orderOut = await Api.GetListAsync<List<OrderOutApi>>("OrderOut");
            Orders = order.Where(s => s.SupplierId != 0);
            OrdersOut = orderOut.Where(s => s.SupplierId != 0);
        }

        private async Task DeleteSupplier(SupplierApi supplierApi)
        {
            var delete = await Api.DeleteAsync<SupplierApi>(supplierApi, "Supplier");
        }

        internal void Sort()
        {
            if (SelectedOrderType == "По умолчанию")
                return;
            if (SelectedOrderType == "По убыванию")
            {
                if (SelectedSortType == "Рейтинг")
                    searchResult.Sort((x, y) => ((Int32)y.Rating).CompareTo(x.Rating));
            }

            if (SelectedOrderType == "По возрастанию")
            {
                if (SelectedSortType == "Рейтинг")
                    searchResult.Sort((x, y) => ((Int32)x.Rating).CompareTo(y.Rating));
            }
            paginationPageIndex = 0;

            Pagination();
            SignalChanged("Suppliers");
        }

        private void InitPagination()
        {
            SearchCountRows = $"Найдено записей: {searchResult.Count} из {Suppliers.Count()}";
            paginationPageIndex = 0;
        }

        private void Pagination()
        {
            
            int rowsOnPage = 0;
            if (!int.TryParse(SelectedViewCountRows, out rowsOnPage))
            {
                Suppliers = searchResult;
            }
            else
            {
                Suppliers = searchResult.Skip(rowsOnPage * paginationPageIndex).Take(rowsOnPage).ToList();
                SignalChanged("Suppliers");
                int.TryParse(SelectedViewCountRows, out rows);
                CountPages = (searchResult.Count() - 1) / rows;
                Pages = $"{paginationPageIndex + 1} из {CountPages + 1}";
            }
        }

        private void Search()
        {
            var search = SearchText.ToLower();
            if (SelectedSearchType == "Название поставщика")
            {
                searchResult = Suppliers.Where(c => c.Title.ToLower().Contains(search)).ToList();
            }
            else if (SelectedSearchType == "Рейтинг")
                searchResult = Suppliers.Where(c => c.Rating.ToString().ToLower().Contains(search)).ToList();
            else if(SelectedSearchType == "Почта")
                searchResult = Suppliers.Where(c => c.Email.ToLower().Contains(search)).ToList();
            Sort();
            InitPagination();
            Pagination();
            SignalChanged("Suppliers");
        }
    }
}
