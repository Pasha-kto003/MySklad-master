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
    public class RacksViewModel : BaseViewModel
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

        public List<RackApi> searchResult { get; set; }
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

        private RackApi selectedRack { get; set; }
        public RackApi SelectedRack
        {
            get => selectedRack;
            set
            {
                selectedRack = value;
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

        private async Task GetRacks()
        {
            Personals = await Api.GetListAsync<List<PersonalApi>>("Personal");
            Racks = await Api.GetListAsync<List<RackApi>>("Rack");
            foreach (RackApi rackApi in Racks)
            {
                rackApi.Personal = Personals.First(s => s.Id == rackApi.PersonalId);
            }
            CountAll = Racks.Count;
        }

        private async Task DeleteRack(RackApi rack)
        {
            var delete = await Api.DeleteAsync<RackApi>(rack, "Rack");
        }

        public CustomCommand EditRack { get; set; }
        public CustomCommand CreateRack { get; set; }
        public CustomCommand BackPage { get; set; }
        public CustomCommand ForwardPage { get; set; }
        public CustomCommand UpdateList { get; set; }
        public CustomCommand RemoveRack { get; set; }

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

        public RacksViewModel()
        {
            Racks = new List<RackApi>();
            Personals = new List<PersonalApi>();
            GetRacks();

            ViewCountRows = new List<string>();
            ViewCountRows.AddRange(new string[] { "5", "все" });
            selectedViewCountRows = ViewCountRows.Last();

            SortType = new List<string>();
            SortType.AddRange(new string[] { "Дата", "Вместительность", "Остаток мест" });
            selectedSortType = SortType.First();

            OrderType = new List<string>();
            OrderType.AddRange(new string[] { "По возрастанию", "По убыванию", "По умолчанию" });
            selectedOrderType = OrderType.Last();

            SearchType = new List<string>();
            SearchType.AddRange(new string[] { "Дата последнего изменения", "Сотрудник", "Остаток мест", "Наименование" });
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
                GetRacks();
            });

            CreateRack = new CustomCommand(() =>
            {
                EditRackView editRack = new EditRackView();
                editRack.ShowDialog();
                GetRacks();
            });

            EditRack = new CustomCommand(() =>
            {
                if (SelectedRack == null) return;
                EditRackView editRack = new EditRackView(SelectedRack);
                editRack.ShowDialog();
                GetRacks();
            });

            RemoveRack = new CustomCommand(() =>
            {
                if (SelectedRack == null) return;
                MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить этот стеллаж?", "Поддьвердите ваше действие", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    DeleteRack(SelectedRack);
                }
                else
                {
                    return;
                }
            });

            SignalChanged("Racks");
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
                Racks = searchResult;
            }
            else
            {
                Racks = searchResult.Skip(rowsOnPage * paginationPageIndex).Take(rowsOnPage).ToList();
                SignalChanged("Racks");
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
                    searchResult.Sort((x, y) => y.ChangedDate.ToString().CompareTo(x.ChangedDate));
                else if (SelectedSortType == "Вместительность")
                    searchResult.Sort((x, y) => y.Capacity.ToString().CompareTo(x.Capacity));
                else if (SelectedSortType == "Остаток")
                    searchResult.Sort((x, y) => y.RemainingPlaces.ToString().CompareTo(x.RemainingPlaces));
            }

            if (SelectedOrderType == "По возрастанию")
            {
                if (SelectedSortType == "Дата")
                    searchResult.Sort((x, y) => x.ChangedDate.ToString().CompareTo(y.ChangedDate));
                else if (SelectedSortType == "Вместительность")
                    searchResult.Sort((x, y) => x.Capacity.ToString().CompareTo(y.Capacity));
                else if (SelectedSortType == "Остаток")
                    searchResult.Sort((x, y) => x.RemainingPlaces.ToString().CompareTo(y.RemainingPlaces));
            }
            paginationPageIndex = 0;

            Pagination();
            SignalChanged("Racks");
        }
        private void Search()
        {
            var search = SearchText.ToLower();
            if (SelectedSearchType == "Дата последнего изменения")
                searchResult = Racks
                    .Where(c => c.ChangedDate.ToShortDateString().ToLower().Contains(search)).ToList();
            else if (SelectedSearchType == "Сотрудник")
                searchResult = Racks
                        .Where(c => c.Personal.LastName.ToLower().Contains(search)).ToList();
            else if (SelectedSearchType == "Остаток мест")
                searchResult = Racks.Where(c => c.RemainingPlaces.ToString().ToLower().Contains(search)).ToList();
            else if (SelectedSearchType == "Наименование")
                searchResult = Racks.Where(c => c.Name.ToLower().Contains(search)).ToList();
            Sort();
            InitPagination();
            Pagination();
            SignalChanged("Racks");
        }
    }
}
