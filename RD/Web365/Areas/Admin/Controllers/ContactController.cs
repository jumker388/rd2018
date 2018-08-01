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
    public class ContactController : BaseController
    {

        private IContactRepository contactRepository;

        // GET: /Admin/ProductType/

        public ContactController(IContactRepository _contactRepository)
        {
            this.contactRepository = _contactRepository;
            this.baseRepository = _contactRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = contactRepository.GetList(out total, name, currentRecord, numberRecord, propertyNameSort, descending);

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
            var obj = new ContactItem();

            var listProductType = contactRepository.GetListForTree<List<ContactItem>>();

            if (id.HasValue)
                obj = contactRepository.GetItemById<ContactItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listProductType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblContact objSubmit)
        {

            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsViewed = false;
                contactRepository.Add(objSubmit);
            }
            else
            {             
                var obj = contactRepository.GetById<tblContact>(objSubmit.ID);
                
                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                contactRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
