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
    public class PersonalViewModel : BaseViewModel
    {

        private PersonalApi selectedPersonal { get; set; }
        public PersonalApi SelectedPersonal
        {
            get => selectedPersonal;
            set
            {
                selectedPersonal = value;
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

        private List<StatusApi> statuses { get;set; }
        public List<StatusApi> Statuses
        {
            get => statuses;
            set
            {
                statuses = value;
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

        public List<PersonalApi> searchResult;

        int paginationPageIndex = 0;
        private string searchCountRows;
        private string selectedViewCountRows;
        public int rows = 0;
        public int CountPages = 0;

        public CustomCommand BackPage { get; set; }
        public CustomCommand ForwardPage { get; set; }
        public CustomCommand AddPersonal { get; set; }
        public CustomCommand EditPersonal { get; set; }


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

        public PersonalViewModel()
        {
            Personals = new List<PersonalApi>();
            Statuses = new List<StatusApi>();
            GetPersonals();

            ViewCountRows = new List<string>();
            ViewCountRows.AddRange(new string[] { "15", "все" });
            selectedViewCountRows = ViewCountRows.First();

            SortType = new List<string>();
            SortType.AddRange(new string[] { "Рейтинг", "Телефон" });
            selectedSortType = SortType.First();

            OrderType = new List<string>();
            OrderType.AddRange(new string[] { "По возрастанию", "По убыванию", "По умолчанию" });
            selectedOrderType = OrderType.Last();

            SearchType = new List<string>();
            SearchType.AddRange(new string[] { "Фамилия", "Рейтинг"});
            selectedSearchType = SearchType.First();

            BackPage = new CustomCommand(() => {
                if (searchResult == null)
                    return;
                if (paginationPageIndex > 0)
                    paginationPageIndex--;
                Pagination();
                GetPersonals();
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
                InitPagination();
                Pagination();
                GetPersonals();
            });

            AddPersonal = new CustomCommand(() =>
            {
                AddPersonalView addPersonal = new AddPersonalView();
                addPersonal.ShowDialog();
                GetPersonals();
            });

            EditPersonal = new CustomCommand(() =>
            {
                if (SelectedPersonal == null) return;
                AddPersonalView addPersonal = new AddPersonalView(SelectedPersonal);
                addPersonal.ShowDialog();
                GetPersonals();
            });
        }

        private async Task GetPersonals()
        {
            Personals = await Api.GetListAsync<List<PersonalApi>>("Personal");
            Statuses = await Api.GetListAsync<List<StatusApi>>("Status");
            foreach (PersonalApi personal in Personals)
            {
                personal.Status = Statuses.First(s => s.Id == personal.StatusId);
            }
            SignalChanged("Personals");
        }

        internal void Sort()
        {
            if (SelectedOrderType == "По умолчанию")
                return;

            if (SelectedOrderType == "По убыванию")
            {
                if (SelectedSortType == "Рейтинг")
                    searchResult.Sort((x, y) => y.Rating.CompareTo(x.Rating));
                else if (SelectedSortType == "Телефон")
                    searchResult.Sort((x, y) => y.Phone.CompareTo(x.Phone));
            }

            if (SelectedOrderType == "По возрастанию")
            {
                if (SelectedSortType == "Рейтинг")
                    searchResult.Sort((x, y) => x.Rating.CompareTo(y.Rating));
                else if (SelectedSortType == "Телефон")
                    searchResult.Sort((x, y) => x.Phone.CompareTo(y.Phone));
            }
            paginationPageIndex = 0;
            Pagination();
            SignalChanged("Shops");
            GetPersonals();
        }

        private void InitPagination()
        {
            SearchCountRows = $"Найдено записей: {searchResult.Count} из {Personals.Count}";
            paginationPageIndex = 0;
        }

        private void Pagination()
        {

            int rowsOnPage = 0;
            if (!int.TryParse(SelectedViewCountRows, out rowsOnPage))
            {
                searchResult = Personals;
            }
            else
            {
                Personals = searchResult.Skip(rowsOnPage * paginationPageIndex).Take(rowsOnPage).ToList();
                SignalChanged("Personals");
                int.TryParse(SelectedViewCountRows, out rows);
                CountPages = searchResult.Count() / rows;
                Pages = $"{paginationPageIndex + 1} из {CountPages + 1}";
            }
            //ShopsApi();
        }

        private void Search()
        {
            var search = SearchText.ToLower();
            if (SelectedSearchType == "Фамилия")
                searchResult = Personals
                    .Where(c => c.LastName.ToLower().Contains(search)).ToList();
            else if (SelectedSearchType == "Рейтинг")
                searchResult = Personals
                        .Where(c => c.Rating.ToLower().Contains(search)).ToList();
            Sort();
            InitPagination();
            Pagination();
            SignalChanged("Shops");
            GetPersonals();
        }
    }
}
