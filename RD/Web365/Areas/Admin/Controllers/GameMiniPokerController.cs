using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Web365Utility;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Domain;
using System;
namespace Web365.Areas.Admin.Controllers
{
    public class GameMiniPokerController : BaseController
    {


        // GET: /Admin/ProductType/
        private IGamePlayerRepository gamePlayerRepository;
        public GameMiniPokerController(IGamePlayerRepository _gamePlayerRepository)
        {
            this.gamePlayerRepository = _gamePlayerRepository;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult GetList()
        {
            var total = 0;
            var list = gamePlayerRepository.GetConfigMiniPoker();

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
            var obj = new ConfigMiniPokerItem();
            if (id.HasValue)
            {
                obj = gamePlayerRepository.GetConfigMiniPokerDetail(id.Value);
            }
            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(ConfigMiniPokerItem objSubmit)
        {
            gamePlayerRepository.UpdateConfigMiniPoker(objSubmit);
            var objLogs = new tblGameConfigLogs
            {
                a = objSubmit.a,
                a1 = objSubmit.a1,
                a2 = objSubmit.a2,
                a3 = objSubmit.a3,
                a4 = objSubmit.a4,
                a5 = objSubmit.a5,
                a6 = objSubmit.a6,
                DateCreared = DateTime.Now,
                CreateBy = User.Identity.Name,
                gameId = 15
            };

            //gamePlayerRepository.AddGameLogs(objLogs);
            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }

    }
}
