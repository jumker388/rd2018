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
    public class UserRepository : BaseBE<UserProfile>, IUserRepository
    {
        /// <summary>
        /// function get all data UserProfile
        /// </summary>
        /// <returns></returns>
        public List<UserProfileItem> GetList(out int total, string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool isDelete = false)
        {
            var query = from p in web365db.UserProfile
                        where p.IsDeleted == false
                        where p.UserName.ToLower().Contains(name)
                        where !p.ShopLevel.HasValue
                        select p;

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Select(p => new UserProfileItem()
            {
                UserId = p.UserId,
                UserName = p.UserName,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Gender = p.Gender,
                Email = p.Email,
                IsDeleted = p.IsDeleted,
                Phone = p.Phone,
                CreateBy = p.CreateBy,
                UpdateBy = p.UpdateBy,
                DateCreated = p.DateCreated,
                DateUpdated = p.DateUpdated,
                IsActive = p.IsActive
            }).Skip(currentRecord).Take(numberRecord).ToList();
        }

        public T GetListForTree<T>(bool isActive = true, bool isDelete = false)
        {
            var query = from p in web365db.UserProfile
                        where p.IsActive == isActive && p.IsDeleted == p.IsDeleted
                        orderby p.UserName ascending
                        select new UserProfileItem()
                        {
                            UserId = p.UserId,
                            UserName = p.UserName
                        };

            return (T)(object)query.ToList();
        }

        public T GetItemById<T>(int id)
        {
            var result = GetById<UserProfile>(id);

            return (T)(object)new UserProfileItem()
            {
                UserId = result.UserId,
                UserName = result.UserName,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Gender = result.Gender,
                Email = result.Email,
                Phone = result.Phone,
                Address = result.Address,
                Note = result.Note,
                CreateBy = result.CreateBy,
                UpdateBy = result.UpdateBy,
                DateCreated = result.DateCreated,
                DateUpdated = result.DateUpdated,
                IsActive = result.IsActive,
                ListRoleId = result.webpages_Roles.Select(r => r.RoleId).ToArray()
            };
        }

        public T GetByUserName<T>(string userName)
        {
            var result = web365db.UserProfile.FirstOrDefault(u => u.UserName == userName);

            return (T)(object)new UserProfileItem()
            {
                UserId = result.UserId,
                UserName = result.UserName,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Gender = result.Gender,
                Email = result.Email,
                Phone = result.Phone,
                Address = result.Address,
                Note = result.Note,
                CreateBy = result.CreateBy,
                UpdateBy = result.UpdateBy,
                DateCreated = result.DateCreated,
                DateUpdated = result.DateUpdated,
                IsActive = result.IsActive,
                ListRoleId = result.webpages_Roles.Select(r => r.RoleId).ToArray()
            };
        }

        public void Show(int id)
        {
            var role = web365db.UserProfile.SingleOrDefault(p => p.UserId == id);
            role.IsActive = true;
            web365db.Entry(role).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var role = web365db.UserProfile.SingleOrDefault(p => p.UserId == id);
            role.IsActive = false;
            web365db.Entry(role).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void RoleForUser(int userId, int[] roleId)
        {
            var user = GetById<UserProfile>(userId);

            user.webpages_Roles.Clear();

            web365db.SaveChanges();

            foreach (var item in roleId)
            {
                if (item > 0)
                {
                    var query = web365db.Database.SqlQuery<object>("EXEC PRC_InsertRoleForUser {0}, {1}", userId, item);

                    query.FirstOrDefault();
                }                
            }                      
        }

        public bool NameExist(int id, string name)
        {
            var query = web365db.UserProfile.Count(c => c.UserName.ToLower() == name.ToLower() && c.UserId != id);

            return query > 0;
        }
    }
}
