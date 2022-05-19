using Microsoft.Win32;
using ModelApi;
using MySklad.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MySklad.ViewModel
{
    public class AddPersonalViewModel : BaseViewModel
    {
        public PersonalApi AddPersonalVM { get; set; }

        public CustomCommand SavePersonal { get; set; }
        public CustomCommand SelectImage { get; set; } //команда для выбора изображения
        public CustomCommand RemovePersonal { get; set; }

        private BitmapImage imagePersonal;
        public BitmapImage ImagePersonal
        {
            get => imagePersonal;
            set
            {
                imagePersonal = value;
                SignalChanged();
            }
        }

        private List<StatusApi> statuses { get; set; }
        public List<StatusApi> Statuses
        {
            get => statuses;
            set
            {
                statuses = value;
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

        private StatusApi selectedStatus { get; set; }
        public StatusApi SelectedStatus
        {
            get => selectedStatus;
            set
            {
                selectedStatus = value;
                SignalChanged();
            }
        }

        async Task GetStatuses()
        {
            var status = await Api.GetListAsync<StatusApi>("Status");
        }
        async Task PostPersonal(PersonalApi personalApi)
        {
            var personal = await Api.PostAsync<PersonalApi>(personalApi, "Personal");
        }
        async Task EditPersonal(PersonalApi personalApi)
        {
            var personal = await Api.PutAsync<PersonalApi>(personalApi, "Personal");
        }

        

        async Task GetPersonals(PersonalApi personalApi)
        {
            Statuses = await Api.GetListAsync<List<StatusApi>>("Status");

            if (personalApi == null)
            {
                SelectedStatus = Statuses.FirstOrDefault();
            }
            else
            {
                SelectedStatus = Statuses.FirstOrDefault(s => s.Id == personalApi.StatusId);
            }
        }

        public AddPersonalViewModel(PersonalApi personalApi)
        {
            GetStatuses();

            if(personalApi == null)
            {
                AddPersonalVM = new PersonalApi { FirstName = "Имя", LastName = "Фамилия", Patronimyc = "Отчество", Rating = 100, Image = @"C:\Users\User\source\repos\MySklad-master\MySklad\materials\ron2.png" };
            }
            else
            {
                AddPersonalVM = new PersonalApi
                {
                    Id = personalApi.Id,
                    FirstName = personalApi.FirstName,
                    LastName = personalApi.LastName,
                    Patronimyc = personalApi.Patronimyc,
                    Rating = personalApi.Rating,
                    Phone = personalApi.Phone,
                    DateStartWork = personalApi.DateStartWork,
                    DateEndWork = personalApi.DateEndWork,
                    Image = personalApi.Image,
                    StatusId = personalApi.StatusId
                };
                if(ImagePersonal == null)
                {
                    ImagePersonal = GetImageFromPath(Environment.CurrentDirectory + "//" + AddPersonalVM.Image);
                }
                else
                    ImagePersonal = GetImageFromPath(Environment.CurrentDirectory + "//" + AddPersonalVM.Image);
                
            }

            GetPersonals(AddPersonalVM);         
            SelectImage = new CustomCommand(() =>
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if(ofd.ShowDialog() == true)
                {
                    try
                    {
                        var info = new FileInfo(ofd.FileName);
                        ImagePersonal = GetImageFromPath(ofd.FileName);
                        AddPersonalVM.Image = $"/materials/{info.Name}";

                        var newPath = Environment.CurrentDirectory + AddPersonalVM.Image;
                        File.Copy(ofd.FileName, newPath);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            });


            SavePersonal = new CustomCommand(() =>
            {
                try
                {
                    if(AddPersonalVM.LastName == null || AddPersonalVM.Patronimyc == null || AddPersonalVM.FirstName == null)
                    {
                        MessageBox.Show("Введенно неправильно ФИО");
                        return;
                    }

                    if(AddPersonalVM.DateStartWork > AddPersonalVM.DateEndWork || AddPersonalVM.DateStartWork == null || AddPersonalVM.DateEndWork == null)
                    {
                        MessageBox.Show("Замечена ошибка в датах сотрудника");
                        return;
                    }

                    if(AddPersonalVM.Rating == null || AddPersonalVM.Rating < 0)
                    {
                        MessageBox.Show("Введен неверный рейтинг");
                        return;
                    }

                    if(AddPersonalVM.Phone == null)
                    {
                        MessageBox.Show("Не введен номер телефона");
                        return;
                    }

                    if(AddPersonalVM.Id == 0)
                    {
                        if(SelectedStatus == null)
                        {
                            MessageBox.Show("Не введен статус сотрудника");
                            return;
                        }
                        AddPersonalVM.StatusId = SelectedStatus.Id;
                        PostPersonal(AddPersonalVM);
                        MessageBox.Show($"Новый сотрудник {AddPersonalVM.FirstName} добавлен на склад");
                    }
                    else
                    {
                        AddPersonalVM.StatusId = SelectedStatus.Id;
                        EditPersonal(AddPersonalVM);
                    }
                    if (AddPersonalVM.DateEndWork < AddPersonalVM.DateStartWork)
                    {
                        MessageBox.Show("Ошибка, таких дат работы не может быть");
                        return;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.DataContext == this)
                    {
                        CloseModalWindow(window);
                    }
                }

                SignalChanged("Personals");
            });
        }

        

        private BitmapImage GetImageFromPath(string url)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.UriSource = new Uri(url, UriKind.Absolute);
            img.EndInit();
            return img;
        }

        public void CloseModalWindow(object obj)
        {
            Window window = obj as Window;
            window.Close();
        }
    }
}
