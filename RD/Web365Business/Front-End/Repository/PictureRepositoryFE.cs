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
    public class PictureRepositoryFE : BaseFE, IPictureRepositoryFE
    {
        public List<PictureTypeItem> GetListTypeByParentId(out int total, int parentId, int page, int pagesize)
        {
            total = 0;
            try
            {
                var key = string.Format("GetListTypeByParentId{0}{1}{2}", parentId, page, pagesize);

                var result = (from p in web365db.tblTypePicture
                              where p.Parent.HasValue && p.Parent.Value == parentId && p.IsDeleted == false && p.IsShow == true
                              select p);

                total = result.Count();

                var advertise = result.OrderByDescending(c => c.Number)
                    .ThenByDescending(n => n.ID)
                    .Skip((page - 1) * pagesize)
                    .Take(pagesize)
                    .Select(p => new PictureTypeItem()
                    {
                        ID = p.ID,
                        Name = p.Name,
                        NameAscii = p.NameAscii,
                        Picture = new PictureItem()
                                        {
                                            FileName = p.tblPicture1.FileName,
                                            ID = p.tblPicture1.ID,
                                            Link = p.tblPicture1.Link,
                                        },

                    }).ToList();

                this.SetCache(key, advertise, 10);

                return advertise;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<PictureItem> GetListPictureByType(out int total, int typeId, int page, int pagesize)
        {
            total = 0;
            try
            {
                var key = string.Format("GetListTypeByParentId{0}{1}{2}", typeId, page, pagesize);

                var result = (from p in web365db.tblPicture
                              where p.TypeID.HasValue && p.TypeID.Value == typeId && p.IsShow == true && p.IsDeleted == false
                              orderby p.ID descending
                              select p);

                total = result.Count();

                var advertise = result.Skip((page - 1) * pagesize).Take(pagesize).Select(p => new PictureItem()
                {
                    ID = p.ID,
                    Link = p.Link,
                    FileName = p.FileName,
                    PictureType = new PictureTypeItem()
                                      {
                                          ID = p.tblTypePicture.ID,
                                          Name = p.tblTypePicture.Name,
                                          NameAscii = p.tblTypePicture.NameAscii
                                      }
                }).ToList();

                this.SetCache(key, advertise, 10);

                return advertise;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
