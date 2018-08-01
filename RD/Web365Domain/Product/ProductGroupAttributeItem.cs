using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web365Domain
{
    public class ProductGroupAttributeItem : BaseModel
    {
        public string Name { get; set; }
        public string NameAscii { get; set; }
        public int? Number { get; set; }
        public int? ProductTypeID { get; set; }
        public string Detail { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }        
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        
    }
}
