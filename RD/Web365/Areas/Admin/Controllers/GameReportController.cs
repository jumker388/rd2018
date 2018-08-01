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
    public class GameReportController : BaseController
    {

        private readonly IGamePlayerRepository _gamePlayerRepository;

        // GET: /Admin/ProductType/

        public GameReportController(IGamePlayerRepository gamePlayerRepository)
        {
            this._gamePlayerRepository = gamePlayerRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList()
        {
            var list = _gamePlayerRepository.ReportOnline();
            return Json(new
            {
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetListPheGame(string fDate, string tDate)
        {
            var list = _gamePlayerRepository.GetPhe(fDate, tDate);
            return Json(new
            {
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetListCash(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                date = DateTime.Now.ToString("yyyy-MM-dd");
            }
            var list = _gamePlayerRepository.GetCash(date);
            return Json(new
            {
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetListGameLogsByGameId(int gameId, int currentRecord, int numberRecord)
        {
            var total = 0;
            var list = _gamePlayerRepository.GetListGameLogs(out total, gameId, currentRecord, numberRecord);

            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditFormLogs(int id)
        {
            var obj = _gamePlayerRepository.GetLogDetail(id);
            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
