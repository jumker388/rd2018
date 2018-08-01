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
    public class PictureController : BaseController
    {

        private IPictureRepository pictureRepository;
        private IPictureTypeRepository pictureTypeRepository;

        // GET: /Admin/ProductType/

        public PictureController(IPictureRepository _pictureRepository,
            IPictureTypeRepository _pictureTypeRepository)
        {
            this.baseRepository = _pictureRepository;
            this.pictureRepository = _pictureRepository;
            this.pictureTypeRepository = _pictureTypeRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, string fileName, int[] typeId, int currentRecord, int numberRecord, string propertyNameSort, bool descending, bool? isShow)
        {
            var total = 0;
            var list = pictureRepository.GetList(out total, name, fileName, typeId, currentRecord, numberRecord, propertyNameSort, descending, isShow);

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
            var listType = pictureTypeRepository.GetListForTree<object>();

            return Json(new
            {
                listType = listType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetListByArrayID(int[] arrID)
        {
            var list = pictureRepository.GetListByArrayID(arrID);

            return Json(new
            {
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new PictureItem();

            var listPictureType = pictureTypeRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = pictureRepository.GetItemById<PictureItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listPictureType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblPicture picture)
        {

            if (System.IO.File.Exists(Server.MapPath(ConfigWeb.TempPath + picture.FileName)))
            {
                picture.Size = new System.IO.FileInfo(Server.MapPath(ConfigWeb.TempPath + picture.FileName)).Length;

                FileUtility.MoveFile(StaticEnum.FileType.Image, picture.FileName);
            }

            if (picture.ID == 0)
            {
                picture.CreatedBy = string.Empty;
                picture.UpdatedBy = string.Empty;
                picture.DateCreated = DateTime.Now;
                picture.DateUpdated = DateTime.Now;
                picture.IsShow = true;
                picture.IsDeleted = false;
                pictureRepository.Add(picture);
            }
            else
            {
                picture.UpdatedBy = string.Empty;

                var obj = pictureRepository.GetById<tblPicture>(picture.ID);
                
                UpdateModel(obj);

                picture.DateUpdated = DateTime.Now;

                pictureRepository.Update(obj);
            }            

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
