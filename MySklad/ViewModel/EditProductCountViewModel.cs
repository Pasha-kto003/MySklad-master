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
    public class EditProductCountViewModel : BaseViewModel
    {
        public ProductApi AddProductVM { get; set; }
        public CustomCommand SaveCount { get; set; }

        public async Task EditCount(ProductApi productApi)
        {
            var count = await Api.PutAsync<ProductApi>(productApi, "Product");
        }

        public void CloseWindow(object obj)
        {
            Window window = obj as Window;
            window.Close();
        }

        private int? countProducts;
        public int? CountProducts
        {
            get => countProducts;
            set
            {
                countProducts = value;
                SignalChanged();
            }
        }

        public EditProductCountViewModel(ProductApi productApi)
        {
            CountProducts = productApi.CountProducts;
            SaveCount = new CustomCommand(() =>
            {
                CrossProductOrderApi crossProductOrderApi = new CrossProductOrderApi();
                productApi.CountProducts = CountProducts;
                productApi.CrossProductOrderApi = crossProductOrderApi;
                crossProductOrderApi.CountInOrder = productApi.CountProducts;
                EditCount(productApi);

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.DataContext == this)
                    {
                        CloseWindow(window);
                    }
                }
            });
        }
    }
}
