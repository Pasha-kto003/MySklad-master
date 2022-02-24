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
    public class AddWriteOffRegisterViewModel : BaseViewModel
    {
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

        private List<WriteOffRegisterApi> writeOffRegisters { get; set; }
        public List<WriteOffRegisterApi> WriteOffRegisters
        {
            get => writeOffRegisters;
            set
            {
                writeOffRegisters = value;
                SignalChanged();
            }
        }

        private ProductApi selectedRegister { get; set; }
        public ProductApi SelectedRegister
        {
            get => selectedRegister;
            set
            {
                selectedRegister = value;
                SignalChanged();
            }
        }

        public void CloseWindow(object obj)
        {
            Window window = obj as Window;
            window.Close();
        }

        private async Task GetRegisters()
        {
            Registers = await Api.GetListAsync<List<WriteOffRegisterApi>>("WriteOffRegister");
            Products = await Api.GetListAsync<List<ProductApi>>("Product");
            foreach (WriteOffRegisterApi registerApi in Registers)
            {
                registerApi.Product = Products.First(s => s.Id == registerApi.ProductId);
            }
        }

        async Task GetRegister(WriteOffRegisterApi registerApi)
        {
            Products = await Api.GetListAsync<List<ProductApi>>("Product");             
            if (registerApi == null)               
            {
                SelectedProduct = Products.FirstOrDefault();           
            }               
            else         
            {
                SelectedProduct = Products.FirstOrDefault(s => s.Id == registerApi.ProductId);         
            }           
        }

        async Task GetProducts()
        {
            var products = await Api.GetListAsync<List<ProductApi>>("Product");
        }

        async Task PostRegister(WriteOffRegisterApi writeOffRegister)
        {
            var register = await Api.PostAsync<WriteOffRegisterApi>(writeOffRegister, "WriteOffRegister");
        }

        public async Task EditRegister(WriteOffRegisterApi writeOffRegister)
        {
            var register = await Api.PutAsync<WriteOffRegisterApi>(writeOffRegister, "WriteOffRegister");
        }

        public async Task EditProduct(ProductApi productApi)
        {
            var product = await Api.PutAsync<ProductApi>(productApi, "Product");
        }

        public WriteOffRegisterApi AddRegisterVM { get; set; }

        public CustomCommand SaveRegister { get; set; }



        public AddWriteOffRegisterViewModel(WriteOffRegisterApi writeOffRegister)
        {
            WriteOffRegisters = new List<WriteOffRegisterApi>();
            Products = new List<ProductApi>();


            GetProducts();
            

            if (writeOffRegister == null)
            {
                AddRegisterVM = new WriteOffRegisterApi { Title = "Название", ReasonDelete = "Причина" };
            }
            else
            {
                AddRegisterVM = new WriteOffRegisterApi
                {
                    Id = writeOffRegister.Id,
                    ReasonDelete = writeOffRegister.ReasonDelete,
                    DeletedAt = writeOffRegister.DeletedAt,
                    Title = writeOffRegister.Title,
                    ProductId = writeOffRegister.ProductId
                };
            }

            GetRegister(AddRegisterVM);

            

            SaveRegister = new CustomCommand(() =>
            {
                if(AddRegisterVM.Id == 0)
                {
                    AddRegisterVM.ProductId = SelectedProduct.Id;
                    PostRegister(AddRegisterVM);
                }
                else
                {
                    AddRegisterVM.ProductId = SelectedProduct.Id;
                    EditRegister(AddRegisterVM);
                }

                if (AddRegisterVM.ProductId == SelectedProduct.Id)
                {
                    SelectedProduct.Status = "Удален";
                    EditProduct(SelectedProduct);
                    SignalChanged("SelectedProduct");
                    MessageBox.Show("Выполнен");
                }


                foreach (Window window in Application.Current.Windows)
                {
                    if (window.DataContext == this)
                    {
                        CloseWindow(window);
                    }
                }
                SignalChanged("WriteOffRegisters");
            });
        }
    }
}
