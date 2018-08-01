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
    public class CardOptionController : BaseController
    {

        private IDaiLyRepository daiLyRepository;
        // GET: /Admin/ProductType/

        public CardOptionController(IDaiLyRepository _daiLyRepository)
        {
            this.daiLyRepository = _daiLyRepository;
        }

        public ActionResult Index()
        {

            return View();
        }


        [HttpGet]
        public ActionResult GetList(int currentPage, int currentRecord, int numberRecord)
        {
            var total = 0;
            var list = daiLyRepository.GetListCardOptions(out total, currentRecord, numberRecord);

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
            var obj = new CardOptionItem();

            if (id.HasValue)
            {
                obj = daiLyRepository.GetCardOptionDetail(id.Value);
            }
            else
            {
            }
            return Json(new
            {
                data = obj,
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateIsDel(int listId)
        {
            daiLyRepository.UpdateIsDel(listId, 1);
            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Hide(int listId)
        {
            daiLyRepository.UpdateIsShow(listId, 0);
            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Show(int listId)
        {
            daiLyRepository.UpdateIsShow(listId, 1);
            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(CardOptionItem objSubmit)
        {

            if (objSubmit.id == 0)
            {
                objSubmit.isDeleted = 0;
                objSubmit.isShow = Convert.ToInt32(objSubmit.IsShowd);
                daiLyRepository.ActionCardOption(objSubmit);

            }
            else
            {
                objSubmit.isShow = Convert.ToInt32(objSubmit.IsShowd);
                daiLyRepository.ActionCardOption(objSubmit);
            }


            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }
    }
}
