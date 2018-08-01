using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Domain;

namespace Web365Business.Back_End.IRepository
{
    public interface IPictureRepository : IBaseRepository
    {
        List<PictureItem> GetList(out int total, string name, string nameFile, int[] typeId, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool? isShow, bool isDelete = false);
        List<PictureItem> GetListByArrayID(int[] arrId, bool isDelete = false);
    }
}
