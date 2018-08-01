using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Domain;

namespace Web365Business.Back_End.IRepository
{
    public interface IArticleRepository : IBaseRepository
    {
        List<ArticleItem> GetList(out int total, string name, int? typeId, int? groupId, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool isDelete = false);
        tblArticle GetItemByTitleAscii(string titleAscii);
        List<ArticleItem> GetAll();

        List<ArticleItem> GetByListId(string lstId);

        List<ArticleItem> AutoCompete(string title);

        T GetListForTree<T>(string title);
    }
}
