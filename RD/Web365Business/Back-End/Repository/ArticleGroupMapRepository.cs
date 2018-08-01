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
    public class ArticleGroupMapRepository : BaseBE<tblGroup_Article_Map>, IArticleGroupMapRepository
    {

        public void ResetGroupOfNews(int newsId, int[] groupId)
        {
            DeleteByNews(newsId);
            AddGroupToNews(newsId, groupId);
        }

        public void ResetNewsOfGroup(int groupId, int[] newsId)
        {
            DeleteByGroup(groupId);
            AddNewsToGroup(groupId, newsId);
        }

        public tblGroup_Article_Map GetById(int id)
        {
            var query = from p in web365db.tblGroup_Article_Map
                        where p.ID == id
                        orderby p.ID descending
                        select p;
            return query.FirstOrDefault();
        }

        public tblGroup_Article_Map GetByNewsAndGroup(int newsId, int groupId)
        {
            var query = from p in web365db.tblGroup_Article_Map
                        where p.NewsID == newsId && p.GroupID == groupId
                        orderby p.ID descending
                        select p;
            return query.FirstOrDefault();
        }

        public List<tblGroup_Article_Map> GetByNews(int newsId)
        {
            var query = from p in web365db.tblGroup_Article_Map
                        where p.NewsID == newsId
                        orderby p.ID descending
                        select p;
            return query.ToList();
        }

        public List<tblGroup_Article_Map> GetByGroup(int groupId)
        {
            var query = from p in web365db.tblGroup_Article_Map
                        where p.GroupID == groupId
                        orderby p.ID descending
                        select p;
            return query.ToList();
        }

        public void Add(tblGroup_Article_Map articleGroupMap)
        {
            web365db.tblGroup_Article_Map.Add(articleGroupMap);
            web365db.SaveChanges();
        }

        public void AddNewsToGroup(int groupId, int[] newsId)
        {

            for (int i = 0; i < newsId.Length; i++)
            {
                Add(new tblGroup_Article_Map()
                {
                    NewsID = newsId[i],
                    GroupID = groupId,
                    DisplayOrder = newsId.Length - i
                });
            }
        }

        public void AddGroupToNews(int newsId, int[] groupId)
        {

            for (int i = 0; i < groupId.Length; i++)
            {
                Add(new tblGroup_Article_Map()
                {
                    NewsID = newsId,
                    GroupID = groupId[i],
                    DisplayOrder = 1
                });
            }
        }

        public void Update(tblGroup_Article_Map articleGroupMap)
        {
            web365db.Entry(articleGroupMap).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Delete(int id)
        {
            var articleGroupMap = web365db.tblGroup_Article_Map.SingleOrDefault(p => p.ID == id);
            Delete(articleGroupMap);
        }

        public void Delete(tblGroup_Article_Map articleGroupMap)
        {
            web365db.tblGroup_Article_Map.Remove(articleGroupMap);
            web365db.SaveChanges();
        }

        public void Delete(int newsId, int groupId)
        {
            web365db.tblGroup_Article_Map.Remove(GetByNewsAndGroup(newsId, groupId));
            web365db.SaveChanges();
        }

        public void DeleteByNews(int newsId)
        {
            var list = GetByNews(newsId);
            list.ForEach(i => Delete(i));
        }

        public void DeleteByGroup(int groupId)
        {
            var list = GetByGroup(groupId);
            list.ForEach(i => Delete(i));
        }

    }
}
