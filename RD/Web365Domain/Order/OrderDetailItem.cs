using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web365Domain
{
    public class OrderDetailItem : BaseModel
    {
        public int? ProductID { get; set; }
        public int? ProductVariantID { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public int? OrderID { get; set; }
        public ProductItem Product { get; set; }
        public ProductVariantItem ProductVariant { get; set; }
    }
}
