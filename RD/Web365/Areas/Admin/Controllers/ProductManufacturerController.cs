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
    public class ProductManufacturerController : BaseController
    {
        private readonly IManufacturerRepository manufacturerRepository;
        //
        // GET: /Admin/ProductManufacturer/
        public ProductManufacturerController(IManufacturerRepository _manufacturerRepository)
        {
            this.baseRepository = _manufacturerRepository;
            this.manufacturerRepository = _manufacturerRepository;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = manufacturerRepository.GetList(out total, name, currentRecord, numberRecord, propertyNameSort, descending);

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
            var obj = new ProductManufacturerItem();

            var listProductType = manufacturerRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = manufacturerRepository.GetItemById<ProductManufacturerItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listProductType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblManufacturer objSubmit)
        {

            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                manufacturerRepository.Add(objSubmit);
            }
            else
            {
                var obj = manufacturerRepository.GetById<tblManufacturer>(objSubmit.ID);

                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                manufacturerRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
