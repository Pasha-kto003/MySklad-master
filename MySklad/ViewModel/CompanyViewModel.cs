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
    public class CompanyViewModel : BaseViewModel
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

        public CustomCommand AddCompany { get; set; }
        public CustomCommand EditCompany { get; set; }
        public CustomCommand DeleteCompany { get; set; }
        public CustomCommand BackPage { get; set; }
        public CustomCommand ForwardPage { get; set; }
        public CustomCommand UpdateList { get; set; }

        private CompanyApi selectedCompany { get; set; }
        public CompanyApi SelectedCompany
        {
            get => selectedCompany;
            set
            {
                selectedCompany = value;
                SignalChanged();
            }
        }

        public List<SupplierApi> Suppliers { get; set; }

        private async Task GetSuppliers()
        {
            Suppliers = await Api.GetListAsync<List<SupplierApi>>("Supplier");
        }

        private async Task GetCompanies()
        {
            var companies = await Api.GetListAsync<List<CompanyApi>>("Company");
            Companies = (List<CompanyApi>)companies;
            CountAll = Companies.Count;
            SignalChanged("Companies");
            InitPagination();
        }

        private async Task CompanyDelete(CompanyApi company)
        {
            var companyApi = await Api.DeleteAsync<CompanyApi>(company, "Company");
        }

        public List<CompanyApi> searchResult;

        int paginationPageIndex = 0;
        private string searchCountRows;
        private string selectedViewCountRows;
        public int rows = 0;
        public int CountPages = 0;
        private int CountAll { get; set; }

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

        public CompanyViewModel()
        {
            Companies = new List<CompanyApi>();
            GetCompanies();
            GetSuppliers();
            ViewCountRows = new List<string>();
            ViewCountRows.AddRange(new string[] { "5", "все" });
            selectedViewCountRows = ViewCountRows.Last();

            SearchType = new List<string>();
            SearchType.AddRange(new string[] { "Название" });
            SearchType.AddRange(new string[] { "Email" });
            SearchType.AddRange(new string[] { "Телефон" });
            SearchType.AddRange(new string[] { "Адрес" });
            SearchType.AddRange(new string[] { "Дата регистрации" });

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
                GetCompanies();
            });

            AddCompany = new CustomCommand(() =>
            {
                AddCompanyView addCompany = new AddCompanyView();
                addCompany.ShowDialog();
                GetCompanies();
            });
            EditCompany = new CustomCommand(() =>
            {
                if (SelectedCompany == null) return;
                AddCompanyView addCompany = new AddCompanyView(SelectedCompany);
                addCompany.ShowDialog();
                GetCompanies();
            });

            DeleteCompany = new CustomCommand(() =>
            {
                GetSuppliers();
                if (SelectedCompany == null) return;
                var search = Suppliers.Where(s => s.CompanyId == SelectedCompany.Id);
                if (search.Count() != 0)
                {
                    MessageBox.Show("У компании есть поставщики, ее нельзя удалить");
                    return;
                }
                MessageBoxResult result = MessageBox.Show($"Вы точно хотите удалить {SelectedCompany.NameOfCompany}", "Подтвердите действие", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    CompanyDelete(SelectedCompany);
                    GetCompanies();
                }

            });

            SignalChanged("Companies");
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
                Companies = searchResult;
            }
            else
            {
                Companies = searchResult.Skip(rowsOnPage * paginationPageIndex).Take(rowsOnPage).ToList();
                SignalChanged("Companies");
                int.TryParse(SelectedViewCountRows, out rows);
                CountPages = (searchResult.Count() - 1) / rows;
                Pages = $"{paginationPageIndex + 1} из {CountPages + 1}";
            }
        }
        private void Search()
        {
            var search = SearchText.ToLower();
            if (SelectedSearchType == "Название")
                searchResult = Companies
                    .Where(c => c.NameOfCompany.ToLower().Contains(search)).ToList();
            else if (SelectedSearchType == "Email")
                searchResult = Companies
                    .Where(s => s.Email.ToLower().Contains(search)).ToList();
            else if (SelectedSearchType == "Адрес")
                searchResult = Companies.Where(c => c.Address.ToLower().Contains(search)).ToList();
            else if (SelectedSearchType == "Телефон")
                searchResult = Companies.Where(c => c.Phone.ToLower().Contains(search)).ToList();
            else if (SelectedSearchType == "Дата регистрации")
                searchResult = Companies.Where(c => c.RegistrationDate.ToString().ToLower().Contains(search)).ToList();
            InitPagination();
            Pagination();
            SignalChanged("Companies");
        }
    }
}
