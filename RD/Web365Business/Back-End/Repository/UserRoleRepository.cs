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
    public class UserRoleRepository : BaseBE<webpages_Roles>, IUserRoleRepository
    {
        /// <summary>
        /// function get all data webpages_Roles
        /// </summary>
        /// <returns></returns>
        public List<UserRoleItem> GetList(out int total, string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool isDelete = false)
        {
            var query = from p in web365db.webpages_Roles
                        where p.IsDeleted == p.IsDeleted
                        where p.RoleName.ToLower().Contains(name)
                        select p;

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Select(p => new UserRoleItem()
            {
                RoleId = p.RoleId,
                RoleName = p.RoleName,
                CreatedBy = p.CreateBy,
                UpdatedBy = p.UpdateBy,
                DateCreated = p.DateCreated,
                DateUpdated = p.DateUpdated,
                IsShow = p.IsShow
            }).Skip(currentRecord).Take(numberRecord).ToList();
        }

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            var query = from p in web365db.webpages_Roles
                        where p.IsShow == isShow && p.IsDeleted == p.IsDeleted
                        orderby p.RoleName ascending
                        select new UserRoleItem()
                        {
                            RoleId = p.RoleId,
                            RoleName = p.RoleName
                        };

            return (T)(object)query.ToList();
        }

        public T GetItemById<T>(int id)
        {
            var result = GetById<webpages_Roles>(id);

            return (T)(object)new UserRoleItem()
            {
                RoleId = result.RoleId,
                RoleName = result.RoleName,
                Detail = result.Detail,
                ListPageId = result.tblPage.Select(p => p.ID).ToArray(),
                CreatedBy = result.CreateBy,
                UpdatedBy = result.UpdateBy,
                DateCreated = result.DateCreated,
                DateUpdated = result.DateUpdated,
                IsShow = result.IsShow
            };
        }

        public void Show(int id)
        {
            var role = web365db.webpages_Roles.SingleOrDefault(p => p.RoleId == id);
            role.IsShow = true;
            web365db.Entry(role).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var role = web365db.webpages_Roles.SingleOrDefault(p => p.RoleId == id);
            role.IsShow = false;
            web365db.Entry(role).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void PageForRole(int roleId, int[] pageId)
        {
            var role = GetById<webpages_Roles>(roleId);

            role.tblPage.Clear();

            web365db.SaveChanges();

            foreach (var item in pageId)
            {
                if (item > 0)
                {
                    var query = web365db.Database.SqlQuery<object>("EXEC PRC_InsertPageForRole {0}, {1}", roleId, item);

                    query.FirstOrDefault();
                }                
            }                      
        }

        public bool NameExist(int id, string name)
        {
            var query = web365db.webpages_Roles.Count(c => c.RoleName.ToLower() == name.ToLower() && c.RoleId != id);

            return query > 0;
        }
    }
}
