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
using OfficeOpenXml;
namespace Web365.Areas.Admin.Controllers
{
    public class PocGameController : BaseController
    {

        private IGamePlayerRepository gamePlayerRepository;


        // GET: /Admin/ProductType/

        public PocGameController(IGamePlayerRepository _gamePlayerRepository)
        {
            this.gamePlayerRepository = _gamePlayerRepository;
        }

        public ActionResult Index()
        {



            return View();
        }

        [HttpGet]
        public ActionResult GetListCcu(string fdate, string tdate, int currentRecord = 0, int numberRecord = 0)
        {
            var objFilter = new FilterItem();

            objFilter.FromDate = fdate;
            objFilter.ToDate = tdate;
            var list = new List<GameReportItem>();
            var totalObj = new GameReportItem();
            if (string.IsNullOrEmpty(tdate))
            {
                list = gamePlayerRepository.GetReportGameCcuOneDay(objFilter);
            }
            else
            {
                list = gamePlayerRepository.GetReportGameCcu(objFilter);
                totalObj.totalAccountRegiter = list.Sum(a => a.totalAccountRegiter.Value);
                totalObj.totalTrans = list.Sum(a => a.totalTrans.Value);
                totalObj.totalCashPlay = list.Sum(a => a.totalCashPlay.Value);
                totalObj.maxccu = list.Sum(a => a.maxccu.Value);
                totalObj.minccu = list.Sum(a => a.minccu.Value);
            }
           
            totalObj.DateCreateString = "Tổng";
            totalObj.avgccu = list.Sum(a => a.avgccu.Value);

            list.Add(totalObj);

            return Json(new
            {
                total = 0,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult GetList(int gameId, string source, string fdate, string tdate, int currentRecord = 0, int numberRecord = 0, string type = "1")
        {
            var objFilter = new FilterItem();
            var total = 0;
            //objFilter.GameId = gameId;
            objFilter.currentRecord = currentRecord;
            objFilter.numberRecord = numberRecord;
            //objFilter.ToDate = tdate;
            var list = new List<GameReportItemNew>();
            //if (type == "1")
            //{
                list = gamePlayerRepository.GetReportGameNew(out total, objFilter);

            //}
            //else
            //{
            //    list = gamePlayerRepository.GetReportGameOneDay(objFilter);
            //}

            //var totalObj = new GameReportItem();

            //totalObj.DateCreateString = "Tổng";
            //totalObj.totalCash = list.Sum(a => a.totalCash.Value);
            //totalObj.totalTax = list.Sum(a => a.totalTax.Value);
            //totalObj.totalTransaction = list.Sum(a => a.totalTransaction.Value);
            //totalObj.totalUser = list.Sum(a => a.totalUser.Value);
            //totalObj.totalAccountRegiter = list.Sum(a => a.totalAccountRegiter.Value);
            //list.Add(totalObj);

            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetListHopThu(string key, string fdate, string tdate, int currentRecord = 0, int numberRecord = 0)
        {
            var objFilter = new FilterItem();

            objFilter.Key = key;
            objFilter.currentRecord = currentRecord;
            objFilter.numberRecord = numberRecord;
            objFilter.FromDate = fdate;
            objFilter.ToDate = tdate;
            int total = 0;
            var list = gamePlayerRepository.GetHopThu(out total, objFilter);
            
            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPropertyFilter()
        {

            var list = gamePlayerRepository.GetAllGame();

            return Json(new
            {
                listType = list
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new ArticleItem();

            var listRef = new List<ArticleItem>();
            if (id.HasValue)
            {
            }
            else
            {
                obj.ListGroupID = new int[0];
            }
            return Json(new
            {
                data = obj,
                listType = "",
                listGroup = "",
                listRef = listRef
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteMess(int id)
        {

            var list = gamePlayerRepository.DeleteOffMessenger(id);

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblArticle objSubmit)
        {

            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;

            }
            else
            {

                objSubmit.DateUpdated = DateTime.Now;
            }


            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult InsertMessByExcel(object data)
        {
            var file = Request["file"];
            var namef = DateTime.Now.Ticks;
            System.IO.File.Move(Server.MapPath(ConfigWeb.TempPath + file), Server.MapPath("~/UploadFile/Files/" + namef + ".xlsx"));

            var fileName = Server.MapPath("~/UploadFile/Files/" + namef + ".xlsx");
            const int startRow = 1;
            var existingFile = new FileInfo(fileName);
            // Open and read the XlSX file.
            using (var package = new ExcelPackage(existingFile))
            {
                // Get the work book in the file
                var workBook = package.Workbook;
                if (workBook != null)
                {
                    if (workBook.Worksheets.Count > 0)
                    {
                        var currentWorksheet = workBook.Worksheets.First();
                        for (int i = startRow + 1; i <= currentWorksheet.Dimension.End.Row; i++)
                        {
                            try
                            {
                                if (currentWorksheet.Cells[i, 2].Value != null)
                                {
                                    var nickname = currentWorksheet.Cells[i, 1].Value.ToString();
                                    var content = currentWorksheet.Cells[i, 2].Value.ToString();

                                    var obj = new HopThuItem();
                                    obj.SendName = User.Identity.Name;
                                    obj.ReceiveName = nickname;
                                    obj.mes = content;

                                    gamePlayerRepository.InsertOffMessenger(obj);

                                }
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                            

                        }
                    }
                }
            }
            return Content(string.Empty);

        }

        [HttpPost]
        public ActionResult SendAllMess(string Content)
        {
            var obj = new HopThuItem();
            obj.SendName = User.Identity.Name;
            obj.mes = Content;

            gamePlayerRepository.SendToMessenger(obj);

            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }

    }
}
