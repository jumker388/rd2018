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
    public class ProductLabelController : BaseController
    {

        private IProductLabelRepository productLabelRepository;

        // GET: /Admin/ProductType/

        public ProductLabelController(IProductLabelRepository _productLabelRepository)
        {
            this.baseRepository = _productLabelRepository;
            this.productLabelRepository = _productLabelRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = productLabelRepository.GetList(out total, name, currentRecord, numberRecord, propertyNameSort, descending);

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
            var obj = new ProductLabelItem();

            var listProductType = productLabelRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = productLabelRepository.GetItemById<ProductLabelItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listProductType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblProductLabel objSubmit)
        {

            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                productLabelRepository.Add(objSubmit);
            }
            else
            {
                var obj = productLabelRepository.GetById<tblProductLabel>(objSubmit.ID);
                
                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                productLabelRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
