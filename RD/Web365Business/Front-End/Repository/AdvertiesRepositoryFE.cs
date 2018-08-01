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
    public class AdvertiesRepositoryFE : BaseFE, IAdvertiesRepositoryFE
    {
        public AdvertiesItem GetItemById(int id)
        {
            var key = string.Format("ArticleRepositoryGetItemById{0}", id);

            var advertise = new AdvertiesItem();

            if (!this.TryGetCache<AdvertiesItem>(out advertise, key))
            {
                var result = (from p in web365db.tblAdvertise
                             where p.ID == id && p.IsShow == true && p.IsDeleted == false
                             orderby p.ID descending
                             select p).FirstOrDefault();

                if (result != null)
                {
                    advertise = new AdvertiesItem()
                    {
                        ID = result.ID,
                        DateCreated = result.DateCreated,
                        DateUpdated = result.DateUpdated,
                        Detail = result.Detail,
                        IsShow = result.IsShow,
                        ListPicture = result.tblAdverties_Picture_Map.OrderByDescending(p => p.DisplayOrder).Select(p => new PictureItem()
                        {
                            ID = p.PictureID.Value,
                            Link = p.tblPicture.Link,
                            FileName = p.tblPicture.FileName,
                            Summary = p.tblPicture.Summary

                        }).ToList()
                    };
                }

                this.SetCache(key, advertise, 10);
            }

            return advertise;
        }
    }
}
