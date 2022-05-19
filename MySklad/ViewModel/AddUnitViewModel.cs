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
    public class AddUnitViewModel : BaseViewModel
    {
        public UnitApi AddUnitVM { get; set; }

        public CustomCommand SaveUnit { get; set; }
        public CustomCommand DeleteUnit { get; set; }

        public async Task CreateUnit(UnitApi unitApi)
        {
            var unit = await Api.PostAsync<UnitApi>(unitApi, "Unit");
        }

        public async Task EditApi(UnitApi unitApi)
        {
            var unit = await Api.PutAsync<UnitApi>(unitApi, "Unit");
        }

        public void CloseWindow(object obj)
        {
            Window window = obj as Window;
            window.Close();
        }

        public AddUnitViewModel(UnitApi unitApi)
        {
            

            if(unitApi == null)
            {
                AddUnitVM = new UnitApi { };
            }
            else
            {
                AddUnitVM = new UnitApi
                {
                    Id = unitApi.Id,
                    Title = unitApi.Title
                };
            }

            SaveUnit = new CustomCommand(() =>
            {
                if(AddUnitVM.Title == null)
                {
                    MessageBox.Show("Не введено название единицы измерения");
                }
                if (AddUnitVM.Id == 0)
                {
                    CreateUnit(AddUnitVM);
                }
                else
                {
                    EditApi(AddUnitVM);
                }
                foreach (Window window in Application.Current.Windows)
                {
                    if(window.DataContext == this)
                    {
                        CloseWindow(window);
                    }
                }
                SignalChanged("Units");
            });
        }
    }
}
