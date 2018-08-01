using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;

namespace Web365Models
{
    public class HeaderModel
    {
        public LayoutContentItem Content { get; set; }
        public List<MenuItem> ListLinkTop { get; set; }
        public List<MenuItem> ListLinkCustomer { get; set; }
        
    }
}
