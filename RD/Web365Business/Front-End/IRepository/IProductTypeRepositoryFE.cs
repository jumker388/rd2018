using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Domain;

namespace Web365Business.Front_End.IRepository
{
    public interface IProductTypeRepositoryFE
    {
        ProductTypeItem GetItemById(int id);
        ProductTypeItem GetByAsciiAndParent(string ascii, int parentId);
        List<ProductTypeItem> GetListByParent(int? parentId);
        ProductTypeItem GetByParentAsciiAndAscii(string parentAscii, string ascii);
        List<ProductTypeItem> GetAllChildByParent(int? parentId);
        List<ProductTypeItem> GetByGroup(int groupId);
    }
}
