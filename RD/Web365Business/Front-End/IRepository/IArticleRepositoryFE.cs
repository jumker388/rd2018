using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Domain;
using Web365Models;

namespace Web365Business.Front_End.IRepository
{
    public interface IArticleRepositoryFE
    {
        ArticleItem GetItemByNameAscii(string nameAscii, bool isShow = true, bool isDeleted = false);
        ArticleItem GetItemById(int id);
        List<ArticleItem> GetTopByType(int type, int skip, int top, bool isShow = true, bool isDeleted = false);
        ListArticleModel GetListByType(int typeID, string nameAscii, int currentRecord, int pageSize, bool isShow = true, bool isDeleted = false);
        ListArticleModel GetListByArrType(int[] arrType, int currentRecord, int pageSize, bool isShow = true, bool isDeleted = false);
        ListArticleModel ArticleSeach(string[] keyword, string[] keywordAscii, int currentRecord, int top, bool isShow = true, bool isDeleted = false);
        ListArticleModel GetListByGroup(int groupId, int skip, int top, bool isShow = true, bool isDeleted = false);
        ListArticleModel GetListByTypeAndDetail(int typeId, int skip, int top, bool isShow = true, bool isDeleted = false);
        List<ArticleItem> GetOtherArticle(int type, int articleId, int skip, int top, bool isShow = true, bool isDeleted = false);
        ArticleItem TopOneNewsByType(string type);
        FooterModel FooterInfo(int typeModel, int typeNewId, int typeHotline);
    }
}
