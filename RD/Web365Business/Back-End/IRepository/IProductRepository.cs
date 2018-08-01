using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Domain;

namespace Web365Business.Back_End.IRepository
{
    public interface IProductRepository : IBaseRepository
    {
        List<ProductItem> GetList(out int total,
            string name,
            string serial,
            int? typeId,
            int? manuId,
            int? distributorId,
            int? statusId,
            int? labelId,
            int currentRecord,
            int numberRecord,
            string propertyNameSort,
            bool descending,
            bool isDelete = false);
        ProductItem GetEditFormFilter(int productId);
        void LabelForProduct(int productId, int[] labelId);
        void FilterForProduct(int productId, int[] filterId);
        void ResetListPicture(int id, string listPictureId);
    }
}
