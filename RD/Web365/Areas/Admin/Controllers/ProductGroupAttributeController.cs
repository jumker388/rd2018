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
    public class ProductGroupAttributeController : BaseController
    {

        private IProductGroupAttributeRepository productGroupAttributeRepository;
        private IProductTypeRepository productTypeRepository;

        // GET: /Admin/ProductType/

        public ProductGroupAttributeController(IProductGroupAttributeRepository _productGroupAttributeRepository,
            IProductTypeRepository _productTypeRepository)
        {
            this.baseRepository = _productGroupAttributeRepository;
            this.productGroupAttributeRepository = _productGroupAttributeRepository;
            this.productTypeRepository = _productTypeRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = productGroupAttributeRepository.GetList(out total, name, currentRecord, numberRecord, propertyNameSort, descending);

            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetListByProductType(int typeId)
        {
            var list = productGroupAttributeRepository.GetListByProductType(typeId);

            return Json(new
            {
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new ProductGroupAttributeItem();

            var listProductType = productTypeRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = productGroupAttributeRepository.GetItemById<ProductGroupAttributeItem>(id.Value);

            return Json(new
            {
                data = obj,
                listProductType = listProductType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblProductGroupAttribute objSubmit)
        {

            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                productGroupAttributeRepository.Add(objSubmit);
            }
            else
            {
                var obj = productGroupAttributeRepository.GetById<tblProductGroupAttribute>(objSubmit.ID);
                
                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                productGroupAttributeRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
