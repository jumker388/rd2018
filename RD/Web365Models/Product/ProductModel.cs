using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;

namespace Web365Models
{
    public class ProductModel
    {
        public List<ProductItem> ListProduct { get; set; }
        public int Total { get; set; }        
    }
}
