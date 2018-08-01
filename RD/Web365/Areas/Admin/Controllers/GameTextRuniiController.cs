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
    public class GameTextRuniiController : BaseController
    {

        private IGamePlayerRepository gamePlayerRepository;

        // GET: /Admin/ProductType/

        public GameTextRuniiController(IGamePlayerRepository _gamePlayerRepository)
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
            var list = gamePlayerRepository.GetAllTextRun();
            return Json(new
            {
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new GameTextItem();
            obj.DataStartstring = DateTime.Now.ToShortDateString();
            obj.DateEndstring = DateTime.Now.ToShortDateString();
            if (id.HasValue)
            {
                obj = gamePlayerRepository.GetOneTextRunItem(id.Value);
                obj.DataStartstring = obj.DataStart.ToShortDateString();
                obj.DateEndstring = obj.DateEnd.ToShortDateString();
            }
            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(GameTextItem objSubmit)
        {

            if (objSubmit.ID == 0)
            {

                gamePlayerRepository.InsertTextRun(objSubmit.Title, objSubmit.Link, objSubmit.DataStart, objSubmit.DateEnd, objSubmit.IsDelete, objSubmit.Order);

            }
            else
            {
                gamePlayerRepository.UpdateTextRun(objSubmit.ID, objSubmit.Title, objSubmit.Link,  objSubmit.DataStart, objSubmit.DateEnd, objSubmit.IsDelete, objSubmit.Order);
            }

            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }
        public bool DeleteTxtR(int id)
        {
            try
            {
                gamePlayerRepository.DeleteTextRun(id);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

    }
}
