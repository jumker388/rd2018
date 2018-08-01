using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web365Utility;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Domain;

namespace Web365.Areas.Admin.Controllers
{
    public class TypeArticleAutoController : BaseController
    {

        private readonly ITypeArticleAutoRepository _articleAutoRepository;
        private IArticleTypeRepository _articleTypeRepository;

        // GET: /Admin/ProductType/

        public TypeArticleAutoController(ITypeArticleAutoRepository articleAutoRepository, IArticleTypeRepository articleTypeRepository)
        {
            this.baseRepository = articleAutoRepository;
            this._articleAutoRepository = articleAutoRepository;
            this._articleTypeRepository = articleTypeRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int? parentId, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = _articleAutoRepository.GetList(out total, name, parentId, currentRecord, numberRecord, propertyNameSort, descending);

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
            var listType = _articleAutoRepository.GetListForTree<object>();

            return Json(new
            {
                listType = listType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new ArticleAutoItem();

            var listProductType = _articleAutoRepository.GetListForTree<object>();
            var listTypeArticle = _articleTypeRepository.GetListForTree<object>();
            if (id.HasValue)
                obj = _articleAutoRepository.GetItemById<ArticleAutoItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listProductType,
                listTypeArticle = listTypeArticle
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblArticleAuto objSubmit)
        {

            if (objSubmit.ID == 0)
            {
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                _articleAutoRepository.Add(objSubmit);
            }
            else
            {
                var obj = _articleAutoRepository.GetById<tblArticleAuto>(objSubmit.ID);

                UpdateModel(obj);

                _articleAutoRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
