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
    public class UserPageRepository : BaseBE<tblPage>, IUserPageRepository
    {
        /// <summary>
        /// function get all data tblPage
        /// </summary>
        /// <returns></returns>
        public List<PageItem> GetList(out int total, string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool isDelete = false)
        {
            var query = from p in web365db.tblPage
                        where p.IsDeleted == false
                        where p.Name.ToLower().Contains(name)
                        select p;

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Select(p => new PageItem()
            {
                ID = p.ID,
                Name = p.Name,
                Link = p.Link,
                ClassAtrtibute = p.ClassAtrtibute,
                Number = p.Number,
                Parent = p.Parent,
                HasChild = p.HasChild,
                IsShow = p.IsShow
            }).Skip(currentRecord).Take(numberRecord).ToList();
        }

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            var query = from p in web365db.tblPage
                        where p.IsShow == isShow && p.IsDeleted == p.IsDeleted
                        orderby p.ID ascending
                        select new PageItem()
                        {
                            ID = p.ID,
                            Name = p.Name,
                            Parent = p.Parent
                        };

            return (T)(object)query.ToList();
        }

        public List<PageItem> GetPageOfUser(int userId)
        {
            var query = web365db.Database.SqlQuery<PageItem>("EXEC PRC_GetMenu {0}", userId);

            return query.ToList();
        }

        public T GetItemById<T>(int id)
        {
            var result = GetById<tblPage>(id);

            return (T)(object)new PageItem()
                        {
                            ID = result.ID,
                            Name = result.Name,
                            Link = result.Link,
                            ClassAtrtibute = result.ClassAtrtibute,
                            Parent = result.Parent,
                            HasChild = result.HasChild,
                            IsShow = result.IsShow,
                            Number = result.Number,
                            Detail = result.Detail,
                            CreatedBy = result.CreateBy,
                            UpdatedBy = result.UpdateBy,
                            DateCreated = result.DateCreated,
                            DateUpdated = result.DateUpdated,
                            LinkNew = result.LinkNew
                        };
        }

        public void Show(int id)
        {
            var page = web365db.tblPage.SingleOrDefault(p => p.ID == id);
            page.IsShow = true;
            web365db.Entry(page).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var page = web365db.tblPage.SingleOrDefault(p => p.ID == id);
            page.IsShow = false;
            web365db.Entry(page).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        #region Check
        public bool NameExist(int id, string name)
        {
            var query = web365db.tblPage.Count(c => c.Name.ToLower() == name.ToLower() && c.ID != id);

            return query > 0;
        }
        #endregion
    }
}
