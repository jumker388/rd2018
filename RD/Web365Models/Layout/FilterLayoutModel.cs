using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;

namespace Web365Models
{
    public class FilterLayoutModel
    {
        public List<ProductTypeItem> ListProductType { get; set; }
        public List<ProductFilterItem> ListProductFilter { get; set; }
        
    }
}
