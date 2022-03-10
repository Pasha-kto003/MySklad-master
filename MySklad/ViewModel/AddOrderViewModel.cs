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
    public class AddOrderViewModel : BaseViewModel
    {
        public RackApi AddRackVM { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        private ProductApi selectedRackProduct { get; set; }
        public ProductApi SelectedRackProduct
        {
            get => selectedRackProduct;
            set
            {
                selectedRackProduct = value;
                SignalChanged();
            }
        }

        private List<ProductApi> selectedRackProducts { get; set; }
        public List<ProductApi> SelectedRackProducts 
        {
            get => selectedRackProducts;
            set
            {
                selectedRackProducts = value;
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

        private List<ProductApi> product { get; set; }
        public List<ProductApi> Product
        {
            get => product;
            set
            {
                product = value;
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

        private PersonalApi personal { get; set; }

        public PersonalApi SelectedPersonal 
        {
            get => personal;
            set
            {
                personal = value;
                SignalChanged();
            }
        }

        public CustomCommand SaveRack { get; set; }
        public CustomCommand AddProduct { get; set; }
        public CustomCommand EditProduct { get; set; }
        public CustomCommand RemoveProduct { get; set; }

        public void CloseWindow(object obj)
        {
            Window window = obj as Window;
            window.Close();
        }

        async Task GetCrosses()
        {
            var cross = await Api.GetListAsync<List<CrossProductRackApi>>("CrossRack");
        }

        async Task GetProducts()
        {
            var products = await Api.GetListAsync<List<ProductApi>>("Product");
            Product = (List<ProductApi>)products;
        }


        async Task EditProduction(ProductApi productApi)
        {
            var prod = Api.PutAsync<ProductApi>(productApi, "Product");
        }

        async Task GetRacs(RackApi rackApi)
        {
            Product = await Api.GetListAsync<List<ProductApi>>("Product");
            Personals = await Api.GetListAsync<List<PersonalApi>>("Personal");

            if(rackApi == null)
            {
                SelectedPersonal = Personals.FirstOrDefault();
            }
            else
            {
                SelectedPersonal = Personals.FirstOrDefault(s => s.Id == rackApi.PersonalId);
            }
        }

        public List<CrossProductRackApi> CrossProductRacks { get; set; }

        async Task PostRack(RackApi rackApi)
        {
            var rack = await Api.PostAsync<RackApi>(rackApi, "Rack");
        }

        async Task EditRack(RackApi rackApi)
        {
            var rack = await Api.PutAsync<RackApi>(rackApi, "Rack");
        }

        public AddOrderViewModel(RackApi rackApi)
        {
            GetProducts();
            GetCrosses();
            
            if(rackApi == null)
            {
                AddRackVM = new RackApi { Name = "A1" };
            }
            else
            {
                AddRackVM = new RackApi
                {
                    Id = rackApi.Id,
                    Capacity = rackApi.Capacity,
                    Name = rackApi.Name,
                    PersonalId = rackApi.PersonalId,
                    RemainingPlaces = rackApi.RemainingPlaces
                };

                if(rackApi.Products != null)
                {
                    SelectedRackProducts = rackApi.Products;
                }
            }

            GetRacs(AddRackVM);

            AddProduct = new CustomCommand(() =>
            {
                if (SelectedProduct == null)
                {
                    MessageBox.Show("Выберите продукцию!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else if (SelectedRackProducts.Contains(SelectedProduct))
                {
                    MessageBox.Show("Данная продукция уже есть в заказе!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    SelectedRackProduct = SelectedProduct;
                    EditProduction(SelectedRackProduct);
                    SelectedRackProducts.Add(SelectedProduct);
                    SignalChanged("SelectedRackProducts");
                }
            });

            SaveRack = new CustomCommand(() =>
            {
                AddRackVM.Products = SelectedRackProducts;
                AddRackVM.CrossProductRacks.PlacementDate = StartDate;
                if(AddRackVM.Id == 0)
                {
                    AddRackVM.PersonalId = SelectedPersonal.Id;
                    PostRack(AddRackVM);
                }
                else
                {
                    AddRackVM.PersonalId = SelectedPersonal.Id;
                    EditRack(AddRackVM);
                }

                if(AddRackVM.Products != null)
                {
                    AddRackVM.CrossProductRacks.PlacementDate = StartDate;
                    EditRack(AddRackVM);
                    EditProduction(SelectedRackProduct);
                    MessageBox.Show("Записано");
                }

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.DataContext == this)
                    {
                        CloseWindow(window);
                    }
                }
                SignalChanged("OrderIns");
            });
        }
    }
}
