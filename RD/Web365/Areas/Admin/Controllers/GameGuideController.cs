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
    public class GameGuideController : BaseController
    {

        private IGamePlayerRepository gamePlayerRepository;

        // GET: /Admin/ProductType/

        public GameGuideController(IGamePlayerRepository _gamePlayerRepository)
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
            var list = gamePlayerRepository.GameGuideGetAll();
            return Json(new
            {
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new GameGuideItem();
            obj.game_id = 0;
            obj.id = 0;
            var listGame = gamePlayerRepository.GetAllGame();
            if (id.HasValue)
            {
                obj = gamePlayerRepository.GameGuideGetOne(id.Value);
            }
            return Json(new
            {
                data = obj,
                listGame = listGame
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteGui(int id)
        {

           var obj = gamePlayerRepository.GameGuideDelete(id);
            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(GameGuideItem objSubmit)
        {

            if (objSubmit.id == 0)
            {
                gamePlayerRepository.GameGuideInsert(objSubmit);
            }
            else
            {
                gamePlayerRepository.GameGuideUpdate(objSubmit);
            }

            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }

    }
}
