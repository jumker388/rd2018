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
    public class UserRoleController : BaseController
    {

        private IUserRoleRepository userRoleRepository;

        // GET: /Admin/ProductType/

        public UserRoleController(IUserRoleRepository _userRoleRepository)
        {
            this.baseRepository = _userRoleRepository;
            this.userRoleRepository = _userRoleRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = userRoleRepository.GetList(out total, name, currentRecord, numberRecord, propertyNameSort, descending);

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
            var obj = new UserRoleItem();

            var list = userRoleRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = userRoleRepository.GetItemById<UserRoleItem>(id.Value);

            return Json(new
            {
                data = obj,
                list = list
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(webpages_Roles objSubmit)
        {
            
            if (objSubmit.RoleId == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                userRoleRepository.Add(objSubmit);
            }
            else
            {
                var obj = userRoleRepository.GetById<webpages_Roles>(objSubmit.RoleId);
                
                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                userRoleRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PageForRole(int roleId, string listPage)
        {
            userRoleRepository.PageForRole(roleId, Web365Utility.Web365Utility.StringToArrayInt(listPage));

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
