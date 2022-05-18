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

        public int CountAllProducts { get; set; }

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

        private IEnumerable<CrossProductRackApi> selectedCrosses { get; set; }
        public IEnumerable<CrossProductRackApi> SelectedCrosses
        {
            get => selectedCrosses;
            set
            {
                selectedCrosses = value;
                SignalChanged();
            }
        }

        public List<RackApi> Racks { get; set; }

        private CrossProductRackApi selectedCross { get; set; }
        public CrossProductRackApi SelectedCross
        {
            get => selectedCross;
            set
            {
                selectedCross = value;
                SignalChanged();
            }
        }

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

        async Task GetProducts()
        {
            var products = await Api.GetListAsync<List<ProductApi>>("Product");
            Product = (List<ProductApi>)products;
        }

        async Task GetRackToUpdate()
        {
            Racks = await Api.GetListAsync<List<RackApi>>("Rack");
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
            ProductTypes = await Api.GetListAsync<List<ProductTypeApi>>("ProductType");
            Units = await Api.GetListAsync<List<UnitApi>>("Unit");
            var cross = await Api.GetListAsync<List<CrossProductRackApi>>("CrossRack");
            CrossProductRacks = await Api.GetListAsync<List<CrossProductRackApi>>("CrossRack");

            if (rackApi == null)
            {
                SelectedPersonal = Personals.FirstOrDefault();
            }
            else
            {
                SelectedPersonal = Personals.FirstOrDefault(s => s.Id == rackApi.PersonalId);
                SelectedCrosses = cross.Where(s => s.RackId == AddRackVM.Id);

                foreach (CrossProductRackApi crossProduct in SelectedCrosses)
                {
                    crossProduct.Product = SelectedRackProducts.FirstOrDefault(s => s.Id == crossProduct.ProductId);
                    crossProduct.Product.ProductType = ProductTypes.FirstOrDefault(s => s.Id == crossProduct.Product.ProductTypeId);
                    crossProduct.Product.Unit = Units.FirstOrDefault(s => s.Id == crossProduct.Product.UnitId);
                    CountAllProducts += (int)crossProduct.Product.CountInStock;
                    SignalChanged(nameof(CountAllProducts));
                }
            }
        }

        public List<CrossProductRackApi> CrossProductRacks { get; set; }

        async Task PostRack(RackApi rackApi)
        {
            var rack = await Api.PostAsync<RackApi>(rackApi, "Rack");
            foreach (CrossProductRackApi crossProduct in SelectedCrosses)
            {
                //crossProduct.Product = SelectedRackProducts.FirstOrDefault(s => s.Id == crossProduct.ProductId);
                crossProduct.DateProductPlacement = DateTime.Now;
            }
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
                        //CrossProductRacks.Remove(AddRackVM.CrossProductRacks);
                        MessageBox.Show("Удаление завершено");
                    }
                }

                else if (SelectedProduct.CrossProductRack != null)
                {
                    MessageBoxResult result = MessageBox.Show("Ошибка, данная продукция уже расположена на стеллаже! Хотите переместить его?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    Racks = new List<RackApi>();
                    GetRackToUpdate();
                    if (result == MessageBoxResult.Yes)
                    {
                        var search = CrossProductRacks.FirstOrDefault(s=> s.ProductId == SelectedProduct.Id);
                        search.Product = Product.FirstOrDefault(s => s.Id == search.ProductId);
                        if(search != null)
                        {
                            CrossProductRacks.Remove(search);
                            MessageBox.Show("Перемещение подтверждено");
                        }

                        AddRackVM.CrossProductRacks = CrossProductRacks.Where(s => s.ProductId == search.ProductId);
                        SelectedCrosses = AddRackVM.CrossProductRacks;
                        SignalChanged("SelectedCrosses");
                    }
                }
                else
                {
                    SelectedRackProduct = SelectedProduct;
                    AddRackVM.ChangedDate = DateTime.Now;
                    SelectedRackProducts.Add(SelectedProduct);
                    SignalChanged("SelectedRackProducts");
                }
            });

            SaveRack = new CustomCommand(() =>
            {
                //AddRackVM.CrossProductRacks = CrossProductRacks.FirstOrDefault(s => s.RackId == AddRackVM.Id);
                AddRackVM.Products = SelectedRackProducts;
                if (AddRackVM.Id == 0)
                {
                    AddRackVM.PersonalId = SelectedPersonal.Id;
                    //AddRackVM.CrossProductRacks = CrossProductRacks.FirstOrDefault(s => s.RackId == AddRackVM.Id);
                    //SelectedRackProduct.CountInStock = 0;
                    foreach (ProductApi product in SelectedRackProducts)
                    {
                        SelectedRackProduct.CountInStock += product.CountInStock;
                    }
                    SignalChanged("SelectedRackProduct");
                    //SignalChanged("SelectedRackProducts");
                    AddRackVM.RemainingPlaces = AddRackVM.Capacity - SelectedProduct.CountInStock / 2;
                    if (AddRackVM.RemainingPlaces < 0)
                    {
                        MessageBoxResult result = MessageBox.Show($"Не хватает мест для данного товара, его следует удалить! Количество данного товара : {SelectedProduct.CountInStock / 2}", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes)
                        {
                            SelectedRackProducts.Remove(SelectedRackProduct);
                            //CrossProductRacks.Remove(AddRackVM.CrossProductRacks);
                            MessageBox.Show("Удаление завершено");
                        }
                    }
                    else
                    {
                        var time = DateTime.Now;
                        foreach (CrossProductRackApi cross in CrossProductRacks)
                        {
                            cross.DateProductPlacement = time;
                        }
                        PostRack(AddRackVM);
                        MessageBox.Show("Добавлен новый стеллаж");
                    }
                    
                }
                else
                {
                    //var time = DateTime.Now;
                    //foreach (CrossProductRackApi cross in CrossProductRacks)
                    //{
                    //    cross.DateProductPlacement = time;
                    //}
                    AddRackVM.PersonalId = SelectedPersonal.Id;
                    //AddRackVM.CrossProductRacks = CrossProductRacks.FirstOrDefault(s => s.RackId == AddRackVM.Id);
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
                        MessageBox.Show($"Вы только что изменили стелаж : {AddRackVM.Name}");
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
