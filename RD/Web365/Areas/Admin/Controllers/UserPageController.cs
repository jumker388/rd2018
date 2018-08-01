using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web365Utility;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Domain;
using System;
using WebMatrix.WebData;
namespace Web365.Areas.Admin.Controllers
{
    public class UserPageController : BaseController
    {

        private IUserPageRepository userPageRepository;

        // GET: /Admin/ProductType/

        public UserPageController(IUserPageRepository _userPageRepository)
        {
            this.userPageRepository = _userPageRepository;
            this.baseRepository = _userPageRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = userPageRepository.GetList(out total, name, currentRecord, numberRecord, propertyNameSort, descending);

            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPageOfUser(int? userId)
        {

            userId = userId.HasValue ? userId.Value : WebSecurity.CurrentUserId;

            var list = userPageRepository.GetPageOfUser(userId.Value);

            return Json(new
            {
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }        

        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new PageItem();

            var listTree= userPageRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = userPageRepository.GetItemById<PageItem>(id.Value);

            return Json(new
            {
                data = obj,
                listTree = listTree
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblPage objSubmit)
        {
            
            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                userPageRepository.Add(objSubmit);
            }
            else
            {
                var obj = userPageRepository.GetById<tblPage>(objSubmit.ID);
                
                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                userPageRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
        
    }
}
