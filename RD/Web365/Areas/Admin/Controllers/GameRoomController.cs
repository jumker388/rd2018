using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web365Domain.Game;
using Web365Utility;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Domain;
using System;
namespace Web365.Areas.Admin.Controllers
{
    public class GameRoomController : BaseController
    {

        private IGamePlayerRepository gamePlayerRepository;

        // GET: /Admin/ProductType/

        public GameRoomController(IGamePlayerRepository _gamePlayerRepository)
        {
            this.gamePlayerRepository = _gamePlayerRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(int gameId)
        {
            var total = 0;
            var list = gamePlayerRepository.RoomGetAll(gameId);

            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllGame()
        {
            var listGame = gamePlayerRepository.GetAllGame();

            return Json(new
            {
                listGame = listGame
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new RoomItem();
            var listGame = gamePlayerRepository.GetAllGame();
            if (id.HasValue)
            {
                obj = gamePlayerRepository.GetDetailRoom(id.Value);
            }
            return Json(new
            {
                data = obj,
                listGame = listGame
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(RoomItem objSubmit)
        {

            if (objSubmit.id == 0)
            {
                gamePlayerRepository.RoomInsert(objSubmit);

            }
            else
            {
                gamePlayerRepository.RoomUpdate(objSubmit);
            }


            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }

    }
}

