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
                SelectedProduct.CrossProductRack = CrossProductRacks.FirstOrDefault(s => s.ProductId == SelectedProduct.Id);
                if (SelectedProduct == null)
                {
                    MessageBox.Show("Выберите продукцию!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else if (SelectedRackProducts.Contains(SelectedProduct))
                {
                    MessageBox.Show("Данная продукция уже есть на стеллаже!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else if(SelectedProduct.Status == "Удален")
                {
                    MessageBoxResult result = MessageBox.Show("Этот товар находится в реестре удалений, его невозможно поместить на стеллаж!", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if(result == MessageBoxResult.Yes)
                    {
                        SelectedRackProducts.Remove(SelectedRackProduct);
                        CrossProductRacks.Remove(AddRackVM.CrossProductRacks);
                        MessageBox.Show("Удаление завершено");
                    }
                }
                
                else if(SelectedProduct.CrossProductRack != null)
                {
                    MessageBox.Show("Ошибка, данная продукция уже расположена на стеллаже");
                }
                else
                {
                    SelectedRackProduct = SelectedProduct;
                    SelectedRackProducts.Add(SelectedProduct);
                    AddRackVM.ChangedDate = DateTime.Now;
                    SignalChanged("SelectedRackProducts");
                }
            });

            SaveRack = new CustomCommand(() =>
            {
                AddRackVM.CrossProductRacks = CrossProductRacks.FirstOrDefault(s => s.RackId == AddRackVM.Id);
                AddRackVM.Products = SelectedRackProducts;
                if (AddRackVM.Id == 0)
                {
                    AddRackVM.PersonalId = SelectedPersonal.Id;
                    AddRackVM.CrossProductRacks = CrossProductRacks.FirstOrDefault(s => s.RackId == AddRackVM.Id);
                    //SelectedRackProduct.CountInStock = 0;
                    foreach (ProductApi product in SelectedRackProducts)
                    {
                        SelectedRackProduct.CountInStock += product.CountInStock;
                    }
                    SignalChanged("SelectedRackProduct");
                    //SignalChanged("SelectedRackProducts");
                    AddRackVM.RemainingPlaces = AddRackVM.Capacity - SelectedRackProduct.CountInStock / 2;
                    if (AddRackVM.RemainingPlaces < 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Не хватает мест для данного товара, его следует удалить!", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes)
                        {
                            SelectedRackProducts.Remove(SelectedRackProduct);
                            CrossProductRacks.Remove(AddRackVM.CrossProductRacks);
                            MessageBox.Show("Удаление завершено");
                        }
                    }
                    else
                    {
                        PostRack(AddRackVM);
                        MessageBox.Show("Добавлен новый стеллаж");
                    }
                    
                }
                else
                {
                    AddRackVM.PersonalId = SelectedPersonal.Id;
                    AddRackVM.CrossProductRacks = CrossProductRacks.FirstOrDefault(s => s.RackId == AddRackVM.Id);

                    //foreach (CrossProductRackApi productApi in CrossProductRacks.Where(s => s.RackId == AddRackVM.Id))
                    //{
                    //    productApi.Product = SelectedRackProduct;
                    //    productApi.Product.CountInStock += SelectedRackProduct.CountInStock;
                    //    AddRackVM.RemainingPlaces = AddRackVM.Capacity - SelectedProduct.CountInStock;
                    //}
                    //SelectedRackProduct.CountInStock = 0;
                    foreach (ProductApi product in SelectedRackProducts)
                    {
                        SelectedRackProduct.CountInStock += product.CountInStock;
                    }
                    AddRackVM.RemainingPlaces = AddRackVM.Capacity - SelectedRackProduct.CountInStock / 2;
                    if (AddRackVM.RemainingPlaces < 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Не хватает мест для данного товара, его следует удалить!", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes)
                        {
                            SelectedRackProducts.Remove(SelectedRackProduct);
                            //CrossProductRacks.Remove(AddRackVM.CrossProductRacks);
                            MessageBox.Show("Удаление завершено");
                        }
                    }
                    else
                    {
                        EditRack(AddRackVM);
                        MessageBox.Show($"Вы только что изменили стелаж :{AddRackVM.Name}");
                    }
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
