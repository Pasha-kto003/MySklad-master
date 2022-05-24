using ModelApi;
using MySklad.Core;
using MySklad.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MySklad.ViewModel
{
    public class CatalogViewModel : BaseViewModel
    {
        public CustomCommand AddUnit { get; set; }
        public CustomCommand EditUnit { get; set; }
        public CustomCommand DeleteUnit { get; set; }

        public CustomCommand AddType { get; set; }
        public CustomCommand EditType { get; set; }
        public CustomCommand DeleteProductType { get; set; }

        public CustomCommand AddStatus { get; set; }
        public CustomCommand EditStatus { get; set; }
        public CustomCommand DeleteStatus { get; set; }

        private ObservableCollection<UnitApi> units { get; set; }
        public ObservableCollection<UnitApi> Units
        {
            get { return units; }
            set
            {
                units = value;
                SignalChanged();
            }
        }

        private UnitApi selectedUnit { get; set; }
        public UnitApi SelectedUnit
        {
            get { return selectedUnit; }

            set
            {
                selectedUnit = value;
                SignalChanged();
            }
        }

        public List<ProductApi> Products { get; set; }

        public async Task UnitDelete(UnitApi unit)
        {
            var unitdelete = await Api.DeleteAsync<UnitApi>(unit, "Unit");
        }

        private List<ProductTypeApi> productTypes { get; set; } 

        public List<ProductTypeApi> ProductTypes
        {
            get { return productTypes; }
            set
            {
                productTypes = value;
                SignalChanged();
            }
        }
        private ProductTypeApi selectedProductType { get; set; }
        public ProductTypeApi SelectedProductType
        {
            get { return selectedProductType; }
            set
            {
                selectedProductType = value;
                SignalChanged();
            }
        }

        public async Task TypeDelete(ProductTypeApi type)
        {
            var typedelete = await Api.DeleteAsync<ProductTypeApi>(type, "ProductType");
        }

        private List<StatusApi> statuses { get; set; }

        public List<StatusApi> Statuses
        {
            get { return statuses; }
            set
            {
                statuses = value;
                SignalChanged();
            }
        }
        private StatusApi selectedStatus { get; set; }
        public StatusApi SelectedStatus
        {
            get { return selectedStatus; }
            set 
            {
                selectedStatus = value;
                SignalChanged();
            }
        }

        public async Task GetProducts()
        {
            Products = await Api.GetListAsync<List<ProductApi>>("Product");
        }

        public async Task StatusDelete(StatusApi status)
        {
            var statusdelete = await Api.DeleteAsync<StatusApi>(status, "Status");
        }

        public CatalogViewModel()
        {
            Units = new ObservableCollection<UnitApi>();
            ProductTypes = new List<ProductTypeApi>();
            Statuses = new List<StatusApi>();
            UnitsApi();
            ProductTypesApi();
            StatusesApi();
            GetProducts();

            AddUnit = new CustomCommand(() =>
            {
                AddUnitView addUnitView = new AddUnitView();
                addUnitView.ShowDialog();
                UnitsApi();
            });

            EditUnit = new CustomCommand(() =>
            {
                if (SelectedUnit == null) return;
                AddUnitView addUnit = new AddUnitView(SelectedUnit);
                addUnit.ShowDialog();
                UnitsApi();
            });

            DeleteUnit = new CustomCommand(() =>
            {
                if (SelectedUnit == null) return;
                var search = Products.Where(s=> s.UnitId == SelectedUnit.Id);
                if(search.Count() != 0)
                {
                    MessageBox.Show("Единица измерения используется в продукции. Ее нельзя удалить");
                    return;
                }
                MessageBoxResult result = MessageBox.Show($"Вы точно хотите удалить {SelectedUnit.Title}", "Подтвердите действие", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes)
                {
                    UnitDelete(SelectedUnit);
                    UnitsApi();
                }
            });

            AddStatus = new CustomCommand(() =>
            {
                AddStatusView addStatus = new AddStatusView();
                addStatus.ShowDialog();
                StatusesApi();
            });

            EditStatus = new CustomCommand(() =>
            {
                if (SelectedStatus == null) return;
                AddStatusView addStatus = new AddStatusView(SelectedStatus);
                addStatus.ShowDialog();
                StatusesApi();
            });

            //DeleteStatus = new CustomCommand(() =>
            //{
            //    if (SelectedStatus == null) return;
            //    MessageBoxResult result = MessageBox.Show($"Вы точно хотите удалить {SelectedStatus.Title}", "Подтвердите действие", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //    if (result == MessageBoxResult.Yes)
            //    {
            //        StatusDelete(SelectedStatus);
            //        StatusesApi();
            //    }
            //});

            AddType = new CustomCommand(() =>
            {
                AddProductTypeView typeView = new AddProductTypeView();
                typeView.ShowDialog();
                ProductTypesApi();
            });

            EditType = new CustomCommand(() =>
            {
                if (SelectedProductType == null) return;
                AddProductTypeView typeView = new AddProductTypeView(SelectedProductType);
                typeView.ShowDialog();
                ProductTypesApi();
            });

            DeleteProductType = new CustomCommand(() =>
            {
                if (SelectedProductType == null) return;
                var search = Products.Where(s => s.ProductTypeId == SelectedProductType.Id);
                if (search.Count() != 0)
                {
                    MessageBox.Show("Тип продукции используется в продукции. Его нельзя удалить");
                    return;
                }
                MessageBoxResult result = MessageBox.Show($"Вы точно хотите удалить {SelectedProductType.Title}", "Подтвердите действие", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    TypeDelete(SelectedProductType);
                    ProductTypesApi();
                }
                
            });
        }

        async Task UnitsApi()
        {
            var units = await Api.GetListAsync<ObservableCollection<UnitApi>>("Unit");
            Units = (ObservableCollection<UnitApi>)units;
        }

        async Task ProductTypesApi()
        {
            var types = await Api.GetListAsync<List<ProductTypeApi>>("ProductType");
            ProductTypes = (List<ProductTypeApi>)types;
        }

        async Task StatusesApi()
        {
            var status = await Api.GetListAsync<List<StatusApi>>("Status");
            Statuses = (List<StatusApi>)status;
        }
    }
}
