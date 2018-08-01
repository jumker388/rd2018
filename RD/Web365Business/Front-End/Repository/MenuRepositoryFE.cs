using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Business.Front_End.IRepository;
using Web365Domain;
using Web365Models;
using Web365Utility;

namespace Web365Business.Front_End.Repository
{
    public class MenuRepositoryFE : BaseFE, IMenuRepositoryFE
    {
        public List<MenuItem> GetListByParent(string parentId, bool isShow = true, bool isDeleted = false)
        {
            var key = string.Format("MenuRepositoryGetListByParent{0}", parentId);

            var list = new List<MenuItem>();

            if (!this.TryGetCache<List<MenuItem>>(out list, key))
            {
                var query = web365db.Database.SqlQuery<MenuItem>("EXEC [dbo].[PRC_MenuByParentId] {0}", string.Join(",", parentId));

                list = query.Select(p => new MenuItem()
                {
                    ID = p.ID,
                    Parent = p.Parent,
                    Name = p.Name,
                    NameAscii = p.NameAscii,
                    CssClass = p.CssClass,
                    Link = p.Link,
                    IsShow = p.IsShow
                }).ToList();

                this.SetCache(key, list, 10);
            }

            return list;
        }
    }
}
