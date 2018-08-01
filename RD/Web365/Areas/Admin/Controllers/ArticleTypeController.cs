using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web365Utility;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Domain;
using System;
namespace Web365.Areas.Admin.Controllers
{
    public class ArticleTypeController : BaseController
    {

        private IArticleTypeRepository articleTypeRepository;
        private IArticleGroupTypeRepository articleGroupTypeRepository;
        private IArticleGroupTypeMapRepository articleGroupTypeMapRepository;

        // GET: /Admin/ProductType/

        public ArticleTypeController(IArticleTypeRepository _articleTypeRepository,
            IArticleGroupTypeRepository _articleGroupTypeRepository,
            IArticleGroupTypeMapRepository _articleGroupTypeMapRepository)
        {
            this.baseRepository = _articleTypeRepository;
            this.articleTypeRepository = _articleTypeRepository;
            this.articleGroupTypeRepository = _articleGroupTypeRepository;
            this.articleGroupTypeMapRepository = _articleGroupTypeMapRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = articleTypeRepository.GetList(out total, name, currentRecord, numberRecord, propertyNameSort, descending);

            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetListOfGroup(int groupId)
        {
            var list = articleTypeRepository.GetListOfGroup(groupId);

            return Json(new
            {
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new ArticleTypeItem();

            var listType = articleTypeRepository.GetListForTree<object>();
            var listGroup = articleGroupTypeRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = articleTypeRepository.GetItemById<ArticleTypeItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listType,
                listGroup = listGroup
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblTypeArticle objSubmit)
        {

            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                articleTypeRepository.Add(objSubmit);
            }
            else
            {
                var obj = articleTypeRepository.GetById<tblTypeArticle>(objSubmit.ID);
                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                articleTypeRepository.Update(obj);
            }

            articleGroupTypeMapRepository.ResetGroupOfType(objSubmit.ID, Web365Utility.Web365Utility.StringToArrayInt(Request["groupID"]));

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
