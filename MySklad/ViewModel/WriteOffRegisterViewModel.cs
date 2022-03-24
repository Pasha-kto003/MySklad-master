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
    public class WriteOffRegisterViewModel : BaseViewModel
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

        private List<WriteOffRegisterApi> registers { get; set; }
        public List<WriteOffRegisterApi> Registers
        {
            get => registers;
            set
            {
                registers = value;
                SignalChanged();
            }
        }


        private WriteOffRegisterApi selectedRegister { get; set; }
        public WriteOffRegisterApi SelectedRegister
        {
            get => selectedRegister;
            set
            {
                selectedRegister = value;
                SignalChanged();
            }
        }

        public CustomCommand AddRegister { get; set; }
        public CustomCommand EditRegister { get; set; }
        public CustomCommand UpdateList { get; set; }
        public CustomCommand BackPage { get; set; }
        public CustomCommand ForwardPage { get; set; }

        public List<WriteOffRegisterApi> searchResult;

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

        int paginationPageIndex = 0;
        private string searchCountRows;
        private string selectedViewCountRows;
        public int rows = 0;
        public int CountPages = 0;
        public WriteOffRegisterViewModel()
        {

            Products = new List<ProductApi>();
            Registers = new List<WriteOffRegisterApi>();
            GetRegisters();

            ViewCountRows = new List<string>();
            ViewCountRows.AddRange(new string[] { "5", "все" });
            selectedViewCountRows = ViewCountRows.Last();

            SearchType = new List<string>();
            SearchType.AddRange(new string[] { "Дата удаления", "Продукт" });
            selectedSearchType = SearchType.First();

            AddRegister = new CustomCommand(() =>
            {
                AddWriteOffRegisterView addWriteOff = new AddWriteOffRegisterView();
                addWriteOff.ShowDialog();
                GetRegisters();
            });

            EditRegister = new CustomCommand(() =>
            {
                if (SelectedRegister == null) return;
                AddWriteOffRegisterView addWriteOff = new AddWriteOffRegisterView(SelectedRegister);
                addWriteOff.ShowDialog();
                GetRegisters();
            });

            BackPage = new CustomCommand(() => {
                if (searchResult == null)
                    return;
                if (paginationPageIndex > 0)
                    paginationPageIndex--;
                Pagination();

            });

            UpdateList = new CustomCommand(() =>
            {
                GetRegisters();
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

            SignalChanged("Registers");
            Search();
            InitPagination();
            Pagination();
        }

        private void InitPagination()
        {
            SearchCountRows = $"Найдено записей: {searchResult.Count} из {Registers.Count}";
            paginationPageIndex = 0;
        }

        private void Pagination()
        {
            int rowsOnPage = 0;
            if (!int.TryParse(SelectedViewCountRows, out rowsOnPage))
            {
                Registers = searchResult;
            }
            else
            {
                Registers = searchResult.Skip(rowsOnPage * paginationPageIndex).Take(rowsOnPage).ToList();
                SignalChanged("Registers");
                int.TryParse(SelectedViewCountRows, out rows);
                CountPages = (searchResult.Count() - 1) / rows;
                Pages = $"{paginationPageIndex + 1} из {CountPages + 1}";
            }
        }

        private async Task GetRegisters()
        {
            Registers = await Api.GetListAsync<List<WriteOffRegisterApi>>("WriteOffRegister");
            Products = await Api.GetListAsync<List<ProductApi>>("Product");
            foreach (WriteOffRegisterApi registerApi in Registers)
            {
                registerApi.Product = Products.First(s => s.Id == registerApi.ProductId);
            }
            SignalChanged("Registers");
            InitPagination();
        }

        private void Search()
        {
            var search = SearchText.ToLower();
            if (SelectedSearchType == "Дата удаления")
                searchResult = Registers
                    .Where(c => c.DeletedAt.ToString().ToLower().Contains(search)).ToList();
            else if (SelectedSearchType == "Продукт")
                searchResult = Registers
                        .Where(c => c.Product.Title.ToLower().Contains(search)).ToList();
            InitPagination();
            Pagination();
            SignalChanged("Registers");
        }
    }
}
