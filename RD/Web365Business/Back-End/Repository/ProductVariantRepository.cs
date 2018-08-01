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
    public class ProductVariantRepository : BaseBE<tblProduct_Variant>, IProductVariantRepository
    {
        /// <summary>
        /// function get all data tblProduct_Variant
        /// </summary>
        /// <returns></returns>
        public List<ProductVariantItem> GetList(out int total, 
            string name, 
            string code, 
            int? productId, 
            int currentRecord, 
            int numberRecord, 
            string propertyNameSort, 
            bool descending, 
            bool isDelete = false)
        {
            var query = from p in web365db.tblProduct_Variant
                        where p.Name.ToLower().Contains(name) && p.Code.ToLower().Contains(code) && p.IsDeleted == isDelete
                        select p;

            query = query.Where(p => p.tblProduct.tblTypeProduct.LanguageId == LanguageId);

            if (productId.HasValue)
            {
                query = query.Where(p => p.ProductID == productId.Value);
            }

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Select(p => new ProductVariantItem()
            {
                ID = p.ID,
                Name = p.Name,
                DisplayOrder = p.DisplayOrder,
                Code = p.Code,
                IsShow = p.IsShow
            }).Skip(currentRecord).Take(numberRecord).ToList();
        }

        public T GetItemById<T>(int id)
        {
            var result = GetById<tblProduct_Variant>(id);

            return (T)(object)new ProductVariantItem()
            {
                ID = result.ID,
                Name = result.Name,
                Code = result.Code,
                ProductID = result.ProductID,
                DateCreated = result.DateCreated,
                DateUpdated = result.DateUpdated,
                CreatedBy = result.CreatedBy,
                UpdatedBy = result.UpdatedBy,
                Price = result.Price.HasValue ? result.Price.Value : 0,
                VAT = result.VAT.HasValue ? result.VAT.Value : 0,
                Quantity = result.Quantity.HasValue ? result.Quantity.Value : 0,
                IsShow = result.IsShow,
                IsOutOfStock = result.IsOutOfStock.HasValue ? result.IsOutOfStock.Value : false
            };
        }

        public void Show(int id)
        {
            var article = web365db.tblProduct_Variant.SingleOrDefault(p => p.ID == id);
            article.IsShow = true;
            web365db.Entry(article).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var article = web365db.tblProduct_Variant.SingleOrDefault(p => p.ID == id);
            article.IsShow = false;
            web365db.Entry(article).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        #region Check
        public bool NameExist(int id, string name)
        {
            var query = web365db.tblProduct_Variant.Count(c => c.Name.ToLower() == name.ToLower() && c.tblProduct.tblTypeProduct.LanguageId == LanguageId && c.ID != id);

            return query > 0;
        }
        #endregion

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            throw new NotImplementedException();
        }
    }
}
