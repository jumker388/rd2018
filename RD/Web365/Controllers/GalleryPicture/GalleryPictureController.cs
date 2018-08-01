using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web365Business.Front_End.IRepository;
using Web365Utility;

namespace Web365.Controllers.GalleryPicture
{
    public class GalleryPictureController : Controller
    {
        //
        // GET: /GalleryPicture/

        IPictureRepositoryFE _pictureRepositoryFe;
        private readonly IAdvertiesRepositoryFE advertiesRepositoryFe;

        public GalleryPictureController(IPictureRepositoryFE pictureRepositoryFe, IAdvertiesRepositoryFE _advertiesRepositoryFe)
        {
            this._pictureRepositoryFe = pictureRepositoryFe;
            this.advertiesRepositoryFe = _advertiesRepositoryFe;
        }

        public ActionResult Index()
        {
            int totalRecord = 0;
            var page = !string.IsNullOrEmpty(Request["trang"]) ? Convert.ToInt32(Request["trang"]) : 1;
            var newsModel = _pictureRepositoryFe.GetListTypeByParentId(out totalRecord, 18, page, ConfigWeb.PageSizeNews);
            ViewBag.RightAds = advertiesRepositoryFe.GetItemById(14);
            ViewBag.totalRecord = totalRecord;
            return View(newsModel);
        }

        public ActionResult AlbumImges(string nameAscii, int id)
        {
            int totalRecord = 0;
            ViewBag.RightAds = advertiesRepositoryFe.GetItemById(14);
            var page = !string.IsNullOrEmpty(Request["trang"]) ? Convert.ToInt32(Request["trang"]) : 1;
            var newsModel = _pictureRepositoryFe.GetListPictureByType(out totalRecord, id, page, 100);
            ViewBag.totalRecord = totalRecord;
            return View(newsModel);
        }

    }
}
