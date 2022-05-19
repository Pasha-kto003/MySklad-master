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
    public class AddStatusViewModel : BaseViewModel
    {
        public StatusApi AddStatusVM { get; set; }

        public CustomCommand SaveStatus { get; set; }
        public CustomCommand DeleteStatus { get; set; }

        

        public void CloseWindow(object obj)
        {
            Window window = obj as Window;
            window.Close();
        }

        public async Task CreateStatus(StatusApi statusApi)
        {
            var status = await Api.PostAsync<StatusApi>(statusApi, "Status");
        }

        public async Task EditApi(StatusApi statusApi)
        {
            var status = await Api.PutAsync<StatusApi>(statusApi, "Status");
        }

        public AddStatusViewModel(StatusApi statusApi)
        {
            if (statusApi == null)
            {
                AddStatusVM = new StatusApi { };
            }
            else
            {
                AddStatusVM = new StatusApi
                {
                    Id = statusApi.Id,
                    Title = statusApi.Title
                };
            }

            SaveStatus = new CustomCommand(() =>
            {
                if(AddStatusVM.Title == null)
                {
                    MessageBox.Show("Не введен статус");
                    return;
                }
                if (AddStatusVM.Id == 0)
                {
                    CreateStatus(AddStatusVM);
                }
                else
                {
                    EditApi(AddStatusVM);
                }
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.DataContext == this)
                    {
                        CloseWindow(window);
                    }
                }
                SignalChanged("Statuses");
            });
            
        }
    }
}
