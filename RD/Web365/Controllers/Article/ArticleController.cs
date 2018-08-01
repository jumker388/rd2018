using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web365Business.Front_End.IRepository;
using Web365Models;
using Web365Utility;

namespace Web365.Controllers
{
    public class ArticleController : BaseController
    {
        //
        // GET: /Article/

        IArticleRepositoryFE articleRepositoryFE;
        IArticleTypeRepositoryFE articleTypeRepositoryFE;
        private readonly IAdvertiesRepositoryFE advertiesRepositoryFe;

        public ArticleController(IArticleRepositoryFE _articleRepositoryFE, IAdvertiesRepositoryFE _advertiesRepositoryFe,
            IArticleTypeRepositoryFE _articleTypeRepositoryFE)
        {
            this.articleRepositoryFE = _articleRepositoryFE;
            this.advertiesRepositoryFe = _advertiesRepositoryFe;
            this.articleTypeRepositoryFE = _articleTypeRepositoryFE;
        }

        [Cache("Home")]
        public ActionResult Index()
        {
            return View();
        }

        [Cache("ListNewsByType")]
        public ActionResult List(string type)
        {
            var objType = articleTypeRepositoryFE.GetItemByNameAscii(type);

            if (objType.IsSinglePage.HasValue && objType.IsSinglePage.Value)
            {
                var result = articleRepositoryFE.TopOneNewsByType(type);
                return View("~/Views/Article/TopOneNewsByType.cshtml", result);
            }
            var page = !string.IsNullOrEmpty(Request["page"]) ? Web365Utility.Web365Utility.ToInt32(Request["page"]) : 1;
            var list = articleRepositoryFE.GetListByType(0, type, (page - 1) * ConfigWeb.PageSizeNews, ConfigWeb.PageSizeNews);
            return Request.Browser.IsMobileDevice ? View(@"~/Views/Mobile/ListNews.cshtml", list) : View(list);
        }

        [Cache("GroupNewsByType")]
        public ActionResult Group(string group)
        {
            var objGroup = articleTypeRepositoryFE.GetGroupItemByNameAscii(group);
            ViewBag.Group = objGroup;
            var page = !string.IsNullOrEmpty(Request["page"]) ? Web365Utility.Web365Utility.ToInt32(Request["page"]) : 1;
            var list = articleRepositoryFE.GetListByGroup(objGroup.ID, (page - 1) * ConfigWeb.PageSizeNews, ConfigWeb.PageSizeNews);
            return View(list);
        }

        [Cache("Home")]
        public ActionResult Detail(string ascii, int id)
        {
            var result = articleRepositoryFE.GetItemByNameAscii(ascii);
            ViewBag.Ads = advertiesRepositoryFe.GetItemById(14);
            ViewBag.CopyRight = articleRepositoryFE.GetTopByType(3343, 0, 1).FirstOrDefault();
            ViewBag.OtherNews = articleRepositoryFE.GetOtherArticle(result.ArticleType.ID, result.ID, 0, 5);
            return Request.Browser.IsMobileDevice ? View(@"~/Views/Mobile/DetailNews.cshtml", result) : View(result);
        }

        //[Cache("Home")]
        //public ActionResult TopOneNewsByType(string type)
        //{
        //    var result = articleRepositoryFE.TopOneNewsByType(type);
        //    return View(result);
        //}

        #region Ajax

        [HttpGet]
        public ActionResult ListMore(int typeId, int currentRecord)
        {
            try
            {
                var data = articleRepositoryFE.GetListByType(typeId, string.Empty, currentRecord, Web365Utility.ConfigWeb.PageSizeNews);

                var str = RenderPartialViewToString(@"~/Views/Shared/ViewMore.cshtml", data);

                return Json(new
                {
                    error = false,
                    data = str,
                    total = data.total
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Elmah.Error(ex));
            }

            return Json(new
            {
                error = true
            }, JsonRequestBehavior.AllowGet);

        }

        #endregion

    }
}
