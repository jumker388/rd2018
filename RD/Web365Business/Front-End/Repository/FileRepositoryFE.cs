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
    public class FileRepositoryFE : BaseFE, IFileRepositoryFE
    {
        public List<FileItem> GetListByTypeId(out int total, int type, int page, int pagesize)
        {
            var query = from p in web365db.tblFile
                        where p.TypeID == type && p.IsDeleted == false && p.IsShow == true && p.PictureID.HasValue
                        select p;

            total = query.Count();

            var result =  query.OrderByDescending(c => c.Number)
                    .ThenByDescending(n => n.ID)
                    .Skip((page - 1) * pagesize)
                    .Take(pagesize).Select(p => new FileItem()
            {
                ID = p.ID,
                Name = p.Name,
                NameAscii = p.NameAscii,
                SEOTitle = p.SEOTitle,
                SEODescription = p.SEODescription,
                SEOKeyword = p.SEOKeyword,
                Number = p.Number,
                Summary = p.Summary,
                Detail = p.Detail,
                IsShow = p.IsShow,
                Picture = new PictureItem()
                              {
                                  ID = p.tblPicture.ID,
                                  Link = p.tblPicture.Link,
                                  FileName = p.tblPicture.FileName
                              }
            }).ToList();

            return result;

        }

        public FileItem GetFileById(int id)
        {
            var query = from p in web365db.tblFile
                        where p.ID == id && p.IsDeleted == false && p.IsShow == true
                        select p;

            return query.Select(p => new FileItem()
            {
                ID = p.ID,
                Name = p.Name,
                NameAscii = p.NameAscii,
                SEOTitle = p.SEOTitle,
                SEODescription = p.SEODescription,
                SEOKeyword = p.SEOKeyword,
                Number = p.Number,
                Summary = p.Summary,
                FileName = p.FileName,
                Detail = p.Detail,
                IsShow = p.IsShow,
                DateCreated = p.DateCreated,
                DateUpdated = p.DateUpdated,
                Picture = new PictureItem()
                {
                    ID = p.tblPicture.ID,
                    Link = p.tblPicture.Link,
                    FileName = p.tblPicture.FileName
                }
            }).FirstOrDefault();
        }

        public List<FileItem> GetOtherFile(int type, int current, int top)
        {
            var query = from p in web365db.tblFile
                        where p.TypeID == type && p.IsDeleted == false && p.IsShow == true && p.ID != current
                        select p;

            return query.Select(p => new FileItem()
            {
                ID = p.ID,
                Name = p.Name,
                NameAscii = p.NameAscii,
                SEOTitle = p.SEOTitle,
                SEODescription = p.SEODescription,
                SEOKeyword = p.SEOKeyword,
                Number = p.Number,
                Summary = p.Summary,
                Detail = p.Detail,
                IsShow = p.IsShow
            }).Take(top).ToList();

        }
    }
}
