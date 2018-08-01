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
    public class GameManagerController : BaseController
    {

        private IGamePlayerRepository gamePlayerRepository;

        // GET: /Admin/ProductType/

        public GameManagerController(IGamePlayerRepository _gamePlayerRepository)
        {
            this.gamePlayerRepository = _gamePlayerRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList()
        {
            var list = gamePlayerRepository.GetAllGame().Where(c=>c.zoneID != 17);
            return View(list);
        }

        [HttpGet]
        public ActionResult GetGameHistoryByZoneId(int gameId, int currentRecord, int numberRecord)
        {
            var total = 0;
            var total2 = 0;
            var list = gamePlayerRepository.GetGameHistoryByZoneId(out total, gameId, currentRecord, numberRecord);
            var list2 = gamePlayerRepository.GetGiaoDich(out total2, 18, 0, 10, 0);

            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetGiaoDich(int typeId, int currentRecord, int numberRecord, int uid = 0)
        {
            int total;
            var list = gamePlayerRepository.GetGiaoDich(out total, typeId, currentRecord, numberRecord, uid);

            return Json(new
            {
                list = list,
                total = total
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetListRealPlayerInGame(int id)
        {
            int total;
            var list = gamePlayerRepository.RealTimeAccPlayingByGameId(out total, id, 100);

            return Json(new
            {
                list = list,
                total = total
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetTopUserByZoneId(int id)
        {
            var list = gamePlayerRepository.GetTopUserByZoneId(id, 10);

            return Json(new
            {
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new GameItem();

            if (id.HasValue)
            {
                obj = gamePlayerRepository.GetGameItemById(id.Value);
            }
            return Json(new
            {
                data = obj,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(GameItem objSubmit)
        {

            if (objSubmit.zoneID == 0)
            {
            }
            else
            {
                objSubmit.displayStatus = objSubmit.IsShow ? 2 : 1;
                var obj = gamePlayerRepository.UpdateGame(objSubmit.zoneID, objSubmit.name, objSubmit.displayStatus, objSubmit.gameOrder);

                return Json(new
                {
                    Error = false

                }, JsonRequestBehavior.AllowGet);

            }
            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }



    }
}
