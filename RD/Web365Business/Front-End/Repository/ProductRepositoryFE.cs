using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Business.Front_End.IRepository;
using Web365Domain;
using Web365Models;
using Web365Utility;

namespace Web365Business.Front_End.Repository
{
    public class ProductRepositoryFE : BaseFE, IProductRepositoryFE
    {
        public tblProduct GetById(int id)
        {
            var query = from p in web365db.tblProduct
                        where p.ID == id
                        orderby p.ID descending
                        select p;
            return query.FirstOrDefault();
        }

        public ProductItem GetItemById(int id)
        {
            var key = string.Format("ProductRepositoryGetItemById{0}", id);

            var product = new ProductItem();

            if (!this.TryGetCache<ProductItem>(out product, key))
            {
                var result = GetById(id);

                product = new ProductItem()
                {
                    ID = result.ID,
                    TypeID = result.TypeID,
                    Name = result.Name,
                    NameAscii = result.NameAscii,
                    SEOTitle = result.SEOTitle,
                    SEODescription = result.SEODescription,
                    SEOKeyword = result.SEOKeyword,
                    DateCreated = result.DateCreated,
                    DateUpdated = result.DateUpdated,
                    Number = result.Number,
                    PictureID = result.PictureID,
                    Summary = result.Summary,
                    Detail = result.Detail,
                    IsShow = result.IsShow
                };

                this.SetCache(key, product, 10);
            }            

            return product;
        }

        public ProductItem GetItemByAscii(string ascii)
        {
            var key = string.Format("ProductRepositoryGetItemByAscii{0}", ascii);

            var product = new ProductItem();

            if (!this.TryGetCache<ProductItem>(out product, key))
            {
                var result = (from p in web365db.tblProduct
                            where p.NameAscii == ascii && p.IsShow == true && p.IsDeleted == false
                            orderby p.ID descending
                            select p).FirstOrDefault();

                product = new ProductItem()
                {
                    ID = result.ID,
                    TypeID = result.TypeID,
                    Name = result.Name,
                    NameAscii = result.NameAscii,
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
                    IsShow = result.IsShow,
                    Picture = new PictureItem(){
                        FileName = result.tblPicture.FileName
                    },
                    ListPicture = result.tblPicture1.Select(p => new PictureItem() { 
                        FileName = p.FileName
                    }).ToList(),
                    ListProductVariant = result.tblProduct_Variant.OrderBy(p=> p.Price).Select(p => new ProductVariantItem()
                    {
                        ID = p.ID,
                        ProductID = p.ProductID,
                        Name = p.Name,
                        Price = p.Price.HasValue ? p.Price.Value : 0,
                        IsOutOfStock = p.IsOutOfStock.HasValue ? p.IsOutOfStock.Value : false
                    }).ToList()
                };

                this.SetCache(key, product, 10);
            }

            return product;
        }

        public ProductModel SearchProduct(string keyword, int skip, int top)
        {
            var key = string.Format("ProductRepositorySearchProduct{0}{1}{2}", keyword, skip, top);

            var product = new ProductModel();

            if (!this.TryGetCache<ProductModel>(out product, key))
            {
                var query = from p in web365db.tblProduct
                            where p.NameAscii.Contains(keyword) && p.tblTypeProduct.LanguageId == LanguageId && p.IsShow == true && p.IsDeleted == false
                            orderby p.Number descending, p.ID descending
                            select new ProductItem()
                            {
                                ID = p.ID,
                                TypeID = p.TypeID,
                                Name = p.Name,
                                NameAscii = p.NameAscii,
                                SEOTitle = p.SEOTitle,
                                SEODescription = p.SEODescription,
                                SEOKeyword = p.SEOKeyword,
                                DateCreated = p.DateCreated,
                                DateUpdated = p.DateUpdated,
                                Number = p.Number,
                                PictureID = p.PictureID,
                                Summary = p.Summary,
                                Detail = p.Detail,
                                IsShow = p.IsShow,
                                Picture = new PictureItem()
                                {
                                    FileName = p.tblPicture.FileName
                                },
                                ProductType = new ProductTypeItem()
                                {
                                    Name = p.tblTypeProduct.Name,
                                    NameAscii = p.tblTypeProduct.tblTypeProduct2.tblTypeProduct2.NameAscii + "/" + p.tblTypeProduct.tblTypeProduct2.NameAscii + "/" + p.tblTypeProduct.NameAscii
                                }
                            };

                product.Total = query.Count();

                product.ListProduct = query.Skip(skip).Take(top).ToList();

                if (product.Total > 0)
                {

                    var listProductId = product.ListProduct.Select(p => p.ID).ToArray();

                    var productVariants = (from p in web365db.tblProduct_Variant
                                           where listProductId.Contains(p.ProductID.Value)
                                           orderby p.DisplayOrder descending, p.Price ascending
                                           select new ProductVariantItem()
                                           {
                                               ID = p.ID,
                                               ProductID = p.ProductID,
                                               Name = p.Name,
                                               Price = p.Price.HasValue ? p.Price.Value : 0,
                                               IsOutOfStock = p.IsOutOfStock.HasValue ? p.IsOutOfStock.Value : false
                                           }).ToList();

                    product.ListProduct.ForEach(p =>
                    {
                        p.ListProductVariant = productVariants.Where(v => v.ProductID == p.ID).ToList();
                    });
                }

                this.SetCache(key, product, 10);
            }

            return product;
        }

