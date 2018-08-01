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
    public class ProductTypeGroupMapRepository : BaseBE<tblProductType_Group_Map>, IProductTypeGroupMapRepository
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

        public tblProductType_Group_Map GetById(int id)
        {
            var query = from p in web365db.tblProductType_Group_Map
                        where p.ID == id
                        orderby p.ID descending
                        select p;
            return query.FirstOrDefault();
        }

        public tblProductType_Group_Map GetByTypeAndGroup(int typeId, int groupId)
        {
            var query = from p in web365db.tblProductType_Group_Map
                        where p.ProductTypeID == typeId && p.GroupTypeID == groupId
                        orderby p.ID descending
                        select p;
            return query.FirstOrDefault();
        }

        public List<tblProductType_Group_Map> GetByType(int typeId)
        {
            var query = from p in web365db.tblProductType_Group_Map
                        where p.ProductTypeID == typeId
                        orderby p.ID descending
                        select p;
            return query.ToList();
        }

        public List<tblProductType_Group_Map> GetByGroup(int groupId)
        {
            var query = from p in web365db.tblProductType_Group_Map
                        where p.GroupTypeID == groupId
                        orderby p.ID descending
                        select p;
            return query.ToList();
        }

        public void Add(tblProductType_Group_Map articleGroupMap)
        {
            web365db.tblProductType_Group_Map.Add(articleGroupMap);
            web365db.SaveChanges();
        }

        public void AddTypeToGroup(int groupId, int[] typeId)
        {

            for (int i = 0; i < typeId.Length; i++)
            {
                Add(new tblProductType_Group_Map()
                {
                    ProductTypeID = typeId[i],
                    GroupTypeID = groupId,
                    DisplayOrder = typeId.Length - i
                });
            }
        }

        public void AddGroupToType(int typeId, int[] groupId)
        {

            for (int i = 0; i < groupId.Length; i++)
            {
                Add(new tblProductType_Group_Map()
                {
                    ProductTypeID = typeId,
                    GroupTypeID = groupId[i],
                    DisplayOrder = 1
                });
            }
        }

        public void Update(tblProductType_Group_Map articleGroupMap)
        {
            web365db.Entry(articleGroupMap).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Delete(int id)
        {
            var articleGroupMap = web365db.tblProductType_Group_Map.SingleOrDefault(p => p.ID == id);
            Delete(articleGroupMap);
        }

        public void Delete(tblProductType_Group_Map articleGroupMap)
        {
            web365db.tblProductType_Group_Map.Remove(articleGroupMap);
            web365db.SaveChanges();
        }

        public void Delete(int TypeId, int groupId)
        {
            web365db.tblProductType_Group_Map.Remove(GetByTypeAndGroup(TypeId, groupId));
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
