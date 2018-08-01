using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Domain;
using Web365Utility;

namespace Web365Business.Back_End.Repository
{
    /// <summary>
    ///  05-01-2013
    /// </summary>
    public class PictureRepository : BaseBE<tblPicture>, IPictureRepository
    {
        /// <summary>
        /// function get all data tblTypeProduct
        /// </summary>
        /// <returns></returns>
        public List<PictureItem> GetList(out int total, 
            string name, 
            string nameFile, 
            int[] typeId,
            int currentRecord, 
            int numberRecord, 
            string propertyNameSort,
            bool descending, 
            bool? isShow,
            bool isDelete = false)
        {
            var query = from p in web365db.tblPicture
                        where p.Name.ToLower().Contains(name) && p.FileName.ToLower().Contains(nameFile) && p.IsDeleted == isDelete                        
                        select p;

            if (typeId != null && typeId.Count() > 0)
            {
                query = query.Where(p => typeId.Contains(p.TypeID.Value));
            }

            if(isShow.HasValue){
                query = query.Where(p => p.IsShow == isShow);
            }

            total = query.Count();

            query = descending ? QueryableHelper.OrderByDescending(query, propertyNameSort) : QueryableHelper.OrderBy(query, propertyNameSort);

            return query.Select(p => new PictureItem()
            {
                ID = p.ID,
                Name = p.Name,
                FileName = p.FileName,
                TypeID = p.TypeID,
                Summary = p.Summary,
                Alt = p.Alt,
                Link = p.Link,
                Size = p.Size,
                DateCreated = p.DateCreated,
                DateUpdated = p.DateUpdated,
                CreatedBy = p.CreatedBy,
                UpdatedBy = p.UpdatedBy,
                IsShow = p.IsShow,
                IsDeleted = p.IsDeleted
            }).Skip(currentRecord).Take(numberRecord).ToList();
        }

        public List<PictureItem> GetListByArrayID(int[] arrId, bool isDelete = false)
        {
            var result = new List<PictureItem>();

            var query = from p in web365db.tblPicture
                        where arrId.Contains(p.ID) && p.IsDeleted == isDelete
                        orderby p.ID descending
                        select new PictureItem()
                        {
                            ID = p.ID,
                            Name = p.Name,
                            FileName = p.FileName,
                            TypeID = p.TypeID,
                            Summary = p.Summary,
                            Link = p.Link,
                            Alt = p.Alt,
                            Size = p.Size,
                            DateCreated = p.DateCreated,
                            DateUpdated = p.DateUpdated,
                            CreatedBy = p.CreatedBy,
                            UpdatedBy = p.UpdatedBy,
                            IsShow = p.IsShow,
                            IsDeleted = p.IsDeleted
                        };

            result = query.ToList();

            result = arrId.Select(item => result.FirstOrDefault(p => p.ID == item)).Where(product => product != null).ToList();

            return result;
        }

        /// <summary>
        /// get product type item
        /// </summary>
        /// <param name="id">id of product type</param>
        /// <returns></returns>
        public T GetItemById<T>(int id)
        {
            var query = from p in web365db.tblPicture
                        where p.ID == id
                        orderby p.ID descending
                        select new PictureItem()
                        {
                            ID = p.ID,
                            Name = p.Name,
                            FileName = p.FileName,
                            TypeID = p.TypeID,
                            Summary = p.Summary,
                            Link = p.Link,
                            Alt = p.Alt,
                            Size = p.Size,
                            DateCreated = p.DateCreated,
                            DateUpdated = p.DateUpdated,
                            CreatedBy = p.CreatedBy,
                            UpdatedBy = p.UpdatedBy,
                            IsShow = p.IsShow,
                            IsDeleted = p.IsDeleted
                        };
            return (T)(object)query.FirstOrDefault();
        }

        #region Insert, Edit, Delete, Save

        public void Show(int id)
        {
            var picture = web365db.tblPicture.SingleOrDefault(p => p.ID == id);
            picture.IsShow = true;
            web365db.Entry(picture).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Hide(int id)
        {
            var picture = web365db.tblPicture.SingleOrDefault(p => p.ID == id);
            picture.IsShow = false;
            web365db.Entry(picture).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        #endregion

        public T GetListForTree<T>(bool isShow = true, bool isDelete = false)
        {
            throw new NotImplementedException();
        }

        public bool NameExist(int id, string name)
        {
            throw new NotImplementedException();
        }
    }
}
