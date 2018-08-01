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
    public class ProductLabelRepository : BaseBE<tblProductLabel>, IProductLabelRepository
    {
        /// <summary>
        /// function get all data tblProductLabel
        /// </summary>
        /// <returns></returns>
        public List<ProductLabelItem> GetList(out int total, string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool isDelete = false)
        {
            var query = from p in web365db.tblProductLabel
                        where p.Name.ToLower().Contains(name) && p.IsDeleted == isDelete
                        orderby p.ID descending
                        select new ProductLabelItem()
                        {
                            ID = p.ID,
                            Name = p.Name,
                            PictureID = p.PictureID,
                            Number = p.Number,
                            IsShow = p.IsShow
                        };

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Skip(currentRecord).Take(numberRecord).ToList();
        }

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            var query = from p in web365db.tblProductLabel
                        where p.IsShow == isShow && p.IsDeleted == isDelete
                        orderby p.ID descending
                        select new ProductLabelItem()
                        {
                            ID = p.ID,
                            Name = p.Name,
                            Number = p.Number,
                            IsShow = p.IsShow
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
            var query = from p in web365db.tblProductLabel
                        where p.ID == id
                        orderby p.ID descending
                        select new ProductLabelItem()
                        {
                            ID = p.ID,
                            Name = p.Name,
                            DateCreated = p.DateCreated,
                            DateUpdated = p.DateUpdated,
                            CreatedBy = p.CreatedBy,
                            UpdatedBy = p.UpdatedBy,
                            PictureID = p.PictureID,
                            Number = p.Number,
                            IsShow = p.IsShow
                        };
            return (T)(object)query.FirstOrDefault();
        }

        #region Insert, Edit, Delete, Save

        public void Show(int id)
        {
            var typeProduct = web365db.tblProductLabel.SingleOrDefault(p => p.ID == id);
            typeProduct.IsShow = true;
            web365db.Entry(typeProduct).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var typeProduct = web365db.tblProductLabel.SingleOrDefault(p => p.ID == id);
            typeProduct.IsShow = false;
            web365db.Entry(typeProduct).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        #endregion

        #region Check
        public bool NameExist(int id, string name)
        {
            var query = web365db.tblProductLabel.Count(c => c.Name.ToLower() == name.ToLower() && c.ID != id);

            return query > 0;
        }
        #endregion
    }
}
