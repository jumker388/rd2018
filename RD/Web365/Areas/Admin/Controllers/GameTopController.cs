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
    public class GameTopController : BaseController
    {

        private IGamePlayerRepository gamePlayerRepository;

        // GET: /Admin/ProductType/

        public GameTopController(IGamePlayerRepository _gamePlayerRepository)
        {
            this.gamePlayerRepository = _gamePlayerRepository;
        }

        [HttpGet]
        public ActionResult GetTopGem()
        {
            var list = gamePlayerRepository.GetTopGem();

            return Json(new
            {
                list = list,
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetTopXu()
        {
            var list = gamePlayerRepository.GetTopXu();

            return Json(new
            {
                list = list,
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetTopExp()
        {
            var list = gamePlayerRepository.GetTopExp();

            return Json(new
            {
                list = list,
            },
            JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetTopNapTien()
        {
            var list = gamePlayerRepository.GetTopNapTien();

            return Json(new
            {
                list = list,
            },
            JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetDoiThuong()
        {
            var list = gamePlayerRepository.GetTopDoiThuong();

            return Json(new
            {
                list = list,
            },
            JsonRequestBehavior.AllowGet);
        }


    }
}
