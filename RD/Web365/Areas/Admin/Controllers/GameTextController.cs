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
    public class GameTextController : BaseController
    {

        private IGamePlayerRepository gamePlayerRepository;

        // GET: /Admin/ProductType/

        public GameTextController(IGamePlayerRepository _gamePlayerRepository)
        {
            this.gamePlayerRepository = _gamePlayerRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string GetText()
        {
            return gamePlayerRepository.GetText();
        }


        [HttpPost]
        [ValidateInput(false)]
        public bool Action(string txt)
        {
            try
            {
                gamePlayerRepository.UpdateText(txt);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

    }
}
