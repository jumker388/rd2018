using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Web365Utility;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Domain;
using System;
namespace Web365.Areas.Admin.Controllers
{
    public class ArticleController : BaseController
    {

        private IArticleRepository articleRepository;
        private IArticleGroupRepository articleGroupRepository;
        private IArticleTypeRepository articleTypeRepository;
        private IArticleGroupMapRepository articleGroupMapRepository;

        // GET: /Admin/ProductType/

        public ArticleController(IArticleRepository _articleRepository,
            IArticleGroupRepository _articleGroupRepository,
            IArticleTypeRepository _articleTypeRepository,
            IArticleGroupMapRepository _articleGroupMapRepository)
        {
            this.baseRepository = _articleRepository;
            this.articleRepository = _articleRepository;
            this.articleGroupRepository = _articleGroupRepository;
            this.articleTypeRepository = _articleTypeRepository;
            this.articleGroupMapRepository = _articleGroupMapRepository;
        }

        public ActionResult Index()
        {

            return View();
        }


        [HttpGet]
        public ActionResult GetList(string name, int? typeId, int? groupId, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = articleRepository.GetList(out total, name, typeId, groupId, currentRecord, numberRecord, propertyNameSort, descending);

            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPropertyFilter()
        {
            var listArticleType = articleTypeRepository.GetListForTree<object>();

            var listArticleGroup = articleGroupRepository.GetListForTree<object>();

            return Json(new
            {
                listType = listArticleType,
                listGroup = listArticleGroup
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new ArticleItem();

            var listArticleType = articleTypeRepository.GetListForTree<object>();

            var listArticleGroup = articleGroupRepository.GetListForTree<object>();
            var listRef = new List<ArticleItem>();
            if (id.HasValue)
            {
                obj = articleRepository.GetItemById<ArticleItem>(id.Value);
                listRef = articleRepository.GetByListId(obj.RelatedArticles);
            }
            else
            {
                obj.ListGroupID = new int[0];
            }
            return Json(new
            {
                data = obj,
                listType = listArticleType,
                listGroup = listArticleGroup,
                listRef = listRef
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteMess(int id)
        {


            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblArticle objSubmit)
        {

            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                articleRepository.Add(objSubmit);

            }
            else
            {
                var obj = articleRepository.GetById<tblArticle>(objSubmit.ID);

                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                articleRepository.Update(obj);
            }

            articleGroupMapRepository.ResetGroupOfNews(objSubmit.ID, Web365Utility.Web365Utility.StringToArrayInt(Request["groupID"]));

            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutoConplete(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                var listRef = articleRepository.GetListForTree<object>(title);
                return Json(new
                {
                    listRef = listRef
                }, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
    }
}
