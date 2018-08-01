using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;

namespace Web365Models
{
    public class MenuProductModel
    {
        public ProductTypeItem RootType { get; set; }
        public List<ProductTypeItem> ListProductType { get; set; }
        
    }
}
