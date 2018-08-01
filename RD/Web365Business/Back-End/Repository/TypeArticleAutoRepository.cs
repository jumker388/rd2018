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
    public class TypeArticleAutoRepository : BaseBE<tblArticleAuto>, ITypeArticleAutoRepository
    {

        /// <summary>
        /// function get all data tblTypeProduct
        /// </summary>
        /// <returns></returns>
        public List<ArticleAutoItem> GetList(out int total, string name, int? parentId, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool isDelete = false)
        {
            var query = from p in web365db.tblArticleAuto
                        where p.Name.ToLower().Contains(name) && p.IsDeleted == isDelete
                        select p;
            if (parentId.HasValue)
            {
                query = query.Where(p => p.Parent == parentId);
            }

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Select(p => new ArticleAutoItem()
            {
                ID = p.ID,
                Name = p.Name,
                Link = p.Link,
                PictureID = p.PictureID.HasValue ? p.PictureID.Value : 0,
                Number = p.Number.HasValue ? p.Number.Value : 0,
                Parent = p.Parent,
                IsShow = p.IsShow.HasValue && p.IsShow.Value,
                IsDeleted = p.IsDeleted.HasValue && p.IsDeleted.Value,
                Xpath = p.Xpath,
                XpathTitle = p.XpathTitle,
                XpathSummary = p.XpathSummary,
                XpathDetail = p.XpathDetail,
                XpathImage = p.XpathImage,
                Xpage = p.Xpage,
                XpathPaging = p.XpathPaging,
                TypeID = p.TypeID.HasValue ? p.TypeID.Value : 0
            }).Skip(currentRecord).Take(numberRecord).ToList();
        }

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            var query = from p in web365db.tblArticleAuto
                        where p.IsShow == isShow && p.IsDeleted == isDelete
                        select p;


            return (T)(object)query.OrderByDescending(p => p.ID).Select(p => new ArticleAutoItem()
            {
                ID = p.ID,
                Name = p.Name,
                Link = p.Link,
                PictureID = p.PictureID.HasValue ? p.PictureID.Value : 0,
                Parent = p.Parent,
                IsShow = p.IsShow.HasValue && p.IsShow.Value,
                IsDeleted = p.IsDeleted.HasValue && p.IsDeleted.Value,
                Number = p.Number.HasValue ? p.Number.Value : 0,
                Xpath = p.Xpath,
                XpathTitle = p.XpathTitle,
                XpathSummary = p.XpathSummary,
                XpathDetail = p.XpathDetail,
                Xpage = p.Xpage,
                XpathPaging = p.XpathPaging,
                XpathImage = p.XpathImage,
                TypeID = p.TypeID.HasValue ? p.TypeID.Value : 0
            }).ToList();
        }

        /// <summary>
        /// get product type item
        /// </summary>
        /// <param name="id">id of product type</param>
        /// <returns></returns>
        public T GetItemById<T>(int id)
        {

            var result = GetById<tblArticleAuto>(id);

            return (T)(object)new ArticleAutoItem()
            {
                ID = result.ID,
                Name = result.Name,
                Link = result.Link,
                PictureID = result.PictureID.HasValue ? result.PictureID.Value : 0,
                Parent = result.Parent,
                IsShow = result.IsShow.HasValue && result.IsShow.Value,
                IsDeleted = result.IsDeleted.HasValue && result.IsDeleted.Value,
                Number = result.Number.HasValue ? result.Number.Value : 0,
                Xpath = result.Xpath,
                XpathTitle = result.XpathTitle,
                XpathSummary = result.XpathSummary,
                XpathDetail = result.XpathDetail,
                Xpage = result.Xpage,
                XpathPaging = result.XpathPaging,
                XpathImage = result.XpathImage,
                TypeID = result.TypeID.HasValue ? result.TypeID.Value : 0
            };
        }


        #region Insert, Edit, Delete, Save

        public void Show(int id)
        {
            var typeProduct = web365db.tblArticleAuto.SingleOrDefault(p => p.ID == id);
            typeProduct.IsShow = true;
            web365db.Entry(typeProduct).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var typeProduct = web365db.tblArticleAuto.SingleOrDefault(p => p.ID == id);
            typeProduct.IsShow = false;
            web365db.Entry(typeProduct).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        #endregion

        #region Check
        public bool NameExist(int id, string name)
        {
            var query = web365db.tblArticleAuto.Count(c => c.Link.ToLower() == name.ToLower() && c.ID != id);

            return query > 0;
        }
        #endregion
    }
}
