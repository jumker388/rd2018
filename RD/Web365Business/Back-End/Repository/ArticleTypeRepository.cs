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
    public class ArticleTypeRepository : BaseBE<tblTypeArticle>, IArticleTypeRepository
    {

        /// <summary>
        /// function get all data tblTypeArticle
        /// </summary>
        /// <returns></returns>
        public List<ArticleTypeItem> GetList(out int total, string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool isDelete = false)
        {
            var query = from p in web365db.tblTypeArticle
                        where p.Name.ToLower().Contains(name) && p.IsDeleted == isDelete
                        select p;

            query = query.Where(p => p.LanguageId == LanguageId);

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Select(p => new ArticleTypeItem()
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
                Parent = p.Parent,
                IsShow = p.IsShow
            }).Skip(currentRecord).Take(numberRecord).ToList();            
        }

        public List<ArticleTypeItem> GetListOfGroup(int groupId, bool isShow = true, bool isDelete = false)
        {
            var query = from p in web365db.tblGroup_TypeArticle_Map
                        where p.GroupTypeID == groupId && p.tblTypeArticle.IsShow == isShow && p.tblTypeArticle.IsDeleted == isDelete
                        select p;

            query = query.Where(p => p.tblTypeArticle.LanguageId == LanguageId);

            return query.OrderByDescending(p => p.DisplayOrder).Select(p => new ArticleTypeItem()
            {
                ID = p.tblTypeArticle.ID,
                Name = p.tblTypeArticle.Name,
                NameAscii = p.tblTypeArticle.NameAscii,
                IsShow = p.tblTypeArticle.IsShow
            }).ToList();
        }

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            var query = from p in web365db.tblTypeArticle
                        where p.IsShow == isShow && p.IsDeleted == isDelete
                        select p;

            query = query.Where(p => p.LanguageId == LanguageId);

            return (T)(object)query.OrderByDescending(p => p.ID).Select(p => new ArticleTypeItem()
                        {
                            ID = p.ID,
                            Name = p.Name,
                            NameAscii = p.NameAscii,
                            Parent = p.Parent
                        }).ToList();
        }

        public T GetItemById<T>(int id)
        {

            var result = GetById<tblTypeArticle>(id);

            return (T)(object)new ArticleTypeItem()
            {
                ID = result.ID,
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
                Parent = result.Parent,
                IsShow = result.IsShow,
                ListGroupID = result.tblGroup_TypeArticle_Map.Select(g => g.GroupTypeID.Value).ToArray()
            };
        }

        public void Show(int id)
        {
            var typeArticle = web365db.tblTypeArticle.SingleOrDefault(p => p.ID == id);
            typeArticle.IsShow = true;
            web365db.Entry(typeArticle).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var typeArticle = web365db.tblTypeArticle.SingleOrDefault(p => p.ID == id);
            typeArticle.IsShow = false;
            web365db.Entry(typeArticle).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        #region Check
        public bool NameExist(int id, string name)
        {
            var query = web365db.tblTypeArticle.Count(c => c.Name.ToLower() == name.ToLower() && c.LanguageId == LanguageId && c.ID != id);

            return query > 0;
        }
        #endregion
    }
}
