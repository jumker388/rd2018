using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web365Domain
{
    public class GoogleMessageItem : BaseModel
    {
        public string Title { get; set; }
        public string ContentText { get; set; }
        public string GoogleId { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
