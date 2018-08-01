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
    public class MenuController  : BaseController
    {

        private IMenuRepository menuRepository;

        // GET: /Admin/ProductType/

        public MenuController(IMenuRepository _menuRepository)
        {
            this.baseRepository = _menuRepository;
            this.menuRepository = _menuRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetPropertyFilter()
        {
            var listType = menuRepository.GetListForTree<object>();

            return Json(new
            {
                listType = listType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetList(string name, int? parentId, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = menuRepository.GetList(out total, name, parentId, currentRecord, numberRecord, propertyNameSort, descending);

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
            var obj = new MenuItem();

            var listType = menuRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = menuRepository.GetItemById<MenuItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblMenu objSubmit)
        {
            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.CreatedBy = User.Identity.Name;
                objSubmit.UpdatedBy = User.Identity.Name;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                menuRepository.Add(objSubmit);
            }
            else
            {
                var obj = menuRepository.GetById<tblMenu>(objSubmit.ID);

                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.UpdatedBy = User.Identity.Name;

                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                menuRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
