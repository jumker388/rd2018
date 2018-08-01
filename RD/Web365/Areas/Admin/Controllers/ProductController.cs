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
    public class ProductController : BaseController
    {
        private IProductRepository productRepository;
        private IProductTypeRepository productTypeRepository;
        private IDistributorRepository distributorRepository;
        private IManufacturerRepository manufacturerRepository;
        private IProductStatusRepository productStatusRepository;
        private IProductStatusMapRepository productStatusMapRepository;
        private IProductLabelRepository productLabelRepository;

        // GET: /Admin/ProductType/

        public ProductController(IProductRepository _productRepository,
            IProductTypeRepository _productTypeRepository,
            IDistributorRepository _distributorRepository,
            IManufacturerRepository _manufacturerRepository,
            IProductStatusRepository _productStatusRepository,
            IProductStatusMapRepository _productStatusMapRepository,
            IProductLabelRepository _productLabelRepository)
        {
            this.baseRepository = _productRepository;
            this.productRepository = _productRepository;
            this.productTypeRepository = _productTypeRepository;
            this.distributorRepository = _distributorRepository;
            this.manufacturerRepository = _manufacturerRepository;
            this.productStatusRepository = _productStatusRepository;
            this.productStatusMapRepository = _productStatusMapRepository;
            this.productLabelRepository = _productLabelRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name,
            string serial,
            int? typeId,
            int? manuId,
            int? distributorId,
            int? statusId,
            int? labelId,
            int currentRecord, 
            int numberRecord, 
            string propertyNameSort,
            bool descending)
        {
            var total = 0;

            var list = productRepository.GetList(out total, name, serial, typeId, manuId, distributorId, statusId, labelId, currentRecord, numberRecord, propertyNameSort, descending);

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
            var listType = productTypeRepository.GetListForTree<object>();

            var listDistributor = distributorRepository.GetListForTree<object>();

            var listManufacturer = manufacturerRepository.GetListForTree<object>();

            var listStatus = productStatusRepository.GetListForTree<object>();

            var listLabel = productLabelRepository.GetListForTree<object>();

            return Json(new
            {
                listType = listType,
                listDistributor = listDistributor,
                listManufacturer = listManufacturer,
                listStatus = listStatus,
                listLabel = listLabel
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new ProductItem();

            var listType = productTypeRepository.GetListForTree<object>();

            var listDistributor = distributorRepository.GetListForTree<object>();

            var listManufacturer = manufacturerRepository.GetListForTree<object>();

            var listStatus = productStatusRepository.GetListForTree<object>();

            var listLabel = productLabelRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = productRepository.GetItemById<ProductItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listType,
                listDistributor = listDistributor,
                listManufacturer = listManufacturer,
                listStatus = listStatus,
                listLabel = listLabel
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditFormFilter(int id)
        {
            var obj = new ProductItem();

            obj = productRepository.GetEditFormFilter(id);

            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblProduct objSubmit)
        {

            if (objSubmit.ID == 0)
            {                
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                productRepository.Add(objSubmit);

            }
            else
            {
                var obj = productRepository.GetById<tblProduct>(objSubmit.ID);
                
                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                productRepository.Update(obj);
            }

            productStatusMapRepository.ResetStatusOfProduct(objSubmit.ID, Web365Utility.Web365Utility.StringToArrayInt(Request["statusId"]));
            productRepository.LabelForProduct(objSubmit.ID, Web365Utility.Web365Utility.StringToArrayInt(Request["labelId"]));
            productRepository.ResetListPicture(objSubmit.ID, Request["listPictureId"]);

            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddFilter(int productId, string filterId)
        {
            productRepository.FilterForProduct(productId, Web365Utility.Web365Utility.StringToArrayInt(Request["filterId"]));

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