        public ProductModel SearchProductAdvance(string asciiType, string asciiFilter, int skip, int top)
        {
            var key = string.Format("ProductRepositorySearchProductAdvance{0}{1}{2}{3}", asciiType, asciiFilter, skip, top);

            var product = new ProductModel();

            if (!this.TryGetCache<ProductModel>(out product, key))
            {
                var arrTypeId = new int[]{};

                if (string.IsNullOrEmpty(asciiType))
                {
                    arrTypeId = web365db.tblTypeProduct.Where(t => t.IsShow == true && t.IsDeleted == false).Select(t => t.ID).ToArray();
                }
                else
                {
                    arrTypeId = web365db.tblTypeProduct.Where(t => t.NameAscii == asciiType && t.IsShow == true && t.IsDeleted == false).Select(t => t.ID).ToArray();
                }

                var query = from p in web365db.tblProduct
                            where p.tblProductFilter.Any(f => f.NameAscii == asciiFilter) && arrTypeId.Contains(p.TypeID.Value) && p.tblTypeProduct.LanguageId == LanguageId && p.IsShow == true && p.IsDeleted == false
                            orderby p.Number descending, p.ID descending
                            select new ProductItem()
                            {
                                ID = p.ID,
                                TypeID = p.TypeID,
                                Name = p.Name,
                                NameAscii = p.NameAscii,
                                SEOTitle = p.SEOTitle,
                                SEODescription = p.SEODescription,
                                SEOKeyword = p.SEOKeyword,
                                DateCreated = p.DateCreated,
                                DateUpdated = p.DateUpdated,
                                Number = p.Number,
                                PictureID = p.PictureID,
                                Summary = p.Summary,
                                Detail = p.Detail,
                                IsShow = p.IsShow,
                                Picture = new PictureItem()
                                {
                                    FileName = p.tblPicture.FileName
                                },
                                ProductType = new ProductTypeItem()
                                {
                                    Name = p.tblTypeProduct.Name,
                                    NameAscii = p.tblTypeProduct.tblTypeProduct2.tblTypeProduct2.NameAscii + "/" + p.tblTypeProduct.tblTypeProduct2.NameAscii + "/" + p.tblTypeProduct.NameAscii
                                }
                            };

                product.Total = query.Count();

                product.ListProduct = query.Skip(skip).Take(top).ToList();

                if (product.Total > 0)
                {

                    var listProductId = product.ListProduct.Select(p => p.ID).ToArray();

                    var productVariants = (from p in web365db.tblProduct_Variant
                                           where listProductId.Contains(p.ProductID.Value)
                                           orderby p.DisplayOrder descending, p.Price ascending
                                           select new ProductVariantItem()
                                           {
                                               ID = p.ID,
                                               ProductID = p.ProductID,
                                               Name = p.Name,
                                               Price = p.Price.HasValue ? p.Price.Value : 0,
                                               IsOutOfStock = p.IsOutOfStock.HasValue ? p.IsOutOfStock.Value : false
                                           }).ToList();

                    product.ListProduct.ForEach(p =>
                    {
                        p.ListProductVariant = productVariants.Where(v => v.ProductID == p.ID).ToList();
                    });
                }

                this.SetCache(key, product, 10);
            }

            return product;
        }

