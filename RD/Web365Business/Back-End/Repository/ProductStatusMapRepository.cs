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
    public class ProductStatusMapRepository : BaseBE<tblProduct_Status_Map>, IProductStatusMapRepository
    {

        public void ResetStatusOfProduct(int productId, int[] statusId)
        {
            DeleteByProduct(productId);
            AddStatusToProduct(productId, statusId);
        }

        public void ResetProductOfProduct(int statusId, int[] productId)
        {
            DeleteByStatus(statusId);
            AddProductToStatus(statusId, productId);
        }

        public tblProduct_Status_Map GetById(int id)
        {
            var query = from p in web365db.tblProduct_Status_Map
                        where p.ID == id
                        orderby p.ID descending
                        select p;
            return query.FirstOrDefault();
        }

        public tblProduct_Status_Map GetByProductAndStatus(int productId, int statusId)
        {
            var query = from p in web365db.tblProduct_Status_Map
                        where p.ProductID == productId && p.ProductStatusID == statusId
                        orderby p.ID descending
                        select p;
            return query.FirstOrDefault();
        }

        public List<tblProduct_Status_Map> GetByProduct(int productId)
        {
            var query = from p in web365db.tblProduct_Status_Map
                        where p.ProductID == productId
                        orderby p.ID descending
                        select p;
            return query.ToList();
        }

        public List<tblProduct_Status_Map> GetByStatus(int statusId)
        {
            var query = from p in web365db.tblProduct_Status_Map
                        where p.ProductStatusID == statusId
                        orderby p.ID descending
                        select p;
            return query.ToList();
        }

        public void Add(tblProduct_Status_Map productStatusMap)
        {
            web365db.tblProduct_Status_Map.Add(productStatusMap);
            web365db.SaveChanges();
        }

        public void AddProductToStatus(int statusId, int[] productId)
        {

            for (int i = 0; i < productId.Length; i++)
            {
                Add(new tblProduct_Status_Map()
                {
                    ProductID = productId[i],
                    ProductStatusID = statusId,
                    DisplayOrder = productId.Length - i
                });
            }
        }

        public void AddStatusToProduct(int productId, int[] statusId)
        {

            for (int i = 0; i < statusId.Length; i++)
            {
                Add(new tblProduct_Status_Map()
                {
                    ProductID = productId,
                    ProductStatusID = statusId[i],
                    DisplayOrder = 1
                });
            }
        }

        public void Update(tblProduct_Status_Map productStatusMap)
        {
            web365db.Entry(productStatusMap).State = EntityState.Modified;
            web365db.SaveChanges();
        }

        public void Delete(int id)
        {
            var productStatusMap = web365db.tblProduct_Status_Map.SingleOrDefault(p => p.ID == id);
            Delete(productStatusMap);
        }

        public void Delete(tblProduct_Status_Map productStatusMap)
        {
            web365db.tblProduct_Status_Map.Remove(productStatusMap);
            web365db.SaveChanges();
        }

        public void Delete(int ProductId, int StatusId)
        {
            web365db.tblProduct_Status_Map.Remove(GetByProductAndStatus(ProductId, StatusId));
            web365db.SaveChanges();
        }

        public void DeleteByProduct(int ProductId)
        {
            var list = GetByProduct(ProductId);
            list.ForEach(i => Delete(i));
        }

        public void DeleteByStatus(int StatusId)
        {
            var list = GetByStatus(StatusId);
            list.ForEach(i => Delete(i));
        }

    }
}
