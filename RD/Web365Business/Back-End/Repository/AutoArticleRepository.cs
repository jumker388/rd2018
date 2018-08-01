using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Domain;
using Web365Utility;

namespace Web365Business.Back_End.Repository
{
    public class AutoArticleRepository : BaseBE<tblArticle>, IAutoArticleRepository
    {
        /// <summary>
        /// function get all data tblArticle
        /// </summary>
        /// <returns></returns>
        public List<ArticleItem> GetList(out int total, string name, int? typeId, int? groupId, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool isDelete = false)
        {
            var query = from p in web365db.tblArticle
                        where p.IsDeleted == isDelete && !string.IsNullOrEmpty(p.LinkReference)
                        select p;

            if (typeId.HasValue)
            {
                query = query.Where(a => a.TypeID == typeId);
            }

            if (groupId.HasValue)
            {
                query = query.Where(a => a.tblGroup_Article_Map.Any(g => g.GroupID == groupId));
            }

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Select(p => new ArticleItem()
            {
                ID = p.ID,
                Title = p.Title,
                TitleAscii = p.TitleAscii,
                SEOTitle = p.SEOTitle,
                SEODescription = p.SEODescription,
                SEOKeyword = p.SEOKeyword,
                Number = p.Number,
                Summary = p.Summary,
                Detail = p.Detail,
                Tags = p.Tags,
                IsShow = p.IsShow
            }).Skip(currentRecord).Take(numberRecord).ToList();
        }

        public T GetItemById<T>(int id)
        {
            var result = GetById<tblArticle>(id);

            return (T)(object)new ArticleItem()
            {
                ID = result.ID,
                TypeID = result.TypeID,
                Title = result.Title,
                TitleAscii = result.TitleAscii,
                SEOTitle = result.SEOTitle,
                SEODescription = result.SEODescription,
                SEOKeyword = result.SEOKeyword,
                DateCreated = result.DateCreated,
                DateUpdated = result.DateUpdated,
                Number = result.Number,
                PictureID = result.PictureID,
                Summary = result.Summary,
                Detail = result.Detail,
                IsShow = result.IsShow,
                Viewed = result.Viewed,
                Tags = result.Tags,
                ListGroupID = result.tblGroup_Article_Map.Select(g => g.GroupID.Value).ToArray()
            };
        }

        public void Show(int id)
        {
            var article = web365db.tblArticle.SingleOrDefault(p => p.ID == id);
            article.IsShow = true;
            web365db.Entry(article).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var article = web365db.tblArticle.SingleOrDefault(p => p.ID == id);
            article.IsShow = false;
            web365db.Entry(article).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public bool HideAll()
        {
            var article = web365db.tblArticleAuto.ToList();
            foreach (var tblArticle in article)
            {
                tblArticle.IsShow = false;
                tblArticle.IsDeleted = true;
                web365db.Entry(tblArticle).State = EntityState.Modified;
                web365db.SaveChanges();
            }
            return true;
        }

        public List<ArticleAutoItem> GetAllTypeAutoArticle()
        {
            var query = from p in web365db.tblArticleAuto
                        where !string.IsNullOrEmpty(p.Link) && !string.IsNullOrEmpty(p.Xpath) && !string.IsNullOrEmpty(p.XpathTitle) && p.IsShow.HasValue && p.IsShow.Value == true && p.IsDeleted.HasValue && p.IsDeleted.Value == false
                        select p;
            return query.Select(p => new ArticleAutoItem()
            {
                ID = p.ID,
                Name = p.Name,
                Link = p.Link,
                PictureID = p.PictureID.HasValue ? p.PictureID.Value : 0,
                Number = p.Number.HasValue ? p.Number.Value : 0,
                Parent = p.Parent,
                IsShow = p.IsShow.HasValue && p.IsShow.Value,
                IsDeleted = p.IsDeleted.HasValue && p.IsDeleted.Value,
                Xpath = p.Xpath,
                XpathTitle = p.XpathTitle,
                XpathSummary = p.XpathSummary,
                XpathDetail = p.XpathDetail,
                XpathImage = p.XpathImage,
                XpathPaging = p.XpathPaging,
                Xpage = p.Xpage,
                TypeID = p.TypeID,
                ListGroupArticle = p.ListGroupArticle
            }).ToList();
        }

        #region Check
        public bool NameExist(int id, string name)
        {
            var query = web365db.tblArticle.Count(c => c.Title.ToLower() == name.ToLower() && c.tblTypeArticle.LanguageId == LanguageId && c.ID != id);

            return query > 0;
        }
        #endregion

        #region Check
        public int NameExist(string name)
        {
            var query = from p in web365db.tblArticle
                        where p.TitleAscii.ToLower() == name.ToLower()
                        select p;

            return query.FirstOrDefault().ID;
        }
        #endregion

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            var query = from p in web365db.tblArticleAuto
                        where p.IsShow == isShow && p.IsDeleted == isDelete
                        select p;

            return (T)(object)query.OrderByDescending(p => p.ID).Select(p => new ArticleAutoItem()
            {
                ID = p.ID,
                Name = p.Name,
                Link = p.Link,
                PictureID = p.PictureID.HasValue ? p.PictureID.Value : 0,
                Number = p.Number.HasValue ? p.Number.Value : 0,
                Parent = p.Parent,
                IsShow = p.IsShow.HasValue && p.IsShow.Value,
                IsDeleted = p.IsDeleted.HasValue && p.IsDeleted.Value,
                Xpath = p.Xpath,
                XpathTitle = p.XpathTitle,
                XpathSummary = p.XpathSummary,
                XpathDetail = p.XpathDetail,
                XpathImage = p.XpathImage,
                XpathPaging = p.XpathPaging,
                Xpage = p.Xpage,
                ListGroupArticle = p.ListGroupArticle
            }).ToList();
        }

        public ArticleAutoItem GetTypeAutoArticle(int id)
        {
            var query = from p in web365db.tblArticleAuto
                        where p.ID == id
                        select p;
            return query.Select(p => new ArticleAutoItem()
                                         {
                                             ID = p.ID,
                                             Name = p.Name,
                                             Link = p.Link,
                                             PictureID = p.PictureID.HasValue ? p.PictureID.Value : 0,
                                             Number = p.Number.HasValue ? p.Number.Value : 0,
                                             Parent = p.Parent,
                                             IsShow = p.IsShow.HasValue && p.IsShow.Value,
                                             IsDeleted = p.IsDeleted.HasValue && p.IsDeleted.Value,
                                             Xpath = p.Xpath,
                                             XpathTitle = p.XpathTitle,
                                             XpathSummary = p.XpathSummary,
                                             XpathDetail = p.XpathDetail,
                                             XpathPaging = p.XpathPaging,
                                             XpathImage = p.XpathImage,
                                             Xpage = p.Xpage,
                                             ListGroupArticle = p.ListGroupArticle
                                         }).FirstOrDefault();
        }
    }
}
