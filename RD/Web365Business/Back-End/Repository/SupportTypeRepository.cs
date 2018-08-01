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
    public class SupportTypeRepository : BaseBE<tblTypeSupport>, ISupportTypeRepository
    {
        /// <summary>
        /// function get all data tblTypeSupport
        /// </summary>
        /// <returns></returns>
        public List<SupportTypeItem> GetList(out int total, string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool isDelete = false)
        {
            var query = from p in web365db.tblTypeSupport
                        where p.Name.ToLower().Contains(name) && p.IsDeleted == isDelete
                        select p;

            query = query.Where(p => p.LanguageId == LanguageId);

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Select(p => new SupportTypeItem()
            {
                ID = p.ID,
                Name = p.Name,
                Number = p.Number,                
                IsShow = p.IsShow
            }).Skip(currentRecord).Take(numberRecord).ToList();
        }

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            var query = from p in web365db.tblTypeSupport
                        where p.IsShow == isShow && p.IsDeleted == isDelete
                        select p;

            query = query.Where(p => p.LanguageId == LanguageId);

            return (T)(object)query.OrderByDescending(p => p.ID).Select(p => new SupportTypeItem()
            {
                ID = p.ID,
                Name = p.Name
            }).ToList();
        }

        public T GetItemById<T>(int id)
        {
            var result = GetById<tblTypeSupport>(id);

            return (T)(object)new SupportTypeItem()
            {
                ID = result.ID,
                Name = result.Name,
                Number = result.Number,
                DateCreated = result.DateCreated,
                DateUpdated = result.DateUpdated,
                UpdateBy = result.UpdateBy,
                CreateBy = result.CreateBy,
                Detail = result.Detail,
                IsShow = result.IsShow
            };
        }

        public void Show(int id)
        {
            var adv = web365db.tblTypeSupport.SingleOrDefault(p => p.ID == id);
            adv.IsShow = true;
            web365db.Entry(adv).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var adv = web365db.tblTypeSupport.SingleOrDefault(p => p.ID == id);
            adv.IsShow = false;
            web365db.Entry(adv).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public bool NameExist(int id, string name)
        {
            var query = web365db.tblTypeSupport.Count(c => c.Name.ToLower() == name.ToLower() && c.LanguageId == LanguageId && c.ID != id);

            return query > 0;
        }
    }
}
