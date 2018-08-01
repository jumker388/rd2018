using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Domain;
using Web365Utility;

namespace Web365Business.Back_End.Repository
{
    /// <summary>
    ///  05-01-2013
    /// </summary>
    public class OrderRepository : BaseBE<tblOrder>, IOrderRepository
    {
        /// <summary>
        /// function get all data tblTypeProduct
        /// </summary>
        /// <returns></returns>
        public List<OrderItem> GetList(out int total,
            int? id,
            string customerName, 
            string email,
            string phone,
            int currentRecord, 
            int numberRecord, 
            string propertyNameSort,
            bool descending,
            bool? isViewed,
            bool isDelete = false)
        {
            var query = from p in web365db.tblOrder
                        where p.CustomerName.ToLower().Contains(customerName)
                        && p.Email.ToLower().Contains(email)
                        && p.Phone.ToLower().Contains(phone) 
                        && p.IsDeleted == isDelete                        
                        select p;

            if (id.HasValue)
            {
                query = query.Where(p => p.ID == id);
            }

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Select(p => new OrderItem()
            {
                ID = p.ID,
                CustomerName = p.CustomerName,
                Email = p.Email,
                Phone = p.Phone,
                DateCreated = p.DateCreated,
                TotalCost = p.TotalCost,
                IsViewed = p.IsViewed
            }).Skip(currentRecord).Take(numberRecord).ToList();
        }

        /// <summary>
        /// get product type item
        /// </summary>
        /// <param name="id">id of product type</param>
        /// <returns></returns>
        public T GetItemById<T>(int id)
        {
            var result = GetById<tblOrder>(id);

            return (T)(object)new OrderItem()
            {
                ID = result.ID,
                CustomerName = result.CustomerName,
                Email = result.Email,
                Phone = result.Phone,
                DateCreated = result.DateCreated,
                DateUpdated = result.DateUpdated,
                CreatedBy = result.CreatedBy,
                UpdatedBy = result.UpdatedBy,
                TotalCost = result.TotalCost,
                Address = result.Address,
                CustomerID = result.CustomerID,
                Note = result.Note,
                IsViewed = result.IsViewed,
                OrderStatusID = result.OrderStatusID,
                OrderShipping = result.tblOrder_Shipping.Select(p =>new OrderShippingItem(){
                    Address = p.Address,
                    CustomerName = p.CustomerName,
                    Email = p.Email,
                    Phone = p.Phone
                }).FirstOrDefault(),
                ListOrderDetail = result.tblOrderDetail.Select(p => new OrderDetailItem() { 
                    Product = new ProductItem(){
                        ID = p.tblProduct.ID,
                        Name = p.tblProduct.Name
                    },                    
                    Price = p.Price,
                    Quantity = p.Quantity
                }).ToList()
            };
        }

        #region Insert, Edit, Delete, Save

        public void Show(int id)
        {
            var order = web365db.tblOrder.SingleOrDefault(p => p.ID == id);
            order.IsViewed = true;
            web365db.Entry(order).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var order = web365db.tblOrder.SingleOrDefault(p => p.ID == id);
            order.IsViewed = false;
            web365db.Entry(order).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        #endregion

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            throw new NotImplementedException();
        }

        public bool NameExist(int id, string name)
        {
            throw new NotImplementedException();
        }
    }
}
