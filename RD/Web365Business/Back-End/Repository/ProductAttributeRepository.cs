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
    public class ProductAttributeRepository : BaseBE<tblProductAttribute>, IProductAttributeRepository
    {
        /// <summary>
        /// function get all data tblProductAttribute
        /// </summary>
        /// <returns></returns>
        public List<ProductAttributeItem> GetList(out int total, string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool isDelete = false)
        {
            var query = from p in web365db.tblProductAttribute
                        where p.Name.ToLower().Contains(name) && p.IsDeleted == isDelete
                        orderby p.ID descending
                        select new ProductAttributeItem()
                        {
                            ID = p.ID,
                            Name = p.Name,
                            NameAscii = p.NameAscii,
                            Number = p.Number,
                            GroupID = p.GroupID,
                            IsShow = p.IsShow
                        };

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Skip(currentRecord).Take(numberRecord).ToList();
        }

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            var query = from p in web365db.tblProductAttribute
                        where p.IsShow == isShow && p.IsDeleted == isDelete
                        orderby p.ID descending
                        select new ProductAttributeItem()
                        {
                            ID = p.ID,
                            Name = p.Name,
                            NameAscii = p.NameAscii,
                            GroupID = p.GroupID
                        };

            return (T)(object)query.ToList();
        }

        public List<ProductAttributeItem> GetListByProductType(int typeId, int productId, bool isShow = true, bool isDelete = false)
        {
            var query = from p in web365db.tblProductAttribute
                        where p.tblProductGroupAttribute.ProductTypeID == typeId && p.IsShow == isShow && p.IsDeleted == isDelete
                        orderby p.Number descending
                        select new ProductAttributeItem()
                        {
                            ID = p.ID,
                            Name = p.Name,
                            NameAscii = p.NameAscii,
                            GroupID = p.GroupID,
                            ValueMap = p.tblProduct_Attribute_Map.FirstOrDefault(a => a.ProductID == productId).Name ?? string.Empty
                        };

            return query.ToList();
        }

        /// <summary>
        /// get product type item
        /// </summary>
        /// <param name="id">id of product type</param>
        /// <returns></returns>
        public T GetItemById<T>(int id)
        {
            var query = from p in web365db.tblProductAttribute
                        where p.ID == id
                        orderby p.ID descending
                        select new ProductAttributeItem()
                        {
                            ID = p.ID,
                            Name = p.Name,
                            NameAscii = p.NameAscii,
                            Detail = p.Detail,
                            DateCreated = p.DateCreated,
                            DateUpdated = p.DateUpdated,
                            Number = p.Number,
                            GroupID = p.GroupID,
                            IsShow = p.IsShow
                        };
            return (T)(object)query.FirstOrDefault();
        }

        #region Insert, Edit, Delete, Save

        public void Show(int id)
        {
            var filterProduct = web365db.tblProductAttribute.SingleOrDefault(p => p.ID == id);
            filterProduct.IsShow = true;
            web365db.Entry(filterProduct).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var filterProduct = web365db.tblProductAttribute.SingleOrDefault(p => p.ID == id);
            filterProduct.IsShow = false;
            web365db.Entry(filterProduct).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void AddProductAttribute(int productId, int attributeId, string value)
        {
            var attributeMap = web365db.tblProduct_Attribute_Map.FirstOrDefault(a => a.ProductID == productId && a.AttributeID == attributeId);

            if (attributeMap != null)
            {
                web365db.tblProduct_Attribute_Map.Remove(attributeMap);

                web365db.SaveChanges();
            }

            if (!string.IsNullOrEmpty(value))
            {
                web365db.tblProduct_Attribute_Map.Add(new tblProduct_Attribute_Map()
                {
                    ProductID = productId,
                    AttributeID = attributeId,
                    Name = value
                });

                web365db.SaveChanges();
            }            
        }

        #endregion


        public bool NameExist(int id, string name)
        {
            throw new NotImplementedException();
        }
    }
}
