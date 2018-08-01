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
    public class LayoutContentRepository : BaseBE<tblLayoutContent>, ILayoutContentRepository
    {
        /// <summary>
        /// function get all data tblTypeProduct
        /// </summary>
        /// <returns></returns>
        public List<LayoutContentItem> GetList(out int total, string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool isDelete = false)
        {
            var query = from p in web365db.tblLayoutContent
                        where p.Name.ToLower().Contains(name) && p.IsDeleted == isDelete
                        select p;


            query = query.Where(p => p.LanguageId == LanguageId);

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Select(p => new LayoutContentItem()
            {
                ID = p.ID,
                Name = p.Name,
                NameAscii = p.NameAscii,
                DateCreated = p.DateCreated,
                SEOTitle = p.SEOTitle,
                SEODescription = p.SEODescription,
                SEOKeyword = p.SEOKeyword,
                IsShow = p.IsShow
            }).Skip(currentRecord).Take(numberRecord).ToList();
        }

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            var query = from p in web365db.tblLayoutContent
                        where p.IsShow == isShow && p.IsDeleted == isDelete
                        select p;

            query = query.Where(p => p.LanguageId == LanguageId);

            return (T)(object)query.OrderByDescending(p => p.ID).Select(p => new LayoutContentItem()
            {
                ID = p.ID,
                Name = p.Name,
                NameAscii = p.NameAscii
            }).ToList();
        }

        /// <summary>
        /// get product type item
        /// </summary>
        /// <param name="id">id of product type</param>
        /// <returns></returns>
        public T GetItemById<T>(int id)
        {
            var result = GetById<tblLayoutContent>(id);

            return (T)(object)new LayoutContentItem()
                        {
                            ID = result.ID,
                            Name = result.Name,
                            NameAscii = result.NameAscii,
                            SEOTitle = result.SEOTitle,
                            SEODescription = result.SEODescription,
                            SEOKeyword = result.SEOKeyword,
                            DateCreated = result.DateCreated,
                            DateUpdated = result.DateUpdated,
                            PictureID = result.PictureID,
                            Detail = result.Detail,
                            IsShow = result.IsShow
                        };
        }        

        #region Insert, Edit, Delete, Save
        public void Show(int id)
        {
            var layoutContent = web365db.tblLayoutContent.SingleOrDefault(p => p.ID == id);
            layoutContent.IsShow = true;
            web365db.Entry(layoutContent).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var layoutContent = web365db.tblLayoutContent.SingleOrDefault(p => p.ID == id);
            layoutContent.IsShow = false;
            web365db.Entry(layoutContent).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        #endregion

        #region Check
        public bool NameExist(int id, string name)
        {
            var query = web365db.tblLayoutContent.Count(c => c.Name.ToLower() == name.ToLower() && c.LanguageId == LanguageId && c.ID != id);

            return query > 0;
        }
        #endregion
    }
}
