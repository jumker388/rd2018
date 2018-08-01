using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web365Domain
{
    public class ProductVariantItem : BaseModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ProductID { get; set; }
        public decimal Price { get; set; }
        public decimal VAT { get; set; }
        public bool IsOutOfStock { get; set; }
        public int Quantity { get; set; }
        public int? DisplayOrder { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
