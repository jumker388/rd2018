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
    public class ArticleTypeGroupController : BaseController
    {

        private IArticleGroupTypeRepository articleGroupTypeRepository;
        private IArticleGroupTypeMapRepository articleGroupTypeMapRepository;

        // GET: /Admin/ProductType/

        public ArticleTypeGroupController(IArticleGroupTypeRepository _articleGroupTypeRepository,
            IArticleGroupTypeMapRepository _articleGroupTypeMapRepository)
        {
            this.baseRepository = _articleGroupTypeRepository;
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
            var list = articleGroupTypeRepository.GetList(out total, name, currentRecord, numberRecord, propertyNameSort, descending);

            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new ArticleGroupTypeItem();

            var listProductType = articleGroupTypeRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = articleGroupTypeRepository.GetItemById<ArticleGroupTypeItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listProductType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblGroupTypeArticle objSubmit)
        {
            
            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                articleGroupTypeRepository.Add(objSubmit);
            }
            else
            {
                var obj = articleGroupTypeRepository.GetById<tblGroupTypeArticle>(objSubmit.ID);
                
                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                articleGroupTypeRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddTypeForGroup(int groupId, string typeId)
        {

            articleGroupTypeMapRepository.ResetTypeOfGroup(groupId, Web365Utility.Web365Utility.StringToArrayInt(typeId));

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
