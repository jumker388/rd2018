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
    public class PictureTypeController : BaseController
    {

        private IPictureTypeRepository pictureTypeRepository;

        // GET: /Admin/ProductType/

        public PictureTypeController(IPictureTypeRepository _pictureTypeRepository)
        {
            this.baseRepository = _pictureTypeRepository;
            this.pictureTypeRepository = _pictureTypeRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = pictureTypeRepository.GetList(out total, name, currentRecord, numberRecord, propertyNameSort, descending);

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
            var obj = new PictureTypeItem();

            var listPictureType = pictureTypeRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = pictureTypeRepository.GetItemById<PictureTypeItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listPictureType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblTypePicture objSubmit)
        {            

            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                pictureTypeRepository.Add(objSubmit);
            }
            else
            {
                var obj = pictureTypeRepository.GetById<tblTypePicture>(objSubmit.ID);
                
                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                pictureTypeRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
