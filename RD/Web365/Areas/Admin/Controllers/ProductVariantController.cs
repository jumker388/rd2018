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
    public class ProductVariantController : BaseController
    {
        private IProductVariantRepository productVariantRepository;
        private IProductRepository productRepository;

        // GET: /Admin/ProductType/

        public ProductVariantController(IProductVariantRepository _productVariantRepository,
            IProductRepository _productRepository)
        {
            this.baseRepository = _productVariantRepository;
            this.productVariantRepository = _productVariantRepository;
            this.productRepository = _productRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name,
            string code,
            int? productId,
            int currentRecord, 
            int numberRecord, 
            string propertyNameSort,
            bool descending)
        {
            var total = 0;

            var list = productVariantRepository.GetList(out total, name, code, productId, currentRecord, numberRecord, propertyNameSort, descending);

            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPropertyFilter()
        {
            var listProduct = productRepository.GetListForTree<object>();

            return Json(new
            {
                listProduct = listProduct
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new ProductVariantItem();

            var listProduct = productRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = productVariantRepository.GetItemById<ProductVariantItem>(id.Value);

            return Json(new
            {
                data = obj,
                listProduct = listProduct
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblProduct_Variant objSubmit)
        {

            if (objSubmit.ID == 0)
            {                
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.CreatedBy = User.Identity.Name;
                objSubmit.UpdatedBy = User.Identity.Name;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                productVariantRepository.Add(objSubmit);

            }
            else
            {
                var obj = productVariantRepository.GetById<tblProduct_Variant>(objSubmit.ID);
                
                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.UpdatedBy = User.Identity.Name;

                productVariantRepository.Update(obj);
            }

            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }

    }
}
