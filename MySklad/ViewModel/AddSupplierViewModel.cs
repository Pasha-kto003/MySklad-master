using ModelApi;
using MySklad.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MySklad.ViewModel
{
    public class AddSupplierViewModel : BaseViewModel
    {
        public CustomCommand SaveSupplier { get; set; }

        public SupplierApi AddSupplierVM { get; set; }

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

        public void CloseWindow(object obj)
        {
            Window window = obj as Window;
            window.Close();
        }

        private async Task GetCompanies()
        {
            var companies = await Api.GetListAsync<List<CompanyApi>>("Company");
        }

        private async Task GetSuppliers(SupplierApi supplierApi)
        {
            Companies = await Api.GetListAsync<List<CompanyApi>>("Company");
            if (supplierApi == null)
            {
                SelectedCompany = Companies.First();
            }
            else
            {
                SelectedCompany = Companies.First(s => s.Id == supplierApi.CompanyId);
            }
        }

        private async Task PostSupplier(SupplierApi supplierApi)
        {
            var supplier = await Api.PostAsync<SupplierApi>(supplierApi, "Supplier");
        }
        private async Task EditSupplier(SupplierApi supplierApi)
        {
            var supplier = await Api.PutAsync<SupplierApi>(supplierApi, "Supplier");
        }

        public AddSupplierViewModel(SupplierApi supplierApi)
        {
            Suppliers = new List<SupplierApi>();
            GetCompanies();
            GetSuppliers(AddSupplierVM);
            if (supplierApi == null)
            {
                AddSupplierVM = new SupplierApi { FirstName = "Имя поставщика", Email = "example@mail.com", Rating = 100, Status = "В норме", Phone = "+7(___)-XXX-XXX-XX-XX" };
            }
            else
            {
                AddSupplierVM = new SupplierApi
                {
                    Id = supplierApi.Id,
                    FirstName = supplierApi.FirstName,
                    LastName = supplierApi.LastName,
                    Patronimyc = supplierApi.Patronimyc,
                    Email = supplierApi.Email,
                    Rating = supplierApi.Rating,
                    Phone = supplierApi.Phone,
                    Status = supplierApi.Status,
                    CompanyId = supplierApi.CompanyId
                };
            }

            SaveSupplier = new CustomCommand(() =>
            {
                if(AddSupplierVM.FirstName == null || AddSupplierVM.LastName == null || AddSupplierVM.Patronimyc == null)
                {
                    MessageBox.Show("Не правильно введено ФИО");
                    return;
                }
                if(AddSupplierVM.Status == null)
                {
                    MessageBox.Show("Не введен статус");
                    return;
                }
                if(AddSupplierVM.Rating == null)
                {
                    MessageBox.Show("Не введен рейтинг");
                    return;
                }
                if(AddSupplierVM.Phone == null)
                {
                    MessageBox.Show("Не введен номер телефона");
                    return;
                }
                if(AddSupplierVM.Email == null || !AddSupplierVM.Email.Contains("@") || !AddSupplierVM.Email.EndsWith(".com"))
                {
                    MessageBox.Show("Ошибки в почтовом адрессе");
                    return;
                }
                if(AddSupplierVM.Id == 0)
                {
                    if(SelectedCompany == null)
                    {
                        MessageBox.Show("Не записана компания поставщика");
                    }
                    AddSupplierVM.CompanyId = SelectedCompany.Id;
                    PostSupplier(AddSupplierVM);
                }
                else
                {
                    AddSupplierVM.CompanyId = SelectedCompany.Id;
                    EditSupplier(AddSupplierVM);
                }
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.DataContext == this)
                    {
                        CloseWindow(window);
                    }
                }
                SignalChanged("Suppliers");
            });
        }
    }
}