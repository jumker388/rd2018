using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Business.Front_End.IRepository;
using Web365Domain;
using Web365Utility;

namespace Web365Business.Front_End.Repository
{
    public class ProductTypeRepositoryFE : BaseFE, IProductTypeRepositoryFE
    {
        public tblTypeProduct GetById(int id)
        {
            var query = from p in web365db.tblTypeProduct
                        where p.ID == id
                        orderby p.ID descending
                        select p;
            return query.FirstOrDefault();
        }

        public ProductTypeItem GetByParentAsciiAndAscii(string parentAscii, string ascii)
        {
            var key = string.Format("ProductTypeRepositoryGetByParentAsciiAndAscii{0}{1}", parentAscii, ascii);

            var productType = new ProductTypeItem();

            if (!this.TryGetCache<ProductTypeItem>(out productType, key))
            {
                var result = (from p in web365db.tblTypeProduct
                              where p.tblTypeProduct2.NameAscii == parentAscii && p.NameAscii == ascii  && p.IsShow == true && p.IsDeleted == false
                              orderby p.ID descending
                              select p).FirstOrDefault();

                if (result != null)
                {
                    productType = new ProductTypeItem()
                    {
                        ID = result.ID,
                        Name = result.Name,
                        NameAscii = result.NameAscii,
                        ParentName = result.tblTypeProduct2.Name,
                        ParentNameAscii = result.tblTypeProduct2.NameAscii,
                        Detail = result.Detail,
                        IsShow = result.IsShow,
                        ListPicture = result.tblPicture1.Select(p => new PictureItem()
                        {
                            ID = p.ID,
                            Link = p.Link,
                            FileName = p.FileName

                        }).ToList()
                    };
                }

                this.SetCache(key, productType, 10);
            }

            return productType;
        }

        public ProductTypeItem GetItemById(int id)
        {
            var key = string.Format("ProductRepositoryGetItemById{0}", id);

            var product = new ProductTypeItem();

            if (!this.TryGetCache<ProductTypeItem>(out product, key))
            {
                var result = GetById(id);

                product = new ProductTypeItem()
                {
                    ID = result.ID,
                    Name = result.Name,
                    NameAscii = result.NameAscii,
                    SEOTitle = result.SEOTitle,
                    SEODescription = result.SEODescription,
                    SEOKeyword = result.SEOKeyword,
                    DateCreated = result.DateCreated,
                    DateUpdated = result.DateUpdated,
                    Number = result.Number,
                    PictureID = result.PictureID,
                    Detail = result.Detail,
                    IsShow = result.IsShow
                };

                this.SetCache(key, product, 10);
            }            

            return product;
        }

        public ProductTypeItem GetByAsciiAndParent(string ascii, int parentId)
        {
            var key = string.Format("ProductTypeRepositoryGetByAsciiAndParent{0}{1}", ascii, parentId);

            var product = new ProductTypeItem();

            if (!this.TryGetCache<ProductTypeItem>(out product, key))
            {
                var query = from p in web365db.tblTypeProduct
                            where p.NameAscii == ascii && p.Parent == parentId && p.LanguageId == LanguageId
                            orderby p.ID descending
                            select new ProductTypeItem() {
                                ID = p.ID,
                                Name = p.Name,
                                NameAscii = p.NameAscii,
                                ParentName = p.tblTypeProduct2.Name,
                                ParentNameAscii = p.tblTypeProduct2.NameAscii,
                                SEOTitle = p.SEOTitle,
                                SEODescription = p.SEODescription,
                                SEOKeyword = p.SEOKeyword,
                                DateCreated = p.DateCreated,
                                DateUpdated = p.DateUpdated,
                                Number = p.Number,
                                PictureID = p.PictureID,
                                Detail = p.Detail,
                                IsShow = p.IsShow
                            };

                product = query.FirstOrDefault();

                this.SetCache(key, product, 10);
            }

            return product;
        }

        public List<ProductTypeItem> GetListByParent(int? parentId)
        {
            var key = string.Format("ProductTypeRepositoryGetListByParent{0}", parentId);

            var list = new List<ProductTypeItem>();

            if (!this.TryGetCache<List<ProductTypeItem>>(out list, key))
            {
                var query = from p in web365db.tblTypeProduct
                            where p.Parent == parentId && p.LanguageId == LanguageId && p.IsShow == true && p.IsDeleted == false
                            orderby p.ID descending
                            select new ProductTypeItem() { 
                                ID = p.ID,
                                Name = p.Name,
                                NameAscii = p.NameAscii,
                                Number = p.Number,
                                ParentName = p.tblTypeProduct2.Name,
                                ParentNameAscii = p.tblTypeProduct2.NameAscii
                            };

                list = query.ToList();

                this.SetCache(key, list, 10);
            }

            return list;
        }

        public List<ProductTypeItem> GetAllChildByParent(int? parentId)
        {
            var key = string.Format("ProductTypeRepositoryGetAllChildByParent{0}", parentId);

            var list = new List<ProductTypeItem>();

            if (!this.TryGetCache<List<ProductTypeItem>>(out list, key))
            {
                var query = web365db.Database.SqlQuery<ProductTypeItem>("EXEC [dbo].[PRC_Product_GetAllChildTypeByParentID] {0}", parentId);

                list = query.Select(p => new ProductTypeItem()
                {
                    ID = p.ID,
                    Parent = p.Parent,
                    Name = p.Name,
                    NameAscii = p.NameAscii,
                    Number = p.Number,
                    IsShow = p.IsShow
                }).ToList();

                this.SetCache(key, list, 10);
            }

            return list;
        }

        public List<ProductTypeItem> GetByGroup(int groupId)
        {
            var key = string.Format("ProductTypeRepositoryGetByGroup{0}", groupId);

            var list = new List<ProductTypeItem>();

            if (!this.TryGetCache<List<ProductTypeItem>>(out list, key))
            {
                var query = from p in web365db.tblTypeProduct
                            where p.tblProductType_Group_Map.Any(g => g.GroupTypeID == groupId) && p.LanguageId == LanguageId && p.IsShow == true && p.IsDeleted == false
                            orderby p.ID descending
                            select new ProductTypeItem()
                            {
                                ID = p.ID,
                                Name = p.Name,
                                NameAscii = p.NameAscii,
                                Number = p.Number,
                                ParentName = p.tblTypeProduct2.Name,
                                ParentNameAscii = p.tblTypeProduct2.NameAscii
                            };

                list = query.ToList();

                this.SetCache(key, list, 10);
            }

            return list;
        }
    }
}
