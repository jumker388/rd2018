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
    public class LayoutContentRepositoryFE : BaseFE, ILayoutContentRepositoryFE
    {
        public tblLayoutContent GetById(int id)
        {
            var query = from p in web365db.tblLayoutContent
                        where p.ID == id
                        orderby p.ID descending
                        select p;
            return query.FirstOrDefault();
        }

        public LayoutContentItem GetItemById(int id)
        {
            var key = string.Format("LayoutContentRepositoryGetItemById{0}", id);

            var product = new LayoutContentItem();

            if (!this.TryGetCache<LayoutContentItem>(out product, key))
            {
                var result = GetById(id);

                product = new LayoutContentItem()
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
                    Summary = result.Summary,
                    Detail = result.Detail,
                    IsShow = result.IsShow
                };

                this.SetCache(key, product, 10);
            }            

            return product;
        }
    }
}
