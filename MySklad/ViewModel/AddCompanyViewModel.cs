using Microsoft.Win32;
using ModelApi;
using MySklad.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MySklad.ViewModel
{
    public class AddCompanyViewModel : BaseViewModel
    {
        public CompanyApi AddCompanyVM { get; set; }
        public CustomCommand SaveCompany { get; set; }
        public CustomCommand SelectImage { get; set; }

        private BitmapImage imageCompany;
        public BitmapImage ImageCompany
        {
            get => imageCompany;
            set
            {
                imageCompany = value;
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

        private async Task PostCompany(CompanyApi companyApi)
        {
            var company = await Api.PostAsync<CompanyApi>(companyApi, "Company");
        }

        private async Task EditCompany(CompanyApi companyApi)
        {
            var company = await Api.PutAsync<CompanyApi>(companyApi, "Company");
        }

        public AddCompanyViewModel(CompanyApi companyApi)
        {
            if (companyApi == null)
            {
                AddCompanyVM = new CompanyApi { Phone = "12345", NameOfCompany = "Название компании", RegistrationDate = DateTime.Now };
            }
            else
            {
                AddCompanyVM = new CompanyApi
                {
                    Id = companyApi.Id,
                    NameOfCompany = companyApi.NameOfCompany,
                    Email = companyApi.Email,
                    Phone = companyApi.Phone,
                    Image = companyApi.Image,
                    RegistrationDate = companyApi.RegistrationDate,
                    Address = companyApi.Address
                };

                if (ImageCompany == null)
                {
                    ImageCompany = GetImageFromPath(Environment.CurrentDirectory + "//" + AddCompanyVM.Image);
                }
                else
                {
                    ImageCompany = GetImageFromPath(Environment.CurrentDirectory + "//" + AddCompanyVM.Image);
                }
            }
            SelectImage = new CustomCommand(() =>
            {
                    OpenFileDialog ofd = new OpenFileDialog();
                    if (ofd.ShowDialog() == true)
                    {
                        try
                        {
                            var info = new FileInfo(ofd.FileName);
                            ImageCompany = GetImageFromPath(ofd.FileName);
                            AddCompanyVM.Image = $"/company/{info.Name}";

                            var newPath = Environment.CurrentDirectory + AddCompanyVM.Image;
                            File.Copy(ofd.FileName, newPath);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    }
            });

            SaveCompany = new CustomCommand(() =>
            {
                try
                {
                    if(AddCompanyVM.NameOfCompany == null)
                    {
                        MessageBox.Show("Не введено название компании");
                        return;
                    }
                    if(AddCompanyVM.Phone == null)
                    {
                        MessageBox.Show("Не введен телефон компании");
                        return;
                    }
                    if(AddCompanyVM.RegistrationDate == null)
                    {
                        MessageBox.Show("Не введена дата регистрации компании");
                        return;
                    }
                    if(AddCompanyVM.Email == null || !AddCompanyVM.Email.Contains("@") || !AddCompanyVM.Email.EndsWith(".com"))
                    {
                        MessageBox.Show("Неправильно введена почта предприятия");
                        return;
                    }
                    if(AddCompanyVM.Address == null)
                    {
                        MessageBox.Show("Не введен адрес");
                        return;
                    }
                    if(AddCompanyVM.Id == 0)
                    {
                        PostCompany(AddCompanyVM);
                    }
                    else
                    {
                        EditCompany(AddCompanyVM);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.DataContext == this)
                    {
                        CloseModalWindow(window);
                    }
                }

                SignalChanged("Companies");
            });
            
        }

        private BitmapImage GetImageFromPath(string url)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.UriSource = new Uri(url, UriKind.Absolute);
            img.EndInit();
            return img;
        }

        public void CloseModalWindow(object obj)
        {
            Window window = obj as Window;
            window.Close();
        }
    }
}
