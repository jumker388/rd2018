using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;

namespace Web365Models
{
    public class CartModel
    {
        public LayoutContentItem Content { get; set; }
        public List<OrderItemModel> ListOrder { get; set; }
        public LayoutContentItem ContentOrderSuccess { get; set; }
    }
}
