using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Domain;

namespace Web365Business.Back_End.IRepository
{
    public interface IOrderRepository : IBaseRepository
    {
        List<OrderItem> GetList(out int total, int? id, string customerName, string email, string phone, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool? isViewed, bool isDelete = false);
    }
}
