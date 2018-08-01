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
    public class ArticleGroupTypeMapRepository : BaseBE<tblGroup_TypeArticle_Map>, IArticleGroupTypeMapRepository
    {

        public void ResetGroupOfType(int TypeId, int[] groupId)
        {
            DeleteByType(TypeId);
            AddGroupToType(TypeId, groupId);
        }

        public void ResetTypeOfGroup(int groupId, int[] TypeId)
        {
            DeleteByGroup(groupId);
            AddTypeToGroup(groupId, TypeId);
        }

        public tblGroup_TypeArticle_Map GetById(int id)
        {
            var query = from p in web365db.tblGroup_TypeArticle_Map
                        where p.ID == id
                        orderby p.ID descending
                        select p;
            return query.FirstOrDefault();
        }

        public tblGroup_TypeArticle_Map GetByTypeAndGroup(int typeId, int groupId)
        {
            var query = from p in web365db.tblGroup_TypeArticle_Map
                        where p.ArticleTypeID == typeId && p.GroupTypeID == groupId
                        orderby p.ID descending
                        select p;
            return query.FirstOrDefault();
        }

        public List<tblGroup_TypeArticle_Map> GetByType(int typeId)
        {
            var query = from p in web365db.tblGroup_TypeArticle_Map
                        where p.ArticleTypeID == typeId
                        orderby p.ID descending
                        select p;
            return query.ToList();
        }

        public List<tblGroup_TypeArticle_Map> GetByGroup(int groupId)
        {
            var query = from p in web365db.tblGroup_TypeArticle_Map
                        where p.GroupTypeID == groupId
                        orderby p.ID descending
                        select p;
            return query.ToList();
        }

        public void Add(tblGroup_TypeArticle_Map articleGroupMap)
        {
            web365db.tblGroup_TypeArticle_Map.Add(articleGroupMap);
            web365db.SaveChanges();
        }

        public void AddTypeToGroup(int groupId, int[] typeId)
        {

            for (int i = 0; i < typeId.Length; i++)
            {
                Add(new tblGroup_TypeArticle_Map()
                {
                    ArticleTypeID = typeId[i],
                    GroupTypeID = groupId,
                    DisplayOrder = typeId.Length - i
                });
            }
        }

        public void AddGroupToType(int typeId, int[] groupId)
        {

            for (int i = 0; i < groupId.Length; i++)
            {
                Add(new tblGroup_TypeArticle_Map()
                {
                    ArticleTypeID = typeId,
                    GroupTypeID = groupId[i],
                    DisplayOrder = 1
                });
            }
        }

        public void Update(tblGroup_TypeArticle_Map articleGroupMap)
        {
            web365db.Entry(articleGroupMap).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Delete(int id)
        {
            var articleGroupMap = web365db.tblGroup_TypeArticle_Map.SingleOrDefault(p => p.ID == id);
            Delete(articleGroupMap);
        }

        public void Delete(tblGroup_TypeArticle_Map articleGroupMap)
        {
            web365db.tblGroup_TypeArticle_Map.Remove(articleGroupMap);
            web365db.SaveChanges();
        }

        public void Delete(int TypeId, int groupId)
        {
            web365db.tblGroup_TypeArticle_Map.Remove(GetByTypeAndGroup(TypeId, groupId));
            web365db.SaveChanges();
        }

        public void DeleteByType(int TypeId)
        {
            var list = GetByType(TypeId);
            list.ForEach(i => Delete(i));
        }

        public void DeleteByGroup(int groupId)
        {
            var list = GetByGroup(groupId);
            list.ForEach(i => Delete(i));
        }

    }
}
