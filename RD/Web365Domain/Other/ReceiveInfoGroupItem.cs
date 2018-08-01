using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web365Domain
{
    public class ReceiveInfoGroupItem : BaseModel
    {
        public string Name { get; set; }
        public string Detail { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
