using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Business.Front_End.IRepository;
using Web365Domain;
using Web365Utility;

namespace Web365Business.Front_End.Repository
{
    public class OrderRepositoryFE : BaseFE, IOrderRepositoryFE
    {
        public bool AddOrder(tblOrder order, tblOrderDetail orderDetail, tblOrder_Shipping orderShipping)
        {
            try
            {
                web365db.tblOrder.Add(order);

                orderShipping.OrderID = order.ID;

                web365db.tblOrder_Shipping.Add(orderShipping);

                orderDetail.OrderID = order.ID;

                web365db.tblOrderDetail.Add(orderDetail);

                var result = web365db.SaveChanges();

                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<OrderItem> GetListOrderByCustomer(int customerId)
        {
            var query = web365db.tblOrder.Where(o => o.CustomerID == customerId).Select(o => new OrderItem()
            {
                ID = o.ID,
                DateCreated = o.DateCreated,
                TotalCost = o.TotalCost,
                OrderStatus = new OrderStatusItem()
                {
                    Name = o.tblOrder_Status.Name
                }
            });

            return query.ToList();
        }
    }
}
