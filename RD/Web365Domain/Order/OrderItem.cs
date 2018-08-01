using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web365Domain
{
    public class OrderItem : BaseModel
    {
        public int? OrderStatusID { get; set; }
        public decimal? TotalCost { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string Note { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int? CustomerID { get; set; }
        public bool? IsViewed { get; set; }
        public bool? IsDeleted { get; set; }
        public OrderStatusItem OrderStatus { get; set; }
        public OrderShippingItem OrderShipping { get; set; }
        public List<OrderDetailItem> ListOrderDetail { get; set; }        
    }
}
