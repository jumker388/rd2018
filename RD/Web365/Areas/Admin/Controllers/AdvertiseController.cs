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
    public class AdvertiseController : BaseController
    {

        private IAdvertiesRepository advertiesRepository;
        private IAdvertiesPictureMapRepository advertiesPictureMapRepository;

        // GET: /Admin/ProductType/

        public AdvertiseController(IAdvertiesRepository _advertiesRepository,
            IAdvertiesPictureMapRepository _advertiesPictureMapRepository)
        {
            this.baseRepository = _advertiesRepository;
            this.advertiesRepository = _advertiesRepository;
            this.advertiesPictureMapRepository = _advertiesPictureMapRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = advertiesRepository.GetList(out total, name, currentRecord, numberRecord, propertyNameSort, descending);

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
            var obj = new AdvertiesItem();

            var listProductType = advertiesRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = advertiesRepository.GetItemById<AdvertiesItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listProductType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblAdvertise objSubmit)
        {
            
            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                advertiesRepository.Add(objSubmit);
            }
            else
            {
                var obj = advertiesRepository.GetById<tblAdvertise>(objSubmit.ID);
                
                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                advertiesRepository.Update(obj);
            }

            advertiesPictureMapRepository.ResetPictureOfAdverties(objSubmit.ID, Web365Utility.Web365Utility.StringToArrayInt(Request["listPictureId"]));

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
