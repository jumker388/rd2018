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
    public class GameChangeAwardController : BaseController
    {

        private IGamePlayerRepository gamePlayerRepository;

        // GET: /Admin/ProductType/

        public GameChangeAwardController(IGamePlayerRepository _gamePlayerRepository)
        {
            this.gamePlayerRepository = _gamePlayerRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetListChangeAward(int id = 0, int numberRecord = 10, int type = 0, int currentRecord = 0)
        {
            var total = 0;
            var list = gamePlayerRepository.GetListChangeAward(out total, id, type, currentRecord, numberRecord);

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
            var fileName = string.Format("DoiThuong_{0}.xlsx", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            var filePath = Path.Combine(Request.PhysicalApplicationPath, "File\\ExportImport", fileName);
            var folder = Request.PhysicalApplicationPath + "File\\ExportImport";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var list = gamePlayerRepository.ChangeAwardExportExcel(obj.FromDate, obj.ToDate);

            ExportToExcel(list, filePath);

            return Request.Url.Authority + "/File/ExportImport/" + fileName;
        }


        [HttpPost]
        [ValidateInput(false)]
        public bool Action(long id, bool approval)
        {
            try
            {
                return gamePlayerRepository.Approval(id, approval);
            }
            catch (Exception)
            {
                return false;
            }

        }

    }
}
