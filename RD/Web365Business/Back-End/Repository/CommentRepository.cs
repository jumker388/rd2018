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
    public class CommentRepository : BaseBE<tblComment>, ICommentRepository
    {

        /// <summary>
        /// function get all data tblTypeArticle
        /// </summary>
        /// <returns></returns>
        public List<CommentItem> GetList(out int total, string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool isShow = true)
        {
            var query = from p in web365db.tblComment
                        //where p.Name.ToLower().Contains(name) && p.IsShow == isShow
                        select p;

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Select(p => new CommentItem()
            {
                ID = p.ID,
                Name = p.Name,
                Detail = p.Detail,
                Email = p.Email,
                CateJoinID = p.CateJoinID,
                JoinID = p.JoinID,
                IsShow = p.IsShow,
                DateCreated = p.DateCreated,
                CateName = p.tblTypeComment.Name
            }).Skip(currentRecord).Take(numberRecord).ToList();
        }

        public T GetItemById<T>(int id)
        {

            var p = GetById<tblComment>(id);

            return (T)(object)new CommentItem()
            {
                ID = p.ID,
                Name = p.Name,
                Detail = p.Detail,
                Email = p.Email,
                CateJoinID = p.CateJoinID,
                JoinID = p.JoinID,
                IsShow = p.IsShow,
                DateCreated = p.DateCreated,
                CateName = p.tblTypeComment.Name
            };
        }

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            var query = from p in web365db.tblComment
                        where p.IsShow == isShow 
                        select p;


            return (T)(object)query.OrderByDescending(p => p.ID).Select(p => new CommentItem()
            {
                ID = p.ID,
                Name = p.Name,
            }).ToList();
        }

        public void Show(int id)
        {
            var typeArticle = web365db.tblComment.SingleOrDefault(p => p.ID == id);
            typeArticle.IsShow = true;
            web365db.Entry(typeArticle).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var typeArticle = web365db.tblComment.SingleOrDefault(p => p.ID == id);
            typeArticle.IsShow = false;
            web365db.Entry(typeArticle).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        #region Check
        public bool NameExist(int id, string name)
        {
            var query = web365db.tblComment.Count(c => c.Name.ToLower() == name.ToLower() && c.ID != id);

            return query > 0;
        }
        #endregion
    }
}
