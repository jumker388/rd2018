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
    public class CustomerController : BaseController
    {

        private ICustomerRepository customerRepository;

        // GET: /Admin/ProductType/

        public CustomerController(ICustomerRepository _customerRepository)
        {
            this.baseRepository = _customerRepository;
            this.customerRepository = _customerRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = customerRepository.GetList(out total, name, currentRecord, numberRecord, propertyNameSort, descending);

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
            var obj = new CustomerItem();

            var listProductType = customerRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = customerRepository.GetItemById<CustomerItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listProductType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblCustomer objSubmit)
        {

            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsActive = true;
                customerRepository.Add(objSubmit);
            }
            else
            {
                var obj = customerRepository.GetById<tblCustomer>(objSubmit.ID);
                
                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                customerRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendMail(string listEmail, string title, string content)
        {

            foreach (var item in listEmail.Split(';'))
            {
                Web365Utility.Web365Utility.SendMail(item, title, content);
            }

            return Json(new
            {
                error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
