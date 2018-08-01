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
    public class OrderController : BaseController
    {

        private IOrderRepository orderRepository;

        // GET: /Admin/ProductType/

        public OrderController(IOrderRepository _orderRepository)
        {
            this.baseRepository = _orderRepository;
            this.orderRepository = _orderRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(int? id, string customerName, string email, string phone, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool? isViewed)
        {
            var total = 0;
            var list = orderRepository.GetList(out total, id, customerName, email, phone, currentRecord, numberRecord, propertyNameSort, descending, isViewed);

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
            var obj = new OrderItem();

            if (id.HasValue)
                obj = orderRepository.GetItemById<OrderItem>(id.Value);

            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblOrder objSubmit)
        {
            
            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsViewed = true;
                orderRepository.Add(objSubmit);
            }
            else
            {
                var obj = orderRepository.GetById<tblOrder>(objSubmit.ID);
                
                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                orderRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ShowAll(int[] listId)
        {
            foreach (var item in listId)
            {
                orderRepository.Show(item);
            }            

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult HideAll(int[] listId)
        {
            foreach (var item in listId)
            {
                orderRepository.Hide(item);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Deleted(string listId)
        {
            var lst =  Web365Utility.Web365Utility.StringToArrayInt(listId);
            foreach (var item in lst)
            {
                orderRepository.Delete(item);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
