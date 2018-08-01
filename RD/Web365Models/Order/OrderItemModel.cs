using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;

namespace Web365Models
{
    public class OrderItemModel
    {
        public int ProductID { get; set; }
        public int ProductVariantID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Picture { get; set; }
        public string ProductName { get; set; }
        public string ProductVariantName { get; set; }
    }
}
