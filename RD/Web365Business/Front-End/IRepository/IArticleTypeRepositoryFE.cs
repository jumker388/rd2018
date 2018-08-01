using System;
using System.Collections.Generic;
using Web365Domain;

namespace Web365Business.Front_End.IRepository
{
    public interface IArticleTypeRepositoryFE
    {
        ArticleTypeItem GetItemById(int id);
        ArticleTypeItem GetItemByNameAscii(string nameAscii, bool isShow = true, bool isDeleted = false);
        List<ArticleTypeItem> GetListByGroup(int groupId, bool isShow = true, bool isDeleted = false);
        List<ArticleTypeItem> GetListByParent(int[] parent, bool isShow = true, bool isDeleted = false);
        List<ArticleTypeItem> GetListByParent(int[] parent, int skip, int top, bool isShow = true, bool isDeleted = false);
        List<ArticleTypeItem> GetListByParentAscii(string parentAscii, bool isShow = true, bool isDeleted = false);
        List<ArticleTypeItem> GetAllChildOfTypeWithGroup(int groupId);
        List<ArticleTypeItem> GetAllChildOfTypeAscii(string[] parent, bool isShow = true, bool isDeleted = false);

        List<ArticleTypeItem> GetAllGroupByIsShowHome(bool isHome = false);

        ArticleTypeItem GetGroupItemByNameAscii(string nameAscii, bool isShow = true, bool isDeleted = false);

        List<ArticleGroupItem> GetGroupItemIsShowMenu();
    }
}