        public ProductModel GetListByTypeAscii(int skip, int top, string typeAscii)
        {
            var key = string.Format("ProductRepositoryGetListByTypeAscii{0}{1}{2}", skip, top, typeAscii);

            var product = new ProductModel();

            if (!this.TryGetCache<ProductModel>(out product, key))
            {

                var listType = web365db.tblTypeProduct.Where(t => t.NameAscii == typeAscii || t.tblTypeProduct2.NameAscii == typeAscii && t.LanguageId == LanguageId && t.IsShow == true && t.IsDeleted == false).Select(t => t.ID).ToArray();

                var query = from p in web365db.tblProduct
                            where listType.Contains(p.TypeID.Value) && p.tblTypeProduct.LanguageId == LanguageId && p.IsShow == true && p.IsDeleted == false
                            orderby p.Number descending, p.ID descending
                            select new ProductItem()
                            {
                                ID = p.ID,
                                TypeID = p.TypeID,
                                Name = p.Name,
                                NameAscii = p.NameAscii,
                                SEOTitle = p.SEOTitle,
                                SEODescription = p.SEODescription,
                                SEOKeyword = p.SEOKeyword,
                                DateCreated = p.DateCreated,
                                DateUpdated = p.DateUpdated,
                                Number = p.Number,
                                PictureID = p.PictureID,
                                Summary = p.Summary,
                                Detail = p.Detail,
                                IsShow = p.IsShow,
                                Picture = new PictureItem()
                                {
                                    FileName = p.tblPicture.FileName
                                },
                                ProductType = new ProductTypeItem()
                                {
                                    Name = p.tblTypeProduct.Name,
                                    NameAscii = p.tblTypeProduct.NameAscii
                                }
                            };

                product.Total = query.Count();

                product.ListProduct = query.Skip(skip).Take(top).ToList();

                if (product.Total > 0)
                {

                    var listProductId = product.ListProduct.Select(p => p.ID).ToArray();

                    var productVariants = (from p in web365db.tblProduct_Variant
                                       where listProductId.Contains(p.ProductID.Value)
                                        orderby p.DisplayOrder descending, p.Price ascending
                                       select new ProductVariantItem()
                                       {
                                           ID = p.ID,
                                           ProductID = p.ProductID,
                                           Name = p.Name,
                                           Price = p.Price.HasValue ? p.Price.Value : 0,
                                           IsOutOfStock = p.IsOutOfStock.HasValue ? p.IsOutOfStock.Value : false
                                       }).ToList();

                    product.ListProduct.ForEach(p => {
                        p.ListProductVariant = productVariants.Where(v => v.ProductID == p.ID).ToList();
                    });
                }

                this.SetCache(key, product, 10);
            }

            return product;
        }

