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
    public class ProductTypeController : BaseController
    {

        private IProductTypeRepository productTypeRepository;
        private IProductTypeGroupRepository productTypeGroupRepository;
        private IProductTypeGroupMapRepository productTypeGroupMapRepository;

        // GET: /Admin/ProductType/

        public ProductTypeController(IProductTypeRepository _productTypeRepository,
            IProductTypeGroupRepository _productTypeGroupRepository,
            IProductTypeGroupMapRepository _productTypeGroupMapRepository)
        {
            this.baseRepository = _productTypeRepository;
            this.productTypeRepository = _productTypeRepository;
            this.productTypeGroupRepository = _productTypeGroupRepository;
            this.productTypeGroupMapRepository = _productTypeGroupMapRepository;
            
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int? parentId, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = productTypeRepository.GetList(out total, name, parentId, currentRecord, numberRecord, propertyNameSort, descending);

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

            return Json(new
            {
                listType = listType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetListOfGroup(int groupId)
        {
            var list = productTypeRepository.GetListOfGroup(groupId);

            return Json(new
            {
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new ProductTypeItem();

            var listProductType = productTypeRepository.GetListForTree<object>();
            var listGroup = productTypeGroupRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = productTypeRepository.GetItemById<ProductTypeItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listProductType,
                listGroup = listGroup
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblTypeProduct objSubmit)
        {

            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                productTypeRepository.Add(objSubmit);
            }
            else
            {
                var obj = productTypeRepository.GetById<tblTypeProduct>(objSubmit.ID);
                
                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                productTypeRepository.Update(obj);
            }

            productTypeGroupMapRepository.ResetGroupOfType(objSubmit.ID, Web365Utility.Web365Utility.StringToArrayInt(Request["groupID"]));

            productTypeRepository.ResetListPicture(objSubmit.ID, Request["listPictureId"]);

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
