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
    public class ProductAttributeController : BaseController
    {

        private IProductAttributeRepository productAttributeRepository;
        private IProductGroupAttributeRepository productGroupAttributeRepository;        

        // GET: /Admin/ProductType/

        public ProductAttributeController(IProductAttributeRepository _productAttributeRepository,
            IProductGroupAttributeRepository _productGroupAttributeRepository)
        {
            this.baseRepository = _productAttributeRepository;
            this.productAttributeRepository = _productAttributeRepository;
            this.productGroupAttributeRepository = _productGroupAttributeRepository;            
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = productAttributeRepository.GetList(out total, name, currentRecord, numberRecord, propertyNameSort, descending);

            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetListByProductType(int typeId, int productId)
        {
            var list = productAttributeRepository.GetListByProductType(typeId, productId);

            return Json(new
            {
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new ProductAttributeItem();

            var listGroup = productGroupAttributeRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = productAttributeRepository.GetItemById<ProductAttributeItem>(id.Value);

            return Json(new
            {
                data = obj,
                listGroup = listGroup
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblProductAttribute objSubmit)
        {

            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                productAttributeRepository.Add(objSubmit);
            }
            else
            {
                var obj = productAttributeRepository.GetById<tblProductAttribute>(objSubmit.ID);
                
                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                productAttributeRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddProductAttribute(int productId, int attributeId, string value)
        {
            productAttributeRepository.AddProductAttribute(productId, attributeId, value);

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
