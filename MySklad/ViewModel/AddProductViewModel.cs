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
    public class AddProductViewModel : BaseViewModel
    {
        private List<ProductTypeApi> productTypes { get; set; }
        public List<ProductTypeApi> ProductTypes
        {
            get => productTypes;
            set
            {
                productTypes = value;
                SignalChanged();
            }
        }

        private List<UnitApi> units { get; set; }
        public List<UnitApi> Units
        {
            get => units;
            set
            {
                units = value;
                SignalChanged();
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

        private ProductTypeApi selectedProductType { get; set; }
        public ProductTypeApi SelectedProductType
        {
            get => selectedProductType;
            set
            {
                selectedProductType = value;
                SignalChanged();
            }
        }

        private UnitApi selectedUnit { get; set; }
        public UnitApi SelectedUnit
        {
            get => selectedUnit;
            set
            {
                selectedUnit = value;
                SignalChanged();
            }
        }

        private ProductApi selectedProduct { get; set; }
        public ProductApi SelectedProduct
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                SignalChanged();
            }
        }

        public ProductApi AddProductVM { get; set; }

        public CustomCommand SaveProduct { get; set; }

        public void CloseWindow(object obj)
        {
            Window window = obj as Window;
            window.Close();
        }

        async Task GetUnits()
        {
            var units = await Api.GetListAsync<List<UnitApi>>("Unit");
            Units = (List<UnitApi>)units;
        }

        async Task GetProductTypes()
        {
            var types = await Api.GetListAsync<List<ProductTypeApi>>("ProductType");
            ProductTypes = (List<ProductTypeApi>)types;
        }

        async Task PostProduct(ProductApi productApi)
        {
            var product = await Api.PostAsync<ProductApi>(productApi, "Product");
        }

        public async Task EditProduct(ProductApi productApi)
        {
            var product = await Api.PutAsync<ProductApi>(productApi, "Product");
        }

        async Task GetProducts(ProductApi productApi)
        {
            ProductTypes = await Api.GetListAsync<List<ProductTypeApi>>("ProductType");
            Units = await Api.GetListAsync<List<UnitApi>>("Unit");
            
                if (productApi == null)
                {
                    SelectedUnit = Units.FirstOrDefault();
                    SelectedProductType = ProductTypes.FirstOrDefault();
                }
                else
                {
                    SelectedUnit = Units.FirstOrDefault(s => s.Id == productApi.UnitId);
                    SelectedProductType = ProductTypes.FirstOrDefault(s => s.Id == productApi.ProductTypeId);
                }
            
        }

        public AddProductViewModel(ProductApi productApi)
        {
            //Products = new List<ProductApi>();
            //Units = new List<UnitApi>();
            //ProductTypes = new List<ProductTypeApi>();
            GetUnits();
            GetProductTypes();

            if (productApi == null)
            {
                AddProductVM = new ProductApi { Title = "Название", Status = "В норме", Description = "Описание" };
            }
            else
            {
                AddProductVM = new ProductApi
                {
                    Id = productApi.Id,
                    Title = productApi.Title,
                    Description = productApi.Description,
                    Status = productApi.Status,
                    CountInStock = productApi.CountInStock,
                    BestBeforeDateStart = productApi.BestBeforeDateStart,
                    BestBeforeDateEnd = productApi.BestBeforeDateEnd,
                    ProductTypeId = productApi.ProductTypeId,
                    UnitId = productApi.UnitId
                };
            }
            GetProducts(AddProductVM);

            

            SaveProduct = new CustomCommand(() =>
            {
                if(AddProductVM.Id == 0)
                {
                    AddProductVM.UnitId = SelectedUnit.Id;
                    AddProductVM.ProductTypeId = SelectedProductType.Id;
                    PostProduct(AddProductVM);
                }
                else
                {
                    AddProductVM.UnitId = SelectedUnit.Id;
                    AddProductVM.ProductTypeId = SelectedProductType.Id; //
                    EditProduct(AddProductVM);
                }

                if (AddProductVM.BestBeforeDateEnd < AddProductVM.BestBeforeDateStart)
                {
                    MessageBox.Show("Ошибка, срок годности не соответствует действительности");
                    return;
                }

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.DataContext == this)
                    {
                        CloseWindow(window);
                    }
                }
                SignalChanged("Products");
            });

            
        }
    }
}
