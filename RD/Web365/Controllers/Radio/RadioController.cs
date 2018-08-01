using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web365Business.Front_End.IRepository;
using Web365Utility;

namespace Web365.Controllers.GalleryPicture
{
    public class RadioController : Controller
    {
        //
        // GET: /GalleryPicture/

        IFileRepositoryFE _fileRepositoryFe;
        private readonly IAdvertiesRepositoryFE advertiesRepositoryFe;

        public RadioController(IFileRepositoryFE fileRepositoryFe, IAdvertiesRepositoryFE _advertiesRepositoryFe)
        {
            this._fileRepositoryFe = fileRepositoryFe;
            this.advertiesRepositoryFe = _advertiesRepositoryFe;
        }

        public ActionResult Index()
        {
            int totalRecord = 0;
            var page = !string.IsNullOrEmpty(Request["trang"]) ? Convert.ToInt32(Request["trang"]) : 1;
            ViewBag.RightAds = advertiesRepositoryFe.GetItemById(14);
            var newsModel = _fileRepositoryFe.GetListByTypeId(out totalRecord, 1, page, ConfigWeb.PageSizeNews);
            ViewBag.totalRecord = totalRecord;
            return View(newsModel);
        }

        public ActionResult DetailRadio(string nameAscii, int id)
        {
            ViewBag.RightAds = advertiesRepositoryFe.GetItemById(14);
            var radioModel = _fileRepositoryFe.GetFileById(id);
            if (radioModel != null)
            {
                ViewBag.OtherRadio = _fileRepositoryFe.GetOtherFile(1, id, 5);
                return View(radioModel);
            }
            return Redirect("/trang-chu");
        }

    }
}
