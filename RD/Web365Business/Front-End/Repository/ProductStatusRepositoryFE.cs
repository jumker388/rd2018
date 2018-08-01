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
    public class ProductStatusRepositoryFE : BaseFE, IProductStatusRepositoryFE
    {
        public ProductStatusItem GetByAscii(string ascii)
        {
            var key = string.Format("ProductStatusRepositoryGetByAscii{0}", ascii);

            var result = new ProductStatusItem();

            if (!this.TryGetCache<ProductStatusItem>(out result, key))
            {
                var query = from p in web365db.tblProductStatus
                            where p.NameAscii == ascii && p.IsShow == true && p.IsDeleted == false
                            orderby p.ID descending
                            select new ProductStatusItem()
                            { 
                                ID = p.ID,
                                Name = p.Name,
                                NameAscii = p.NameAscii,
                                Number = p.Number
                            };

                result = query.FirstOrDefault();

                this.SetCache(key, result, 10);
            }

            return result;
        }
    }
}
