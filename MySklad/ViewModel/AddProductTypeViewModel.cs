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
    public class AddProductTypeViewModel : BaseViewModel
    {
        public ProductTypeApi AddTypeVM { get; set; }
        public CustomCommand SaveType { get; set; }

        public async Task CreateType(ProductTypeApi typeApi)
        {
            var unit = await Api.PostAsync<ProductTypeApi>(typeApi, "ProductType");
        }

        public async Task EditType(ProductTypeApi typeApi)
        {
            var unit = await Api.PutAsync<ProductTypeApi>(typeApi, "ProductType");
        }

        public void CloseWindow(object obj)
        {
            Window window = obj as Window;
            window.Close();
        }
        public AddProductTypeViewModel(ProductTypeApi productType)
        {
            if (productType == null)
            {
                AddTypeVM = new ProductTypeApi { };
            }
            else
            {
                AddTypeVM = new ProductTypeApi
                {
                    Id = productType.Id,
                    Title = productType.Title
                };
            }

            SaveType = new CustomCommand(() =>
            {
                if(AddTypeVM.Title == null)
                {
                    MessageBox.Show("Введите тип продукции");
                    return;
                }
                if (AddTypeVM.Id == 0)
                {
                    CreateType(AddTypeVM);
                }
                else
                {
                    EditType(AddTypeVM);
                }
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.DataContext == this)
                    {
                        CloseWindow(window);
                    }
                }
                SignalChanged("ProductTypes");
            });
        }
    }
}
