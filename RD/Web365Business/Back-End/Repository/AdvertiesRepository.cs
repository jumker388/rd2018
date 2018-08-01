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
    public class AdvertiesRepository : BaseBE<tblAdvertise>, IAdvertiesRepository
    {
        /// <summary>
        /// function get all data tblAdvertise
        /// </summary>
        /// <returns></returns>
        public List<AdvertiesItem> GetList(out int total, string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool isDelete = false)
        {
            var query = from p in web365db.tblAdvertise
                        where p.Name.ToLower().Contains(name) && p.IsDeleted == isDelete
                        select p;

            query = query.Where(p => p.LanguageId == LanguageId);

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Select(p => new AdvertiesItem()
            {
                ID = p.ID,
                Name = p.Name,
                IsShow = p.IsShow
            }).Skip(currentRecord).Take(numberRecord).ToList();
        }

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            var query = from p in web365db.tblAdvertise
                        where p.IsShow == isShow && p.IsDeleted == isDelete
                        select p;

            query = query.Where(p => p.LanguageId == LanguageId);

            return (T)(object)query.OrderByDescending(p => p.ID).Select(p => new AdvertiesItem()
            {
                ID = p.ID,
                Name = p.Name
            }).ToList();
        }        

        public T GetItemById<T>(int id)
        {
            var result = GetById<tblAdvertise>(id);

            return (T)(object)new AdvertiesItem()
            {
                ID = result.ID,
                Name = result.Name,
                DateCreated = result.DateCreated,
                DateUpdated = result.DateUpdated,
                UpdatedBy = result.UpdateBy,
                CreatedBy = result.CreateBy,
                Detail = result.Detail,
                IsShow = result.IsShow,
                ListPictureID = result.tblAdverties_Picture_Map.Select(p => p.PictureID.Value).ToArray()
            };
        }   

        public void Show(int id)
        {
            var adv = web365db.tblAdvertise.SingleOrDefault(p => p.ID == id);
            adv.IsShow = true;
            web365db.Entry(adv).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var adv = web365db.tblAdvertise.SingleOrDefault(p => p.ID == id);
            adv.IsShow = false;
            web365db.Entry(adv).State = EntityState.Modified;
            web365db.SaveChanges();
        }        

        #region Check
        public bool NameExist(int id, string name)
        {
            var query = web365db.tblAdvertise.Count(c => c.Name.ToLower() == name.ToLower() && c.LanguageId == LanguageId && c.ID != id);

            return query > 0;
        }
        #endregion
    }
}
