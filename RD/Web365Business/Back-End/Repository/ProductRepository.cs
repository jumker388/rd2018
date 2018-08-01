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
    public class ProductRepository : BaseBE<tblProduct>, IProductRepository
    {
        /// <summary>
        /// function get all data tblProduct
        /// </summary>
        /// <returns></returns>
        public List<ProductItem> GetList(out int total, 
            string name, 
            string serial, 
            int? typeId, 
            int? manuId, 
            int? distributorId, 
            int? statusId, 
            int? labelId, 
            int currentRecord, 
            int numberRecord, 
            string propertyNameSort, 
            bool descending, 
            bool isDelete = false)
        {
            var query = from p in web365db.tblProduct
                        where p.Name.ToLower().Contains(name) && p.Serial.ToLower().Contains(serial) && p.IsDeleted == isDelete
                        select p;

            query = query.Where(p => p.tblTypeProduct.LanguageId == LanguageId);

            if(typeId.HasValue){
                query = query.Where(p => p.TypeID == typeId);
            }

            if (manuId.HasValue)
            {
                query = query.Where(p => p.Manufacturer == manuId);
            }

            if (distributorId.HasValue)
            {
                query = query.Where(p => p.Distributor == distributorId);
            }

            if (statusId.HasValue)
            {
                query = query.Where(p => p.tblProduct_Status_Map.Any(s => s.ProductStatusID == statusId));
            }

            if (labelId.HasValue)
            {
                query = query.Where(p => p.tblProductLabel.Any(l => l.ID == labelId));
            }

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Select(p => new ProductItem()
            {
                ID = p.ID,
                Name = p.Name,
                NameAscii = p.NameAscii,
                TypeID = p.TypeID,
                Number = p.Number,
                IsShow = p.IsShow
            }).Skip(currentRecord).Take(numberRecord).ToList();
        }

        public T GetItemById<T>(int id)
        {
            var result = GetById<tblProduct>(id);

            return (T)(object)new ProductItem()
            {
                ID = result.ID,
                Name = result.Name,
                NameAscii = result.NameAscii,
                Serial = result.Serial,
                TypeID = result.TypeID,
                Manufacturer = result.Manufacturer,
                Distributor = result.Distributor,                
                SEOTitle = result.SEOTitle,
                SEODescription = result.SEODescription,
                SEOKeyword = result.SEOKeyword,
                DateCreated = result.DateCreated,
                DateUpdated = result.DateUpdated,
                Number = result.Number,
                PictureID = result.PictureID,
                HighLights = result.HighLights,
                Summary = result.Summary,
                Detail = result.Detail,
                Price = result.Price,
                Quantity = result.Quantity,
                IsShow = result.IsShow,
                ListStatusId = result.tblProduct_Status_Map.Select(p => p.ProductStatusID.Value).ToArray(),
                ListLabelId = result.tblProductLabel.Select(p => p.ID).ToArray(),
                ListPictureID = result.tblPicture1.Select(p => p.ID).ToArray()
            };
        }

        public ProductItem GetEditFormFilter(int productId)
        {
            var result = GetById<tblProduct>(productId);

            return new ProductItem()
            {
                ID = result.ID,
                Name = result.Name,
                ListFilterId = result.tblProductFilter.Select(p => p.ID).ToArray()
            };
        }

        public void Show(int id)
        {
            var article = web365db.tblProduct.SingleOrDefault(p => p.ID == id);
            article.IsShow = true;
            web365db.Entry(article).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var article = web365db.tblProduct.SingleOrDefault(p => p.ID == id);
            article.IsShow = false;
            web365db.Entry(article).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void LabelForProduct(int productId, int[] labelId)
        {
            var user = GetById<tblProduct>(productId);

            user.tblProductLabel.Clear();

            web365db.SaveChanges();

            foreach (var item in labelId)
            {
                if (item > 0)
                {
                    var query = web365db.Database.SqlQuery<object>("EXEC PRC_InsertLabelForProduct {0}, {1}", productId, item);

                    query.FirstOrDefault();
                }
            }
        }

        public void FilterForProduct(int productId, int[] filterId)
        {
            var user = GetById<tblProduct>(productId);

            user.tblProductFilter.Clear();

            web365db.SaveChanges();

            foreach (var item in filterId)
            {
                if (item > 0)
                {
                    var query = web365db.Database.SqlQuery<object>("EXEC PRC_InsertFilterForProduct {0}, {1}", productId, item);

                    query.FirstOrDefault();
                }
            }
        }

        public void ResetListPicture(int id, string listPictureId)
        {
            web365db.Database.SqlQuery<object>("EXEC [dbo].[PRC_ResetPictureProduct] {0}, {1}", id, listPictureId).FirstOrDefault();
        }

        #region Check
        public bool NameExist(int id, string name)
        {
            var query = web365db.tblProduct.Count(c => c.Name.ToLower() == name.ToLower() && c.tblTypeProduct.LanguageId == LanguageId && c.ID != id);

            return query > 0;
        }
        #endregion

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            var query = from p in web365db.tblProduct
                        where p.IsShow == isShow && p.IsDeleted == isDelete
                        select p;

            query = query.Where(p => p.tblTypeProduct.LanguageId == LanguageId);

            return (T)(object)query.OrderByDescending(p => p.ID).Select(p => new ProductTypeItem()
            {
                ID = p.ID,
                Name = p.Name,
                NameAscii = p.NameAscii
            }).ToList();
        }
    }
}
