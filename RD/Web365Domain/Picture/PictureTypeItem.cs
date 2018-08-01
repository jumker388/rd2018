using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web365Domain
{
    public class PictureTypeItem : BaseModel
    {
        public string Name { get; set; }
        public string NameAscii { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public string SEOTitle { get; set; }
        public string SEODescription { get; set; }
        public string SEOKeyword { get; set; }
        public int? PictureID { get; set; }
        public string Detail { get; set; }
        public int? Number { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public int? Parent { get; set; }
        public PictureItem Picture { get; set; }
    }
}
