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
    public class AdvertiesPictureMapRepository : BaseBE<tblAdverties_Picture_Map>, IAdvertiesPictureMapRepository
    {

        public void ResetPictureOfAdverties(int advId, int[] pictureId)
        {
            DeleteByAdv(advId);
            AddPictureToAdv(advId, pictureId);
        }

        public tblAdverties_Picture_Map GetById(int id)
        {
            var query = from p in web365db.tblAdverties_Picture_Map
                        where p.ID == id
                        orderby p.ID descending
                        select p;
            return query.FirstOrDefault();
        }

        public tblAdverties_Picture_Map GetByAdvAndPicture(int advId, int pictureId)
        {
            var query = from p in web365db.tblAdverties_Picture_Map
                        where p.AdvertiesID == advId && p.PictureID == pictureId
                        orderby p.ID descending
                        select p;
            return query.FirstOrDefault();
        }

        public List<tblAdverties_Picture_Map> GetbyAdv(int advId)
        {
            var query = from p in web365db.tblAdverties_Picture_Map
                        where p.AdvertiesID == advId
                        orderby p.ID descending
                        select p;
            return query.ToList();
        }

        public void Add(tblAdverties_Picture_Map advPictureMap)
        {
            web365db.tblAdverties_Picture_Map.Add(advPictureMap);
            web365db.SaveChanges();
        }

        public void AddPictureToAdv(int advId, int[] pictureId)
        {

            for (int i = 0; i < pictureId.Length; i++)
            {
                Add(new tblAdverties_Picture_Map()
                {
                    AdvertiesID = advId,
                    PictureID = pictureId[i],
                    DisplayOrder = pictureId.Length - i
                });
            }
        }

        public void Update(tblAdverties_Picture_Map advPictureMap)
        {
            web365db.Entry(advPictureMap).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Delete(int id)
        {
            var advPictureMap = web365db.tblAdverties_Picture_Map.SingleOrDefault(p => p.ID == id);
            Delete(advPictureMap);
        }

        public void Delete(tblAdverties_Picture_Map advPictureMap)
        {
            web365db.tblAdverties_Picture_Map.Remove(advPictureMap);
            web365db.SaveChanges();
        }

        public void Delete(int advId, int pictureId)
        {
            web365db.tblAdverties_Picture_Map.Remove(GetByAdvAndPicture(advId, pictureId));
            web365db.SaveChanges();
        }

        public void DeleteByAdv(int advId)
        {
            var list = GetbyAdv(advId);
            list.ForEach(i => Delete(i));
        }

    }
}
