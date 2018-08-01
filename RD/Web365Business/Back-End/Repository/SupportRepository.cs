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
    public class SupportRepository : BaseBE<tblSupport>, ISupportRepository
    {
        /// <summary>
        /// function get all data tblSupport
        /// </summary>
        /// <returns></returns>
        public List<SupportItem> GetList(out int total, string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool isDelete = false)
        {
            var query = from p in web365db.tblSupport
                        where p.FullName.ToLower().Contains(name) && p.IsDeleted == isDelete
                        select p;

            query = query.Where(p => p.tblTypeSupport.LanguageId == LanguageId);

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Select(p => new SupportItem()
            {
                ID = p.ID,
                FullName = p.FullName,
                Number = p.Number,   
                Yahoo = p.Yahoo,
                Skype = p.Skype,
                Email = p.Email,
                IsShow = p.IsShow
            }).Skip(currentRecord).Take(numberRecord).ToList();
        }

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            var query = from p in web365db.tblSupport
                        where p.IsShow == isShow && p.IsDeleted == isDelete
                        select p;

            query = query.Where(p => p.tblTypeSupport.LanguageId == LanguageId);

            return (T)(object)query.OrderByDescending(p => p.ID).Select(p => new SupportItem()
            {
                ID = p.ID,
                FullName = p.FullName
            }).ToList();
        }

        public T GetItemById<T>(int id)
        {
            var result = GetById<tblSupport>(id);

            return (T)(object)new SupportItem()
            {
                ID = result.ID,
                FullName = result.FullName,
                Phone = result.Phone,
                Email = result.Email,
                Number = result.Number,
                Yahoo = result.Yahoo,
                Skype = result.Skype,
                DateCreated = result.DateCreated,
                DateUpdated = result.DateUpdated,
                UpdateBy = result.UpdateBy,
                CreateBy = result.CreateBy,
                IsShow = result.IsShow
            };
        }

        public void Show(int id)
        {
            var adv = web365db.tblSupport.SingleOrDefault(p => p.ID == id);
            adv.IsShow = true;
            web365db.Entry(adv).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var adv = web365db.tblSupport.SingleOrDefault(p => p.ID == id);
            adv.IsShow = false;
            web365db.Entry(adv).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public bool NameExist(int id, string name)
        {
            throw new NotImplementedException();
        }

        public void AddGoogleMess(tblGoogleMessage entity)
        {
            web365db.tblGoogleMessage.Add(entity);
            web365db.SaveChanges();
        }

        public List<GoogleMessageItem> GetListMessGoogle(out int total, string name, int currentRecord, int numberRecord)
        {
            var query = from p in web365db.tblGoogleMessage
                        where p.Title.ToLower().Contains(name) || p.ContentText.ToLower().Contains(name)
                        select p;

            total = query.Count();

            query = query.OrderByDescending("ID");

            return query.Select(p => new GoogleMessageItem()
            {
                ID = p.ID,
                Title = p.Title,
                ContentText = p.ContentText,
                GoogleId = p.GoogleId,
                DateCreated = p.DateCreated
            }).Skip(currentRecord).Take(numberRecord).ToList();
        }
    }
}
