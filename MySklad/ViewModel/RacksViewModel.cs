using ModelApi;
using MySklad.Core;
using MySklad.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySklad.ViewModel
{
    public class RacksViewModel : BaseViewModel
    {
        private List<RackApi> racks { get; set; }
        public List<RackApi> Racks 
        {
            get => racks;
            set
            {
                racks = value;
                SignalChanged();
            }
        }

        private RackApi selectedRack { get; set; }
        public RackApi SelectedRack
        {
            get => selectedRack;
            set
            {
                selectedRack = value;
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

        private async Task GetRacks()
        {
            Personals = await Api.GetListAsync<List<PersonalApi>>("Personal");
            Racks = await Api.GetListAsync<List<RackApi>>("Rack");
            foreach(RackApi rackApi in Racks)
            {
                rackApi.Personal = Personals.First(s => s.Id == rackApi.PersonalId);
            }
        }

        public CustomCommand EditRack { get; set; }
        public CustomCommand CreateRack { get; set; }

        public RacksViewModel()
        {
            Racks = new List<RackApi>();
            Personals = new List<PersonalApi>();
            GetRacks();

            CreateRack = new CustomCommand(() =>
            {
                EditRackView editRack = new EditRackView();
                editRack.ShowDialog();
                GetRacks();
            });

            EditRack = new CustomCommand(() =>
            {
                if (SelectedRack == null) return;
                EditRackView editRack = new EditRackView(SelectedRack);
                editRack.ShowDialog();
                GetRacks();
            });
        }
    }
}
