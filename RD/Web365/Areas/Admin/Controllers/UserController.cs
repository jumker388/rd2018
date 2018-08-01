using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Domain;
using Web365Utility;
using WebMatrix.WebData;

namespace Web365.Areas.Admin.Controllers
{

    public class UserController : BaseController
    {

        private IUserRepository userRepository;

        public UserController(IUserRepository _userRepository)
        {
            this.baseRepository = _userRepository;
            this.userRepository = _userRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetMenu()
        {
            var result = true;

            return Json(new
            {
                result = result,
                message = string.Empty
            },
            JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetList(string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = userRepository.GetList(out total, name, currentRecord, numberRecord, propertyNameSort, descending);

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
            var obj = new UserProfileItem();

            var list = userRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = userRepository.GetItemById<UserProfileItem>(id.Value);

            return Json(new
            {
                data = obj,
                list = list
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(UserProfile objSubmit)
        {

            if (objSubmit.UserId == 0)
            {
                WebSecurity.CreateUserAndAccount(objSubmit.UserName, Request["password"], propertyValues: new
                {
                    FirstName = objSubmit.FirstName,
                    LastName = objSubmit.LastName,
                    Gender = objSubmit.Gender,
                    Email = objSubmit.Email,
                    Phone = objSubmit.Phone,
                    Address = objSubmit.Address,
                    Note = objSubmit.Note,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false
                });
            }
            else
            {
                var obj = userRepository.GetById<UserProfile>(objSubmit.UserId);

                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                userRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RoleForUser(int userId, string roleId)
        {
            userRepository.RoleForUser(userId, Web365Utility.Web365Utility.StringToArrayInt(roleId));

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCurrentUser()
        {
            var obj = userRepository.GetByUserName<object>(User.Identity.Name);

            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
