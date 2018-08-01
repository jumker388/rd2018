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
    public class ReceiveInfoGroupRepository : BaseBE<tblReceiveInfoGroup>, IReceiveInfoGroupRepository
    {
        /// <summary>
        /// function get all data tblTypeProduct
        /// </summary>
        /// <returns></returns>
        public List<ReceiveInfoGroupItem> GetList(out int total, string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool isDelete = false)
        {
            var query = from p in web365db.tblReceiveInfoGroup
                        where p.Name.ToLower().Contains(name) && p.IsDeleted == isDelete
                        orderby p.ID descending
                        select new ReceiveInfoGroupItem()
                        {
                            ID = p.ID,
                            Name = p.Name,
                            IsShow = p.IsShow,
                            DateCreated = p.DateCreated
                        };

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Skip(currentRecord).Take(numberRecord).ToList();
        }

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            var query = from p in web365db.tblReceiveInfoGroup
                        where p.IsShow == isShow && p.IsDeleted == isDelete
                        orderby p.ID descending
                        select new ReceiveInfoGroupItem()
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
            var result = GetById<tblReceiveInfoGroup>(id);

            return (T)(object)new ReceiveInfoGroupItem()
                        {
                            ID = result.ID,
                            Name = result.Name,
                            DateCreated = result.DateCreated,
                            Detail = result.Detail,
                            IsShow = result.IsShow
                        };
        }        

        #region Insert, Edit, Delete, Save

        public void Show(int id)
        {
            var layoutContent = web365db.tblReceiveInfoGroup.SingleOrDefault(p => p.ID == id);
            layoutContent.IsShow = true;
            web365db.Entry(layoutContent).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var layoutContent = web365db.tblReceiveInfoGroup.SingleOrDefault(p => p.ID == id);
            layoutContent.IsShow = false;
            web365db.Entry(layoutContent).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        #endregion

        #region Check
        public bool NameExist(int id, string name)
        {
            var query = web365db.tblReceiveInfoGroup.Count(c => c.Name.ToLower() == name.ToLower() && c.ID != id);

            return query > 0;
        }
        #endregion
    }
}
