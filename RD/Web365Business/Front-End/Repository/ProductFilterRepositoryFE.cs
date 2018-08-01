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
    public class ProductFilterRepositoryFE : BaseFE, IProductFilterRepositoryFE
    {
        public List<ProductFilterItem> GetByParent(int? parent)
        {
            var key = string.Format("ProductFilterRepositoryGetByParent{0}", parent);

            var result = new List<ProductFilterItem>();

            if (!this.TryGetCache<List<ProductFilterItem>>(out result, key))
            {
                var query = from p in web365db.tblProductFilter
                            where p.IsShow == true && p.IsDeleted == false                            
                            select p;

                if(parent.HasValue)
                {
                    query = query.Where(p => p.Parent == parent);
                }
                else
                {
                    query = query.Where(p => p.Parent == null);
                }

                result = query.OrderByDescending(p => p.Number).Select(p => new ProductFilterItem()
                {
                    ID = p.ID,
                    Name = p.Name,
                    NameAscii = p.NameAscii,
                    Number = p.Number
                }).ToList();

                this.SetCache(key, result, 10);
            }

            return result;
        }
    }
}
