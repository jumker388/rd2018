using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using Web365Utility;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Domain;
using System;
namespace Web365.Areas.Admin.Controllers
{
    public class GamePaymentController : BaseController
    {

        private IGamePlayerRepository gamePlayerRepository;

        // GET: /Admin/ProductType/

        public GamePaymentController(IGamePlayerRepository _gamePlayerRepository)
        {
            this.gamePlayerRepository = _gamePlayerRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(int numberRecord, int currentPage, int uid = 0, string username = "", string date = "", string status = "")
        {
            var total = 0;
            var list = gamePlayerRepository.GetPayment(out total, (currentPage - 1) * numberRecord, numberRecord, uid, username, date, status);

            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DetailPay(int id)
        {
            var obj = gamePlayerRepository.PaymentDetail(id);
            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }



    }
}
