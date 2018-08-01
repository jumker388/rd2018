using System.Collections.Generic;
using System.IO;
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
    public class GameChargeController : BaseController
    {

        private IGamePlayerRepository gamePlayerRepository;

        // GET: /Admin/ProductType/

        public GameChargeController(IGamePlayerRepository _gamePlayerRepository)
        {
            this.gamePlayerRepository = _gamePlayerRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name = "", int currentRecord = 0, int numberRecord = 10, string type = "", int currentPage = 0)
        {
            var total = 0;
            var list = gamePlayerRepository.GetListCharge(out total,type, name, currentRecord, numberRecord);

            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public string ExportExcel(SearchItem obj)
        {
            var fileName = string.Format("NapTien_{0}.xlsx", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            var filePath = Path.Combine(Request.PhysicalApplicationPath, "File\\ExportImport", fileName);
            var folder = Request.PhysicalApplicationPath + "File\\ExportImport";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var list = gamePlayerRepository.ChargeExportExcel(obj.FromDate, obj.ToDate, obj.Type);

            ExportToExcel(list, filePath);

            return Request.Url.Authority + "/File/ExportImport/" + fileName;
        }
    }
}
