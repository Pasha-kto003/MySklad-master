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
    public class RackViewModel : BaseViewModel
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

        private List<ProductApi> products { get; set; }
        public List<ProductApi> Products
        {
            get => products;
            set
            {
                products = value;
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

        public CrossProductRackApi CrossProductRack { get; set; }

        public CustomCommand CreateRack { get; set; }
        public CustomCommand EditRack { get; set; }

        private async Task GetRacks()
        {
            Products = await Api.GetListAsync<List<ProductApi>>("Product");
            Racks = await Api.GetListAsync<List<RackApi>>("Rack");
            Personals = await Api.GetListAsync<List<PersonalApi>>("Personal");

            foreach (RackApi rackApi in Racks)
            {
                rackApi.Personal = Personals.First(s => s.Id == rackApi.PersonalId);
            }
        }

        public RackViewModel()
        {
            //GetRacks();
            Products = new List<ProductApi>();
            Personals = new List<PersonalApi>();
            Racks = new List<RackApi>();
            GetRacks();

            CreateRack = new CustomCommand(() =>
            {
                AddRack addRack = new AddRack();
                addRack.ShowDialog();
                GetRacks();
            });

            EditRack = new CustomCommand(() =>
            {
                if (SelectedRack == null) return;
                AddRack addRack = new AddRack();
                addRack.ShowDialog();
                GetRacks();
            });
        }
    }
}
