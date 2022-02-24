using ModelApi;
using MySklad.Core;
using MySklad.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MySklad.ViewModel
{
    public class WriteOffRegisterViewModel : BaseViewModel
    {
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

        private List<WriteOffRegisterApi> registers { get; set; }
        public List<WriteOffRegisterApi> Registers
        {
            get => registers;
            set
            {
                registers = value;
                SignalChanged();
            }
        }


        private WriteOffRegisterApi selectedRegister { get; set; }
        public WriteOffRegisterApi SelectedRegister
        {
            get => selectedRegister;
            set
            {
                selectedRegister = value;
                SignalChanged();
            }
        }

        public CustomCommand AddRegister { get; set; }
        public CustomCommand EditRegister { get; set; }

        public WriteOffRegisterViewModel()
        {

            Products = new List<ProductApi>();
            Registers = new List<WriteOffRegisterApi>();
            GetRegisters();

            AddRegister = new CustomCommand(() =>
            {
                AddWriteOffRegisterView addWriteOff = new AddWriteOffRegisterView();
                addWriteOff.ShowDialog();
                GetRegisters();
            });

            EditRegister = new CustomCommand(() =>
            {
                if (SelectedRegister == null) return;
                AddWriteOffRegisterView addWriteOff = new AddWriteOffRegisterView(SelectedRegister);
                addWriteOff.ShowDialog();
                GetRegisters();
            });

           
            SignalChanged("WriteOffRegisters");
        }

        private async Task GetRegisters()
        {
            Registers = await Api.GetListAsync<List<WriteOffRegisterApi>>("WriteOffRegister");
            Products = await Api.GetListAsync<List<ProductApi>>("Product");
            foreach (WriteOffRegisterApi registerApi in Registers)
            {
                registerApi.Product = Products.First(s => s.Id == registerApi.ProductId);
            }
            SignalChanged("WriteOffRegisters");
        }
    }
}
