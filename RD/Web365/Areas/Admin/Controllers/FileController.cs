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
    public class FileController : BaseController
    {

        private IFileRepository fileRepository;
        private IFileTypeRepository fileTypeRepository;

        // GET: /Admin/ProductType/

        public FileController(IFileRepository _fileRepository,
            IFileTypeRepository _fileTypeRepository)
        {
            this.baseRepository = _fileRepository;
            this.fileRepository = _fileRepository;
            this.fileTypeRepository = _fileTypeRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = fileRepository.GetList(out total, name, currentRecord, numberRecord, propertyNameSort, descending);

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
            var obj = new FileItem();

            var listProductType = fileTypeRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = fileRepository.GetItemById<FileItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listProductType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblFile objSubmit)
        {

            if (System.IO.File.Exists(Server.MapPath(ConfigWeb.TempPath + objSubmit.FileName)))
            {
                objSubmit.Size = new System.IO.FileInfo(Server.MapPath(ConfigWeb.TempPath + objSubmit.FileName)).Length;

                FileUtility.MoveFile(StaticEnum.FileType.File, objSubmit.FileName);
            }

            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                fileRepository.Add(objSubmit);
            }
            else
            {
                var obj = fileRepository.GetById<tblFile>(objSubmit.ID);
                
                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                fileRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
