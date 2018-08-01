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
    public class ReceiveInfoGroupController : BaseController
    {

        private IReceiveInfoGroupRepository receiveInfoGroupRepository;

        // GET: /Admin/ProductType/

        public ReceiveInfoGroupController(IReceiveInfoGroupRepository _receiveInfoGroupRepository)
        {
            this.receiveInfoGroupRepository = _receiveInfoGroupRepository;
            this.baseRepository = _receiveInfoGroupRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = receiveInfoGroupRepository.GetList(out total, name, currentRecord, numberRecord, propertyNameSort, descending);

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
            var obj = new ReceiveInfoGroupItem();

            var listProductType = receiveInfoGroupRepository.GetListForTree<List<ReceiveInfoGroupItem>>();

            if (id.HasValue)
                obj = receiveInfoGroupRepository.GetItemById<ReceiveInfoGroupItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listProductType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblReceiveInfoGroup objSubmit)
        {

            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                receiveInfoGroupRepository.Add(objSubmit);
            }
            else
            {             
                var obj = receiveInfoGroupRepository.GetById<tblReceiveInfoGroup>(objSubmit.ID);
                
                UpdateModel(obj);

                receiveInfoGroupRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
