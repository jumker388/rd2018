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
    public class ReportController : BaseController
    {

        private IGamePlayerRepository gamePlayerRepository;

        // GET: /Admin/ProductType/

        public ReportController(IGamePlayerRepository _gamePlayerRepository)
        {
            this.gamePlayerRepository = _gamePlayerRepository;
        }


        public ActionResult CcuMinute()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetCcuMinute(int gameId = 0, string source = "", string date = "", int page = 0, int pageSize = 5)
        {
            var objFilter = new FilterItem();
            var total = 0;
            objFilter.GameId = gameId;
            objFilter.PageIndex = page;
            objFilter.FromDate = date;
            objFilter.Source = source;
            var list = gamePlayerRepository.GetReportGameOneDay(objFilter);
            var lst0 = list.Day0;
            var lst1 = list.Day1;
            var lst7 = list.Day7;

            var lst0x = new returnCCitem();
            lst0x.label = "ngaydachon";
            lst0x.data = lst0.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var lst1x = new returnCCitem();
            lst1x.label = "truocdo7ngay";
            lst1x.data = lst1.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var lst7x = new returnCCitem();
            lst7x.label = "truocdo7ngay";


            lst7x.data = lst7.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            list.Day0 = lst0x.data;
            list.Day1 = lst1x.data;
            list.Day7 = lst7x.data;
            var datatable = RenderPartialViewToString("~/Areas/Admin/Views/Report/CCuMinuteTable.cshtml", list);

            return Json(new
            {
                ngaydachon = lst0x,
                truocdo7ngay = lst1x,
                truocdo1ngay = lst7x,
                datatable = datatable
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult CcuDay()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetCcuDay(int gameId = 0, string source = "", string fdate = "", string tdate = "", int page = 0, int pageSize = 5)
        {
            var objFilter = new FilterItem();
            var total = 0;
            objFilter.GameId = gameId;
            objFilter.PageIndex = page;
            objFilter.FromDate = fdate;
            objFilter.ToDate = tdate;
            objFilter.Source = source;
            var list = gamePlayerRepository.GetReportGameOneDay(objFilter);
            var lst0 = list.Day0;
            var lst1 = list.Day1;
            var lst7 = list.Day7;

            var lst0x = new returnCCitem();
            lst0x.label = "ngaydachon";
            lst0x.data = lst0.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var lst1x = new returnCCitem();
            lst1x.label = "truocdo7ngay";
            lst1x.data = lst1.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var lst7x = new returnCCitem();
            lst7x.label = "truocdo7ngay";
            lst7x.data = lst7.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Json(new
            {
                ngaydachon = lst0x,
                truocdo7ngay = lst1x,
                truocdo1ngay = lst7x
            },
            JsonRequestBehavior.AllowGet);
        }

    }
}
