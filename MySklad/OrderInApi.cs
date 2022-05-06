using ModelApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySklad
{
    partial class OrderIn
    {
        public int ID { get; set; }
        public SupplierApi Supplier { get; set; }
        public int SupplierId { get; set; }
        public DateTime DateOrderIn { get; set; }
        public string ColorForXaml { get; set; } = "#FFFFFF";
        public string Title { get; set; }
    }
}
