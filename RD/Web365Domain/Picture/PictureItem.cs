using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web365Domain
{
    public class PictureItem : BaseModel
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public string Link { get; set; }
        public string Alt { get; set; }
        public string Summary { get; set; }
        public long? Size { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public int? TypeID { get; set; }
        public PictureTypeItem PictureType { get; set; }
    }
}
