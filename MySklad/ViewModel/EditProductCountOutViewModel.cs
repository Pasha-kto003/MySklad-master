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
    public class EditProductCountOutViewModel : BaseViewModel
    {
        public ProductApi AddProductVM { get; set; }
        public CustomCommand SaveCount { get; set; }

        public async Task EditCount(ProductApi productApi)
        {
            var count = await Api.PutAsync<ProductApi>(productApi, "Product");
        }

        public async Task EditCross(OrderOutApi orderOutApi)
        {
            var order = await Api.PutAsync<OrderOutApi>(orderOutApi, "OrderOut");
        }

        public void CloseWindow(object obj)
        {
            Window window = obj as Window;
            window.Close();
        }

        private int? countProductsOut;
        public int? CountProductsOut
        {
            get => countProductsOut;
            set
            {
                countProductsOut = value;
                SignalChanged();
            }
        }

        public EditProductCountOutViewModel(ProductApi productApi)
        {
            CountProductsOut = productApi.CountProductsOut;
            SaveCount = new CustomCommand(() =>
            {
                CrossOrderOutApi crossProductOrderApi = new CrossOrderOutApi();
                OrderOutApi orderOutApi = new OrderOutApi();
                productApi.CountProductsOut = CountProductsOut;
                crossProductOrderApi.CountOutOrder = productApi.CountProductsOut;
                EditCount(productApi);
                EditCross(orderOutApi);
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
