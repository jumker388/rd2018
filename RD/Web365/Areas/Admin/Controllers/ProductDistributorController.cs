using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Domain;

namespace Web365.Areas.Admin.Controllers
{
    public class ProductDistributorController : BaseController
    {
        private readonly IDistributorRepository distributorRepository;
        //
        // GET: /Admin/ProductDistributor/

        public ProductDistributorController(IDistributorRepository _distributorRepository)
        {
            this.baseRepository = _distributorRepository;
            this.distributorRepository = _distributorRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = distributorRepository.GetList(out total, name, currentRecord, numberRecord, propertyNameSort, descending);

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
            var obj = new ProductDistributorItem();

            var listProductType = distributorRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = distributorRepository.GetItemById<ProductDistributorItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listProductType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblDistributor objSubmit)
        {

            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                distributorRepository.Add(objSubmit);
            }
            else
            {
                var obj = distributorRepository.GetById<tblDistributor>(objSubmit.ID);

                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                distributorRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
