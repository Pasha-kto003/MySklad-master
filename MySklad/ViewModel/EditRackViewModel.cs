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

        public List<StatusApi> Statuses { get; set; }

        private List<CrossProductRackApi> selectedCrosses { get; set; }
        public List<CrossProductRackApi> SelectedCrosses
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
            //var cross = await Api.GetListAsync<List<CrossProductRackApi>>("CrossRack");
            CrossProductRacks = await Api.GetListAsync<List<CrossProductRackApi>>("CrossRack");
            Statuses = await Api.GetListAsync<List<StatusApi>>("Status");

            if (rackApi == null)
            {
                SelectedPersonal = Personals.FirstOrDefault();
            }
            else
            {
                SelectedPersonal = Personals.FirstOrDefault(s => s.Id == rackApi.PersonalId);

                SelectedCrosses = CrossProductRacks.FindAll(s => s.RackId == AddRackVM.Id);
                rackApi.CrossProductRacks = CrossProductRacks.FindAll(s => s.RackId == AddRackVM.Id);

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
                CrossProductRackApi cross = new CrossProductRackApi();
                cross.ProductId = SelectedProduct.Id;
                cross.Rack = AddRackVM;
                cross.DateProductPlacement = DateTime.Now;
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
                else if (SelectedProduct.CrossProductRack != null)
                {
                    MessageBox.Show("Данный продукт уже есть на стеллаже");
                    return;
                }
                else if (SelectedProduct.Status == "Удален")
                {
                    MessageBoxResult result = MessageBox.Show("Этот товар находится в реестре удалений, его невозможно поместить на стеллаж!", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        SelectedRackProducts.Remove(SelectedRackProduct);
                        //CrossProductRacks.Remove(AddRackVM.CrossProductRacks);
                        MessageBox.Show("Удаление завершено");
                    }
                }

                else
                {
                    SelectedCrosses.Add(cross);
                    SelectedRackProduct = SelectedProduct;
                    AddRackVM.ChangedDate = DateTime.Now;
                    SelectedRackProducts.Add(SelectedProduct);
                    SignalChanged("SelectedRackProducts");

                }
            });

            SaveRack = new CustomCommand(() =>
            {
                SelectedPersonal.Status = Statuses.FirstOrDefault(s => s.Id == SelectedPersonal.StatusId);
                if (AddRackVM.PlacementDate == null)
                {
                    MessageBox.Show("Не введена дата создания стеллажа");
                    return;
                }

                if (SelectedPersonal.Status.Title == "Болеет")
                {
                    MessageBox.Show("Данный сотрудник заболел и не может исполнять свои обязанности");
                    return;
                }

                if (SelectedPersonal.Status.Title == "Уволен")
                {
                    MessageBox.Show("Данный сотрудник уволен и не может выполнять данную работу");
                    return;
                }

                //AddRackVM.CrossProductRacks = CrossProductRacks.FirstOrDefault(s => s.RackId == AddRackVM.Id);
                AddRackVM.Products = SelectedRackProducts;
                SelectedCrosses = AddRackVM.CrossProductRacks;
                //SelectedCrosses = CrossProductRacks.Where(s => s.RackId == AddRackVM.Id);
                if (AddRackVM.Id == 0)
                {
                    if (SelectedPersonal == null)
                    {
                        MessageBox.Show("Добавьте сотрудника на стеллаж");
                        return;
                    }
                    if (AddRackVM.Products == null)
                    {
                        MessageBox.Show("Заполните стеллаж");
                        return;
                    }
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
                        SelectedProduct.CountInStock += product.CountInStock;
                    }
                    AddRackVM.RemainingPlaces = AddRackVM.Capacity - SelectedProduct.CountInStock / 2;
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
                        if (SelectedProduct.CrossProductRack != null)
                        {
                            MessageBoxResult result = MessageBox.Show("Ошибка, данная продукция уже расположена на стеллаже! Хотите переместить его?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            Racks = new List<RackApi>();
                            GetRackToUpdate();
                            if (result == MessageBoxResult.Yes)
                            {
                                var search = CrossProductRacks.FirstOrDefault(s => s.ProductId == SelectedProduct.Id);
                                if (search != null)
                                {
                                    MessageBox.Show("Перемещение подтверждено");
                                }
                                search.RackId = AddRackVM.Id;
                                //AddRackVM.CrossProductRacks = CrossProductRacks.Where(s => s.ProductId == search.ProductId);
                                SelectedCrosses = AddRackVM.CrossProductRacks;
                                SignalChanged("SelectedCrosses");
                            }
                        }
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

