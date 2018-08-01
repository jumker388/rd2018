using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web365Domain
{
    public class MenuItem : BaseModel
    {
        public string Name { get; set; }
        public string NameAscii { get; set; }
        public string Link { get; set; }
        public string Target { get; set; }
        public string CssClass { get; set; }
        public int? DisplayOrder { get; set; }
        public int? LanguageId { get; set; }
        public int? Parent { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
