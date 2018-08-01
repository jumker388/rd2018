using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Domain;
using Web365Models;

namespace Web365Business.Front_End.IRepository
{
    public interface IProductRepositoryFE
    {
        ProductItem GetItemById(int id);
        ProductItem GetItemByAscii(string ascii);
        ProductModel SearchProduct(string keyword, int skip, int top);
        ProductModel SearchProductAdvance(string asciiType, string asciiFilter, int skip, int top);
        ProductModel GetListByTypeAscii(int skip, int top, string typeAscii);
        ProductModel GetListByGroupAscii(string groupAscii);
        ProductModel GetListByGroupId(int groupId);
    }
}
