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
    public class GameTaiXiuController : BaseController
    {


        // GET: /Admin/ProductType/
        private IGamePlayerRepository gamePlayerRepository;
        public GameTaiXiuController(IGamePlayerRepository _gamePlayerRepository)
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
            var list = gamePlayerRepository.GetConfigTaiXiu();

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
            var obj = new ConfigTaiXiuItem();
            if (id.HasValue & id.Value > 0)
            {
                obj = gamePlayerRepository.GetConfigTaiXiuDetail(id.Value);    
            }
            else
            {
                id = 0;
                obj.HourStart = 0;
                obj.HourEnd = 0;
                obj.MaxBetBot = 0;
                obj.MaxCashBot = 0;
                obj.MinBetBot = 0;
                obj.HanMucAm = 0;
            }
            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(ConfigTaiXiuItem objSubmit)
        {

            if (objSubmit.IsShow == false)
            {
                objSubmit.MaxBetBot = 0;
                objSubmit.MaxCashBot = 0;
                objSubmit.MinBetBot = 0;
                objSubmit.HanMucAm = 0;
            }
            gamePlayerRepository.UpdateConfigTaiXiu(objSubmit);

            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }

    }
}
