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
    public class AddShopViewModel : BaseViewModel
    {
        public ShopApi AddShopVM { get; set; }

        public CustomCommand SaveShop { get; set; }
        public CustomCommand DeleteShop { get; set; }
        //public CustomCommand EditShop { get; set; }

        private List<ShopApi> shops { get; set; }
        public List<ShopApi> Shops { get; set; }

        public void CloseWindow(object obj)
        {
            Window window = obj as Window;
            window.Close();
        }

        public async Task CreateShop(ShopApi shopApi)
        {
            var shop = await Api.PostAsync<ShopApi>(shopApi, "Shop");
        }

        public async Task EditShop(ShopApi shopApi)
        {
            var shop = await Api.PutAsync<ShopApi>(shopApi, "Shop");
        }

        public AddShopViewModel(ShopApi shopApi)
        {
            Shops = new List<ShopApi>();
            if (shopApi == null)
            {
                AddShopVM = new ShopApi { Phone = "+7(___)-XXX-XXX-XX-XX", Email = "examplemail@gmail.com" };
            }
            else
            {
                AddShopVM = new ShopApi
                {
                    Id = shopApi.Id,
                    Name = shopApi.Name,
                    Email = shopApi.Email,
                    Phone = shopApi.Phone,
                    Address = shopApi.Address
                };
            }

            SaveShop = new CustomCommand(() =>
            {
                if(AddShopVM.Name == null)
                {
                    MessageBox.Show("Не введено название магазина");
                    return;
                }

                if(AddShopVM.Email == null || !AddShopVM.Email.Contains("@") || !AddShopVM.Email.EndsWith(".com"))
                {
                    MessageBox.Show("Не введена почта или введена не верно");
                    return;
                }

                if(AddShopVM.Phone == null)
                {
                    MessageBox.Show("Не введен номер телефона компании");
                    return;
                }

                if(AddShopVM.Address == null)
                {
                    MessageBox.Show("Не введен адрес");
                    return;
                }

                if (AddShopVM.Id == 0)
                {
                    CreateShop(AddShopVM);
                }
                else
                {
                    EditShop(AddShopVM);
                }
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.DataContext == this)
                    {
                        CloseWindow(window);
                    }
                }
                SignalChanged("Shops");
            });
        }
    }
}
