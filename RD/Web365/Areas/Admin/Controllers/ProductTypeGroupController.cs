using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web365Utility;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Domain;
using System;
namespace Web365.Areas.Admin.Controllers
{
    public class ProductTypeGroupController : BaseController
    {

        private IProductTypeGroupRepository productTypeGroupRepository;
        private IProductTypeGroupMapRepository productTypeGroupMapRepository;

        // GET: /Admin/ProductType/

        public ProductTypeGroupController(IProductTypeGroupRepository _productTypeGroupRepository,
            IProductTypeGroupMapRepository _productTypeGroupMapRepository)
        {
            this.baseRepository = _productTypeGroupRepository;
            this.productTypeGroupRepository = _productTypeGroupRepository;
            this.productTypeGroupMapRepository = _productTypeGroupMapRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = productTypeGroupRepository.GetList(out total, name, currentRecord, numberRecord, propertyNameSort, descending);

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
            var obj = new ProductTypeGroupItem();

            var listProductType = productTypeGroupRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = productTypeGroupRepository.GetItemById<ProductTypeGroupItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listProductType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblProductTypeGroup objSubmit)
        {
            
            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                productTypeGroupRepository.Add(objSubmit);
            }
            else
            {
                var obj = productTypeGroupRepository.GetById<tblProductTypeGroup>(objSubmit.ID);
                
                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                productTypeGroupRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddTypeForGroup(int groupId, string typeId)
        {

            productTypeGroupMapRepository.ResetTypeOfGroup(groupId, Web365Utility.Web365Utility.StringToArrayInt(typeId));

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
