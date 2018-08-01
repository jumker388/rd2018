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
    public class GameDaiLyController : BaseController
    {

        private IDaiLyRepository dailyRepository;

        // GET: /Admin/ProductType/

        public GameDaiLyController(IDaiLyRepository _dailyRepository)
        {
            this.dailyRepository = _dailyRepository;
        }

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetDailyCap1(int currentPage = 0, int numberRecord = 0, int currentRecord = 0, string name = "", string fDate = "", string tDate = "", string propertyNameSort = "", bool descending = true)
        {

            var list = dailyRepository.GetDaiLyCap1(currentPage, numberRecord, fDate, tDate, name, propertyNameSort, descending);
            var totalObj = new UserInfoNew3();
            totalObj.gameCashN = list.Sum(c => c.gameCashN);
            totalObj.CashKet = list.Sum(c => c.CashKet);
            totalObj.totalSendToUser = list.Sum(c => c.totalSendToUser);
            totalObj.totalRecivefromUser = list.Sum(c => c.totalRecivefromUser);
            totalObj.totalTax = list.Sum(c => c.totalTax);

            totalObj.totalSendToUserCap1 = list.Sum(c => c.totalSendToUserCap1);
            totalObj.totalRecivefromUserCap1 = list.Sum(c => c.totalRecivefromUserCap1);
            totalObj.totalTaxCap1 = list.Sum(c => c.totalTaxCap1);

            list.Add(totalObj);
            return Json(new
            {
                list = list,
                total = list.Count
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDailyCap2(int currentPage = 0, int numberRecord = 0, int currentRecord = 0, string name = "", string fDate = "", string tDate = "", string key = "", string propertyNameSort = "", bool descending = true)
        {
            if (name == "0")
            {
                name = "";
            }
            var list = dailyRepository.GetDaiLyCap2(name, currentPage, numberRecord, fDate, tDate, key, propertyNameSort, descending);
            var totalObj = new UserInfoNew2();
            totalObj.gameCashN = list.Sum(c => c.gameCashN);
            totalObj.CashKet = list.Sum(c => c.CashKet);
            totalObj.totalSendToUser = list.Sum(c => c.totalSendToUser);
            totalObj.totalRecivefromUser = list.Sum(c => c.totalRecivefromUser);
            totalObj.totalTax = list.Sum(c => c.totalTax);
            list.Add(totalObj);
            return Json(new
            {
                list = list,
                total = list.Count
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetList(string name, int currentRecord, int numberRecord, int level = 1)
        {
            var total = 0;
            var obj = new FilterItem();
            obj.currentRecord = currentRecord;
            obj.numberRecord = numberRecord;
            obj.Key = name;
            obj.GameId = level;
            var list = dailyRepository.GetListDaiLy(out total, obj);

            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public ActionResult EditForm(long? id)
        //{
        //    var obj = new UserInfo2();

        //    if (id.HasValue)
        //    {
        //        obj = gamePlayerRepository.SelectOne(id.Value);
        //    }
        //    return Json(new
        //    {
        //        data = obj
        //    }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public ActionResult GetGameHistory(long uid, int gameid = 0, string mathid = "", string fDate = "", string tDate = "", int currentPage = 1, int currentRecord = 0, int numberRecord = 10)
        //{
        //    var mat = 0;
        //    if (!string.IsNullOrEmpty(mathid))
        //    {
        //        mat = Convert.ToInt32(mathid);
        //    }
        //    var fromdate = "";
        //    var todate = "";

        //    if (!string.IsNullOrEmpty(fDate))
        //    {
        //        fromdate = Convert.ToDateTime(fDate).ToString("yyyy-MM-dd HH:mm:ss");
        //    }
        //    if (!string.IsNullOrEmpty(tDate))
        //    {
        //        todate = Convert.ToDateTime(tDate).ToString("yyyy-MM-dd HH:mm:ss");
        //    }


        //    var total = 0;
        //    var list = gamePlayerRepository.GetGameHistory(out total, uid, gameid, mat, currentRecord, numberRecord, fromdate, todate);
        //    var currentUserName = uid;
        //    return Json(new
        //    {
        //        total = total,
        //        list = list,
        //        uid = uid,
        //        currentUserName = currentUserName
        //    },
        //    JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public ActionResult GetDetailGameHistory(int mathid = 0)
        //{
        //    var list = gamePlayerRepository.GetDetailGameHistory(mathid);
        //    return Json(new
        //    {
        //        data = list
        //    },
        //    JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public ActionResult BlockUser(int id)
        //{
        //    try
        //    {
        //        gamePlayerRepository.BlockUser(id);

        //        return Json(new
        //        {
        //            Error = false,
        //            msg = "Đã khóa thành công"

        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {

        //        return Json(new
        //        {
        //            Error = true,
        //            msg = "Đã có lỗi xẩy ra"

        //        }, JsonRequestBehavior.AllowGet);
        //    }

        //}

        //[HttpPost]
        //public ActionResult UnBlockUser(int id)
        //{
        //    try
        //    {
        //        gamePlayerRepository.UnBlockUser(id);

        //        return Json(new
        //        {
        //            Error = false,
        //            msg = "Đã mở khóa thành công"

        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {

        //        return Json(new
        //        {
        //            Error = true,
        //            msg = "Đã có lỗi xẩy ra"

        //        }, JsonRequestBehavior.AllowGet);
        //    }

        //}

        //[HttpPost]
        //public string ExportExcel(SearchItem obj)
        //{
        //    var fileName = string.Format("Player_{0}.xlsx", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
        //    var filePath = Path.Combine(Request.PhysicalApplicationPath, "File\\ExportImport", fileName);
        //    var folder = Request.PhysicalApplicationPath + "File\\ExportImport";
        //    if (!Directory.Exists(folder))
        //    {
        //        Directory.CreateDirectory(folder);
        //    }
        //    var total = 0;
        //    var list = gamePlayerRepository.PlayerExportExcel(obj.FromDate, obj.ToDate);

        //    ExportToExcel(list, filePath);

        //    return Request.Url.Authority + "/File/ExportImport/" + fileName;
        //}



        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Action(UserInfo objSubmit)
        //{

        //    //if (objSubmit.uid == 0)
        //    //{
        //    //    objSubmit.dateRegister = DateTime.Now.ToShortDateString();
        //    //    gamePlayerRepository.Add(objSubmit.userName, objSubmit.fullName, objSubmit.phone, objSubmit.email, objSubmit.cmnd, objSubmit.isMale ? 1 : 0, objSubmit.passWord);
        //    //}
        //    //else
        //    //{
        //    try
        //    {
        //        gamePlayerRepository.Update(objSubmit);
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    //var obj = gamePlayerRepository.GetItemById(objSubmit.uid);
        //    //UpdateModel(obj);


        //    return Json(new
        //    {
        //        Error = false

        //    }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public ActionResult InsertByExcel(object data)
        //{
        //    var file = Request["file"];
        //    var namef = "ImportCard-" + DateTime.Now.Ticks;
        //    System.IO.File.Move(Server.MapPath(ConfigWeb.TempPath + file), Server.MapPath("~/UploadFile/Files/" + namef + ".xlsx"));

        //    var fileName = Server.MapPath("~/UploadFile/Files/" + namef + ".xlsx");
        //    const int startRow = 1;
        //    var existingFile = new FileInfo(fileName);
        //    // Open and read the XlSX file.
        //    using (var package = new ExcelPackage(existingFile))
        //    {
        //        // Get the work book in the file
        //        var workBook = package.Workbook;
        //        if (workBook != null)
        //        {
        //            if (workBook.Worksheets.Count > 0)
        //            {
        //                var currentWorksheet = workBook.Worksheets.First();
        //                for (int i = startRow + 1; i <= currentWorksheet.Dimension.End.Row; i++)
        //                {
        //                    if (currentWorksheet.Cells[i, 2].Value != null)
        //                    {
        //                        var obj = new CardItem
        //                        {
        //                            cardNo = currentWorksheet.Cells[i, 1].Value.ToString().Trim(),
        //                            serial = currentWorksheet.Cells[i, 2].Value.ToString().Trim(),
        //                            value = Convert.ToInt32(currentWorksheet.Cells[i, 3].Value.ToString().Trim().Replace(".", "")),
        //                            dateInput = DateTime.Now,
        //                            dateExpired = DateTime.Now.AddYears(1)
        //                        };
        //                        var telname = currentWorksheet.Cells[i, 4].Value.ToString().Trim().ToLower();

        //                        if (telname.Contains("viettel"))
        //                        {
        //                            obj.telcoId = 3;
        //                        }
        //                        if (telname.Contains("vina"))
        //                        {
        //                            obj.telcoId = 2;
        //                        }
        //                        if (telname.Contains("mobi"))
        //                        {
        //                            obj.telcoId = 1;
        //                        }
        //                        gamePlayerRepository.CardInsertItem(obj);

        //                    }

        //                }
        //            }
        //        }
        //    }
        //    return Content(string.Empty);

        //}

        //[HttpGet]
        //public ActionResult GetAllCard(int used = 0, int telcoId = 0, int value = 0, int currentPage = 1, int currentRecord = 0, int numberRecord = 10, string seri = "")
        //{
        //    var total = 0;
        //    var list = gamePlayerRepository.GetAllCard(out total, (currentPage - 1) * numberRecord, numberRecord, used, telcoId, value, seri);

        //    return Json(new
        //    {
        //        total = total,
        //        list = list
        //    },
        //    JsonRequestBehavior.AllowGet);
        //}

    }
}
