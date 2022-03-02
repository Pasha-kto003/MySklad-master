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
    public class CompanyViewModel : BaseViewModel
    {
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

        private async Task GetCompanies()
        {
            var companies = await Api.GetListAsync<List<CompanyApi>>("Company");
            Companies = (List<CompanyApi>)companies;
            SignalChanged("Companies");
        }

        public CompanyViewModel()
        {
            Companies = new List<CompanyApi>();
            GetCompanies();
            SignalChanged("Companies");

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
        }
    }
}
