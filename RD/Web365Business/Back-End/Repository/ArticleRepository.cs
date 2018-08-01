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
    public class ArticleRepository : BaseBE<tblArticle>, IArticleRepository
    {
        /// <summary>
        /// function get all data tblArticle
        /// </summary>
        /// <returns></returns>
        public List<ArticleItem> GetList(out int total, string name, int? typeId, int? groupId, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool isDelete = false)
        {
            var query = from p in web365db.tblArticle
                        where p.Title.ToLower().Contains(name) && p.IsDeleted == isDelete
                        select p;

            //query = query.Where(p => p.tblTypeArticle.LanguageId == LanguageId);

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
                IsShow = p.IsShow,
                LinkReference = p.LinkReference
            }).Skip(currentRecord).Take(numberRecord).ToList();
        }

        public List<ArticleItem> GetAll()
        {
            var query = from p in web365db.tblArticle
                        where p.IsDeleted == false
                        select p;

            return query.Select(p => new ArticleItem()
            {
                TitleAscii = p.TitleAscii,
            }).ToList();
        }

        public List<ArticleItem> GetByListId(string lstId)
        {
            if (string.IsNullOrEmpty(lstId))
            {
                return new List<ArticleItem>();
            }

            var numbers = lstId.Split(',').Select(Int32.Parse).ToList();
            var query = from p in web365db.tblArticle
                        where p.IsDeleted == false && numbers.Contains(p.ID)
                        select p;

            return query.Select(p => new ArticleItem()
            {
                ID = p.ID,
                Title = p.Title
            }).ToList();
        }

        public List<ArticleItem> AutoCompete(string title)
        {
            var query = from p in web365db.tblArticle
                        where p.IsDeleted == false && p.Title.StartsWith(title)
                        select p;

            return query.Select(p => new ArticleItem()
            {
                ID = p.ID,
                Title = p.Title
            }).Take(10).ToList();
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
                RelatedArticles = result.RelatedArticles,
                LinkReference = result.LinkReference,
                ListGroupID = result.tblGroup_Article_Map.Select(g => g.GroupID.Value).ToArray()
            };
        }

        public tblArticle GetItemByTitleAscii(string titleAscii)
        {
            var query = from p in web365db.tblArticle
                        where p.TitleAscii.ToLower().Equals(titleAscii)
                        select p;

            return query.FirstOrDefault();
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

        #region Check
        public bool NameExist(int id, string name)
        {
            var query = web365db.tblArticle.Count(c => c.Title.ToLower() == name.ToLower() && c.tblTypeArticle.LanguageId == LanguageId && c.ID != id);

            return query > 0;
        }
        #endregion

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            throw new NotImplementedException();
        }

        public T GetListForTree<T>(string title)
        {
            var query = from p in web365db.tblArticle
                        where p.IsDeleted == false && p.Title.Trim().StartsWith(title)
                        orderby p.ID descending
                        select new ArticleItem()
                        {
                            ID = p.ID,
                            Title = p.Title
                        };

            return (T)(object)query.Take(10).ToList();
        }
    }
}
