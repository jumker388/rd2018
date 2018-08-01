using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    public class ContactRepository : BaseBE<tblContact>, IContactRepository
    {
        /// <summary>
        /// function get all data tblTypeProduct
        /// </summary>
        /// <returns></returns>
        public List<ContactItem> GetList(out int total, string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool isDelete = false)
        {
            var query = from p in web365db.tblContact
                        where p.Name.ToLower().Contains(name) && p.IsDeleted == isDelete
                        orderby p.ID descending
                        select new ContactItem()
                        {
                            ID = p.ID,
                            Name = p.Name,
                            IsViewed = p.IsViewed
                        };

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Skip(currentRecord).Take(numberRecord).ToList();
        }

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            var query = from p in web365db.tblContact
                        where p.IsViewed == isShow && p.IsDeleted == isDelete
                        orderby p.ID descending
                        select new ContactItem()
                        {
                            ID = p.ID,
                            Name = p.Name
                        };

            return (T)(object)query.ToList();
        }        

        /// <summary>
        /// get product type item
        /// </summary>
        /// <param name="id">id of product type</param>
        /// <returns></returns>
        public T GetItemById<T>(int id)
        {
            var result = GetById<tblContact>(id);

            return (T)(object)new ContactItem()
                        {
                            ID = result.ID,
                            Name = result.Name,
                            DateCreated = result.DateCreated,
                            DateUpdated = result.DateUpdated,
                            IsViewed = result.IsViewed
                        };
        }        

        #region Insert, Edit, Delete, Save        

        public void Show(int id)
        {
            var Contact = web365db.tblContact.SingleOrDefault(p => p.ID == id);
            Contact.IsViewed = true;
            web365db.Entry(Contact).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var Contact = web365db.tblContact.SingleOrDefault(p => p.ID == id);
            Contact.IsViewed = false;
            web365db.Entry(Contact).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        #endregion

        #region Check
        public bool NameExist(int id, string name)
        {
            var query = web365db.tblContact.Count(c => c.Name.ToLower() == name.ToLower() && c.ID != id);

            return query > 0;
        }
        #endregion
    }
}
