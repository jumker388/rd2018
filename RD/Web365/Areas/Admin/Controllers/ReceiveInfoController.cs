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
    public class ReceiveInfoController : BaseController
    {

        private IReceiveInfoRepository receiveInfoRepository;
        private IReceiveInfoGroupRepository receiveInfoGroupRepository;

        // GET: /Admin/ProductType/

        public ReceiveInfoController(IReceiveInfoRepository _receiveInfoRepository,
            IReceiveInfoGroupRepository _receiveInfoGroupRepository)
        {
            this.receiveInfoRepository = _receiveInfoRepository;
            this.receiveInfoGroupRepository = _receiveInfoGroupRepository;
            this.baseRepository = _receiveInfoRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = receiveInfoRepository.GetList(out total, name, currentRecord, numberRecord, propertyNameSort, descending);

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
            var obj = new ReceiveInfoItem();

            var listGroup = receiveInfoGroupRepository.GetListForTree<List<ReceiveInfoGroupItem>>();

            if (id.HasValue)
                obj = receiveInfoRepository.GetItemById<ReceiveInfoItem>(id.Value);

            return Json(new
            {
                data = obj,
                listGroup = listGroup
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblReceiveInfo objSubmit)
        {

            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                receiveInfoRepository.Add(objSubmit);
            }
            else
            {             
                var obj = receiveInfoRepository.GetById<tblReceiveInfo>(objSubmit.ID);
                
                UpdateModel(obj);

                receiveInfoRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
