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
    public class GameEventController : BaseController
    {

        private IGamePlayerRepository gamePlayerRepository;

        // GET: /Admin/ProductType/

        public GameEventController(IGamePlayerRepository _gamePlayerRepository)
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
            var list = gamePlayerRepository.GetAllEvent();
            return Json(new
            {
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetListRule()
        {
            var list = gamePlayerRepository.GetGameRuleEvent();
            return Json(new
            {
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditFormRule(int? id)
        {
            var obj = new RuleEventItem();
            obj.GameID = 0;
            if (id.HasValue)
            {
                obj = gamePlayerRepository.GetDetailRuleEvent(id.Value);
            }
            return Json(new
            {
                listGame = gamePlayerRepository.GetAllGame(),
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new AnnouncementItem();
            if (id.HasValue)
            {
                obj = gamePlayerRepository.GetOneEventItem(id.Value);
            }
            return Json(new
            {
                listGame = gamePlayerRepository.GetAllGame(),
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ActionRuleEvent(RuleEventItem objSubmit)
        {

            if (objSubmit.ID == 0)
            {
                gamePlayerRepository.InsertRuleEvent(objSubmit);
            }
            else
            {
                gamePlayerRepository.UpdateRuleEvent(objSubmit);
            }

            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(AnnouncementItem objSubmit)
        {

            if (objSubmit.ID == 0)
            {

                gamePlayerRepository.InsertEvent(objSubmit.Subject, objSubmit.Content, objSubmit.begin_time, objSubmit.end_time, objSubmit.UrlImage, objSubmit.GameID, objSubmit.DoiTuong, objSubmit.ThoiGian, objSubmit.DisplayOrder);

            }
            else
            {
                gamePlayerRepository.UpdateEvent(objSubmit.ID, objSubmit.Subject, objSubmit.Content, objSubmit.begin_time, objSubmit.end_time, objSubmit.UrlImage, objSubmit.GameID, objSubmit.DoiTuong, objSubmit.ThoiGian, objSubmit.DisplayOrder);
            }

            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }
        public bool DeleteRuleEvent(int id)
        {
            try
            {
                gamePlayerRepository.DeleteRuleEvent(id);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public bool DeleteEvent(int id)
        {
            try
            {
                gamePlayerRepository.DeleteEvent(id);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

    }
}
