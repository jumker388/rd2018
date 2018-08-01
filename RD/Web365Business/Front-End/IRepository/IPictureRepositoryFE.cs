using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Domain;

namespace Web365Business.Front_End.IRepository
{
    public interface IPictureRepositoryFE
    {
        List<PictureTypeItem> GetListTypeByParentId(out int total, int parentId, int page, int pagesize);
        List<PictureItem> GetListPictureByType(out int total, int typeId, int page, int pagesize);
    }
}
