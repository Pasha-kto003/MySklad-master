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
    public class EditRackViewModel : BaseViewModel
    {
        public RackApi AddRackVM { get; set; }

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

        private PersonalApi selectedPersonal { get; set; }
        public PersonalApi SelectedPersonal
        {
            get => selectedPersonal;
            set
            {
                selectedPersonal = value;
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

        private List<ProductApi> selectedRackProducts = new List<ProductApi>();
        public List<ProductApi> SelectedRackProducts
        {
            get => selectedRackProducts;
            set
            {
                selectedRackProducts = value;
                SignalChanged();
            }
        }

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

        public CustomCommand SaveRack { get; set; }
        public CustomCommand AddProduct { get; set; }
        public CustomCommand EditProduct { get; set; }
        public CustomCommand RemoveProduct { get; set; }
        public CustomCommand CountProduct { get; set; }

        public void CloseWindow(object obj)
        {
            Window window = obj as Window;
            window.Close();
        }

        async Task GetProducts()
        {
            var products = await Api.GetListAsync<List<ProductApi>>("Product");
            Product = (List<ProductApi>)products;
        }

        async Task GetPersonals()
        {
            var personals = await Api.GetListAsync<List<PersonalApi>>("Personal");
            Personals = (List<PersonalApi>)personals;
        }

        async Task GetRacks(RackApi rackApi)
        {
            Personals = await Api.GetListAsync<List<PersonalApi>>("Personal");
            Product = await Api.GetListAsync<List<ProductApi>>("Product");
            CrossProductRacks = await Api.GetListAsync<List<CrossProductRackApi>>("CrossRack");
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
            var editRack = await Api.PutAsync<RackApi>(rackApi, "Rack");
        }

        int products = 0;

        public EditRackViewModel(RackApi rackApi)
        {
            GetPersonals();
            GetProducts();
            if (rackApi == null)
            {
                AddRackVM = new RackApi { Name = "A1", Capacity = 500, PlacementDate = DateTime.Now };
            }
            else
            {
                AddRackVM = new RackApi
                {
                    Id = rackApi.Id,
                    PlacementDate = rackApi.PlacementDate,
                    ChangedDate = rackApi.ChangedDate,
                    DeletionDate = rackApi.DeletionDate,
                    Name = rackApi.Name,
                    Capacity = rackApi.Capacity,
                    RemainingPlaces = rackApi.RemainingPlaces,
                    PersonalId = rackApi.PersonalId
                };

                if (rackApi.Products != null)
                {
                    SelectedRackProducts = rackApi.Products;
                }
            }

            GetRacks(AddRackVM);

            AddProduct = new CustomCommand(() =>
            {
                if(SelectedProduct == null)
                {
                    MessageBox.Show("Выберите продукцию!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else if (SelectedRackProducts.Contains(SelectedProduct))
                {
                    MessageBox.Show("Данная продукция уже есть на стеллаже!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else
                {
                    SelectedRackProduct = SelectedProduct;
                    SelectedRackProducts.Add(SelectedProduct);
                    AddRackVM.ChangedDate = DateTime.Now;
                    SignalChanged("SelectedRackProducts");
                }
            });

            CountProduct = new CustomCommand(() =>
            {
                foreach (CrossProductRackApi productApi in CrossProductRacks.Where(s => s.RackId == AddRackVM.Id))
                {
                    productApi.Product = SelectedProduct;
                    productApi.Product.CountInStock++;
                }
                AddRackVM.RemainingPlaces = AddRackVM.Capacity - SelectedProduct.CountInStock;
            });

            SaveRack = new CustomCommand(() =>
            {
                //AddRackVM.CrossProductRacks = CrossProductRacks.FirstOrDefault(s => s.RackId == AddRackVM.Id);
                AddRackVM.Products = SelectedRackProducts;
                if(AddRackVM.Id == 0)
                {
                    AddRackVM.PersonalId = SelectedPersonal.Id;
                    AddRackVM.CrossProductRacks = CrossProductRacks.FirstOrDefault(s => s.RackId == AddRackVM.Id);
                    PostRack(AddRackVM);
                }
                else
                {
                    AddRackVM.PersonalId = SelectedPersonal.Id;
                    AddRackVM.CrossProductRacks = CrossProductRacks.FirstOrDefault(s => s.RackId == AddRackVM.Id);
                    EditRack(AddRackVM);
                }
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.DataContext == this)
                    {
                        CloseWindow(window);
                    }
                }
                SignalChanged("Racks");
            });
        }
    }
}
