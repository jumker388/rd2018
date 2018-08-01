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
    public class ProductTypeRepository : BaseBE<tblTypeProduct>, IProductTypeRepository
    {
        /// <summary>
        /// function get all data tblTypeProduct
        /// </summary>
        /// <returns></returns>
        public List<ProductTypeItem> GetList(out int total, string name, int? parentId, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool isDelete = false)
        {
            var query = from p in web365db.tblTypeProduct
                        where p.Name.ToLower().Contains(name) && p.IsDeleted == isDelete
                        select p;

            query = query.Where(p => p.LanguageId == LanguageId);

            if (parentId.HasValue)
            {
                query = query.Where(p => p.Parent == parentId);
            }

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Select(p => new ProductTypeItem()
            {
                ID = p.ID,
                Name = p.Name,
                NameAscii = p.NameAscii,
                SEOTitle = p.SEOTitle,
                SEODescription = p.SEODescription,
                SEOKeyword = p.SEOKeyword,
                PictureID = p.PictureID,
                Number = p.Number,
                Detail = p.Detail,
                Parent = p.Parent,
                IsShow = p.IsShow
            }).Skip(currentRecord).Take(numberRecord).ToList();
        }

        public List<ProductTypeItem> GetListOfGroup(int groupId, bool isShow = true, bool isDelete = false)
        {
            var query = from p in web365db.tblProductType_Group_Map
                        where p.GroupTypeID == groupId && p.tblTypeProduct.IsShow == isShow && p.tblTypeProduct.IsDeleted == isDelete
                        select p;

            query = query.Where(p => p.tblTypeProduct.LanguageId == LanguageId);

            return query.OrderByDescending(p => p.DisplayOrder).Select(p => new ProductTypeItem()
            {
                ID = p.tblTypeProduct.ID,
                Name = p.tblTypeProduct.Name,
                NameAscii = p.tblTypeProduct.NameAscii,
                IsShow = p.tblTypeProduct.IsShow
            }).ToList();
        }

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            var query = from p in web365db.tblTypeProduct
                        where p.IsShow == isShow && p.IsDeleted == isDelete
                        select p;

            query = query.Where(p => p.LanguageId == LanguageId);

            return (T)(object)query.OrderByDescending(p => p.ID).Select(p => new ProductTypeItem()
            {
                ID = p.ID,
                Name = p.Name,
                NameAscii = p.NameAscii,
                Parent = p.Parent
            }).ToList();
        }

        /// <summary>
        /// get product type item
        /// </summary>
        /// <param name="id">id of product type</param>
        /// <returns></returns>
        public T GetItemById<T>(int id)
        {

            var result = GetById<tblTypeProduct>(id);

            return (T)(object)new ProductTypeItem()
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
                Number = result.Number,
                Detail = result.Detail,
                Parent = result.Parent,
                IsShow = result.IsShow,
                ListPictureID = result.tblPicture1.Select(p => p.ID).ToArray()
            };
        }

        #region Insert, Edit, Delete, Save

        public void Show(int id)
        {
            var typeProduct = web365db.tblTypeProduct.SingleOrDefault(p => p.ID == id);
            typeProduct.IsShow = true;
            web365db.Entry(typeProduct).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var typeProduct = web365db.tblTypeProduct.SingleOrDefault(p => p.ID == id);
            typeProduct.IsShow = false;
            web365db.Entry(typeProduct).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void ResetListPicture(int id, string listPictureId)
        {
            web365db.Database.SqlQuery<object>("EXEC [dbo].[PRC_ResetPictureProductType] {0}, {1}", id, listPictureId).FirstOrDefault();
        }

        #endregion

        #region Check
        public bool NameExist(int id, string name)
        {
            var query = web365db.tblTypeProduct.Count(c => c.Name.ToLower() == name.ToLower() && c.LanguageId == LanguageId && c.ID != id);

            return query > 0;
        }
        #endregion
    }
}