        public ProductModel GetListByGroupId(int groupId)
        {
            var key = string.Format("ProductRepositoryGetListByGroupId{0}", groupId);

            var product = new ProductModel();

            if (!this.TryGetCache<ProductModel>(out product, key))
            {
                var query = from p in web365db.tblProduct
                            where p.tblProduct_Status_Map.Any(s => s.tblProductStatus.ID == groupId) && p.tblTypeProduct.LanguageId == LanguageId && p.IsShow == true && p.IsDeleted == false
                            orderby p.Number descending, p.ID descending
                            select new ProductItem()
                            {
                                ID = p.ID,
                                TypeID = p.TypeID,
                                Name = p.Name,
                                NameAscii = p.NameAscii,
                                SEOTitle = p.SEOTitle,
                                SEODescription = p.SEODescription,
                                SEOKeyword = p.SEOKeyword,
                                DateCreated = p.DateCreated,
                                DateUpdated = p.DateUpdated,
                                Number = p.Number,
                                PictureID = p.PictureID,
                                Summary = p.Summary,
                                Detail = p.Detail,
                                IsShow = p.IsShow,
                                Picture = new PictureItem()
                                {
                                    FileName = p.tblPicture.FileName
                                },
                                ProductType = new ProductTypeItem()
                                {
                                    Name = p.tblTypeProduct.Name,
                                    NameAscii = p.tblTypeProduct.tblTypeProduct2.tblTypeProduct2.NameAscii + "/" + p.tblTypeProduct.tblTypeProduct2.NameAscii + "/" + p.tblTypeProduct.NameAscii
                                }
                            };

                product.Total = query.Count();

                product.ListProduct = query.ToList();

                if (product.Total > 0)
                {

                    var listProductId = product.ListProduct.Select(p => p.ID).ToArray();

                    var productVariants = (from p in web365db.tblProduct_Variant
                                           where listProductId.Contains(p.ProductID.Value)
                                           orderby p.DisplayOrder descending, p.Price ascending
                                           select new ProductVariantItem()
                                           {
                                               ID = p.ID,
                                               ProductID = p.ProductID,
                                               Name = p.Name,
                                               Price = p.Price.HasValue ? p.Price.Value : 0,
                                               IsOutOfStock = p.IsOutOfStock.HasValue ? p.IsOutOfStock.Value : false
                                           }).ToList();

                    product.ListProduct.ForEach(p =>
                    {
                        p.ListProductVariant = productVariants.Where(v => v.ProductID == p.ID).ToList();
                    });
                }

                this.SetCache(key, product, 10);
            }

            return product;
        }

        public ProductModel GetListByGroupAscii(string groupAscii)
        {
            var key = string.Format("ProductRepositoryGetListByGroupAscii{0}", groupAscii);

            var product = new ProductModel();

            if (!this.TryGetCache<ProductModel>(out product, key))
            {
                var query = from p in web365db.tblProduct
                            where p.tblProduct_Status_Map.Any(s => s.tblProductStatus.NameAscii == groupAscii) && p.tblTypeProduct.LanguageId == LanguageId && p.IsShow == true && p.IsDeleted == false
                            orderby p.Number descending, p.ID descending
                            select new ProductItem()
                            {
                                ID = p.ID,
                                TypeID = p.TypeID,
                                Name = p.Name,
                                NameAscii = p.NameAscii,
                                SEOTitle = p.SEOTitle,
                                SEODescription = p.SEODescription,
                                SEOKeyword = p.SEOKeyword,
                                DateCreated = p.DateCreated,
                                DateUpdated = p.DateUpdated,
                                Number = p.Number,
                                PictureID = p.PictureID,
                                Summary = p.Summary,
                                Detail = p.Detail,
                                IsShow = p.IsShow,
                                Picture = new PictureItem()
                                {
                                    FileName = p.tblPicture.FileName
                                },
                                ProductType = new ProductTypeItem()
                                {
                                    Name = p.tblTypeProduct.Name,
                                    NameAscii = p.tblTypeProduct.tblTypeProduct2.tblTypeProduct2.NameAscii + "/" + p.tblTypeProduct.tblTypeProduct2.NameAscii + "/" + p.tblTypeProduct.NameAscii
                                }
                            };

                product.Total = query.Count();

                product.ListProduct = query.ToList();

                if (product.Total > 0)
                {

                    var listProductId = product.ListProduct.Select(p => p.ID).ToArray();

                    var productVariants = (from p in web365db.tblProduct_Variant
                                           where listProductId.Contains(p.ProductID.Value)
                                           orderby p.DisplayOrder descending, p.Price ascending
                                           select new ProductVariantItem()
                                           {
                                               ID = p.ID,
                                               ProductID = p.ProductID,
                                               Name = p.Name,
                                               Price = p.Price.HasValue ? p.Price.Value : 0,
                                               IsOutOfStock = p.IsOutOfStock.HasValue ? p.IsOutOfStock.Value : false
                                           }).ToList();

                    product.ListProduct.ForEach(p =>
                    {
                        p.ListProductVariant = productVariants.Where(v => v.ProductID == p.ID).ToList();
                    });
                }

                this.SetCache(key, product, 10);
            }

            return product;
        }
    }
}
