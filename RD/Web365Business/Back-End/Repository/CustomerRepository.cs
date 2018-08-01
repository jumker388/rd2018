using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Domain;
using Web365Utility;


namespace Web365Business.Back_End.Repository
{
    public class CustomerRepository : BaseBE<tblCustomer>, ICustomerRepository
    {

        /// <summary>
        /// function get all data tblCustomer
        /// </summary>
        /// <returns></returns>
        public List<CustomerItem> GetList(out int total, string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool isDelete = false)
        {
            var query = from p in web365db.tblCustomer
                        where p.UserName.ToLower().Contains(name) && p.IsDeleted == isDelete
                        select p;

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Select(p => new CustomerItem()
            {
                ID = p.ID,
                UserName = p.UserName,
                Email = p.Email,
                DateCreated = p.DateCreated,
                IsActive = p.IsActive.HasValue && p.IsActive.Value
            }).Skip(currentRecord).Take(numberRecord).ToList();
        }

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            var query = from p in web365db.tblCustomer
                        where p.IsActive == isShow && p.IsDeleted == isDelete
                        orderby p.ID descending
                        select new CustomerItem()
                        {
                            ID = p.ID,
                            UserName = p.UserName,
                        };

            return (T)(object)query.ToList();
        }

        public T GetItemById<T>(int id)
        {
            var query = from p in web365db.tblCustomer
                        where p.ID == id
                        orderby p.ID descending
                        select new CustomerItem()
                        {
                            ID = p.ID,
                            UserName = p.UserName,                            
                            BirthDay = p.BirthDay,
                            FirstName = p.FirstName,
                            LastName = p.LastName,
                            Phone = p.Phone,
                            Email = p.Email,
                            Address = p.Address,
                            Gender = p.Gender,
                            Token = p.Token,
                            DateExpiredToken = p.DateExpiredToken,
                            DateCreated = p.DateCreated,
                            DateUpdated = p.DateExpiredToken,
                            IsActive = p.IsActive.HasValue && p.IsActive.Value
                        };
            return (T)(object)query.FirstOrDefault();
        }

        public void Show(int id)
        {
            var Customer = web365db.tblCustomer.SingleOrDefault(p => p.ID == id);
            Customer.IsActive = true;
            web365db.Entry(Customer).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var Customer = web365db.tblCustomer.SingleOrDefault(p => p.ID == id);
            Customer.IsActive = false;
            web365db.Entry(Customer).State = EntityState.Modified;
            web365db.SaveChanges();
        }


        public bool NameExist(int id, string name)
        {
            var query = web365db.tblCustomer.Count(c => c.UserName.ToLower() == name.ToLower() && c.ID != id);

            return query > 0;
        }
    }
}
