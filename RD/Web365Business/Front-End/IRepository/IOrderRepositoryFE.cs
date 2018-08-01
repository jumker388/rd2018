using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Domain;

namespace Web365Business.Front_End.IRepository
{
    public interface IOrderRepositoryFE
    {
        bool AddOrder(tblOrder order, tblOrderDetail orderDetail, tblOrder_Shipping orderShipping);
        List<OrderItem> GetListOrderByCustomer(int customerId);
    }
}
