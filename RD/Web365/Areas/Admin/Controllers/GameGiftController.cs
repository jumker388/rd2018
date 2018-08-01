using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Web365Utility;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Domain;
using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
namespace Web365.Areas.Admin.Controllers
{

    //[HTMLCompress]
    public class GameGiftController : BaseController
    {

        private IGamePlayerRepository gamePlayerRepository;

        // GET: /Admin/ProductType/

        public GameGiftController(IGamePlayerRepository _gamePlayerRepository)
        {
            this.gamePlayerRepository = _gamePlayerRepository;
        }

        public ActionResult Index()
        {
            return View();

        }

        [HttpGet]
        public ActionResult GetListType(int numberRecord = 10, int currentRecord = 0)
        {
            var total = 0;
            var objFilter = new FilterItem();
            objFilter.currentRecord = currentRecord;
            objFilter.numberRecord = numberRecord;
            var list = gamePlayerRepository.GetListTypeGift(objFilter);
            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ActionGiftType(GiftCodeTypeItem objSubmit)
        {

            if (objSubmit.ID == 0)
            {
                gamePlayerRepository.InsertGameGiftType(objSubmit);

            }
            else
            {

                gamePlayerRepository.UpdateGameGiftType(objSubmit);
            }


            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new GiftCodeTypeItem();

            if (id.HasValue)
            {
                obj = gamePlayerRepository.GetDetailGameGiftType(id.Value);
            }
            else
            {
                obj.IsShow = false;
            }
            return Json(new
            {
                data = obj,
            }, JsonRequestBehavior.AllowGet);
        }


        public string ExportInventory(int id)
        {
            using (ExcelPackage excel = new ExcelPackage())
            {

                var worksheet = excel.Workbook.Worksheets.Add("Gift code");

                var properties = new string[]
                    {
                        "Code",
                        "Money",
                        "DateCreated",
                        "DateUsed",
                        "UserID",
                        "UserName",
                        "Campaign",                        
                        };
                for (var i = 0; i < properties.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i];
                    //worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }
                var xx = gamePlayerRepository.GetAllGiftCodeByTurnID(id);
                var row = 2;
                var dem = 0;
                foreach (var sbk in xx)
                {
                    dem++;
                    int col = 1;

                    //order properties
                    worksheet.Cells[row, col].Value = sbk.Code;
                    col++;

                    worksheet.Cells[row, col].Value = sbk.Money;
                    col++;

                    worksheet.Cells[row, col].Value = sbk.DateCreatedString;
                    col++;

                    worksheet.Cells[row, col].Value = sbk.DateUsedString;
                    col++;

                    worksheet.Cells[row, col].Value = sbk.user_id;
                    col++;

                    worksheet.Cells[row, col].Value = sbk.username;
                    col++;

                    worksheet.Cells[row, col].Value = sbk.TypeName;
                    col++;
                    //next row
                    row++;
                }

                var filename = xx.FirstOrDefault().TypeName + "_" + DateTime.Now.Ticks + ".xlsx";
                var xx2 = Server.MapPath("/") + "/UploadFile/Files/" + filename;
                FileInfo excelFile = new FileInfo(xx2);
                excel.SaveAs(excelFile);
                return Request.Url.Authority + "/UploadFile/Files/" + filename;
            }
        }


        [HttpPost]
        public ActionResult ShowAllTurn(int listId)
        {
            try
            {
                if (ConfigWeb.UserSetDaiLy.Contains(User.Identity.Name))
                {
                    gamePlayerRepository.UpdateTurnGift(listId, 1);
                }
                return Json(new
                {
                    Error = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    Error = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult HideAllTurn(int listId)
        {
            try
            {
                if (ConfigWeb.UserSetDaiLy.Contains(User.Identity.Name))
                {
                    gamePlayerRepository.UpdateTurnGift(listId, 0);
                }
                return Json(new
                {
                    Error = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    Error = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ExportToExcel(int id)
        {
            if (ConfigWeb.UserSetDaiLy.Contains(User.Identity.Name))
            {
                return Json(new
                {
                    link = ExportInventory(id)
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                link = ""
            }, JsonRequestBehavior.AllowGet);

        }

        public bool DeleteGameGiftType(int id)
        {
            try
            {
                if (ConfigWeb.UserSetDaiLy.Contains(User.Identity.Name))
                {
                    gamePlayerRepository.DeleteGameGiftType(id);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }


        [HttpGet]
        public ActionResult GetList(bool used, int numberRecord = 10, int currentRecord = 0)
        {
            var total = 0;
            var list = new List<GiftTurnItem>();

            if (ConfigWeb.UserSetDaiLy.Contains(User.Identity.Name))
            {
                list = gamePlayerRepository.GetAllGiftCode(out total, used, currentRecord, numberRecord);
            }
            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        public bool DeleteGameGift(int id)
        {
            try
            {
                gamePlayerRepository.DeleteGiftCode(id);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        [HttpPost]
        [ValidateInput(false)]
        public bool Action(string name, string dateExpired, int type, string prefix, string note, string giatri, string soluong, string Giftpass)
        {
            if (Giftpass != ConfigWeb.GiftPass)
            {
                return false;
            }

            try
            {
                if (ConfigWeb.UserSetDaiLy.Contains(User.Identity.Name))
                {
                    gamePlayerRepository.GeneratorGiftCode(name, Convert.ToDateTime(dateExpired), type, prefix, note,
                        giatri, soluong);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        [HttpPost]
        public ActionResult InsertByExcel(object data)
        {
            if (ConfigWeb.UserSetDaiLy.Contains(User.Identity.Name))
            {
                var file = Request["file"];
                var namef = DateTime.Now.Ticks;
                System.IO.File.Move(Server.MapPath(ConfigWeb.TempPath + file),
                    Server.MapPath("~/UploadFile/Files/" + namef + ".xlsx"));

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
                                if (currentWorksheet.Cells[i, 2].Value != null)
                                {

                                    var type = Convert.ToInt32(currentWorksheet.Cells[i, 1].Value.ToString());
                                    var code = currentWorksheet.Cells[i, 2].Value.ToString();
                                    var value = Convert.ToInt32(currentWorksheet.Cells[i, 3].Value.ToString());
                                    var xx = currentWorksheet.Cells[i, 4].Value.ToString();
                                    var dateExpired = Convert.ToDateTime(xx);

                                    gamePlayerRepository.InsertOneItemGameGift(code, value, "", dateExpired, type);

                                }

                            }
                        }
                    }
                }
                return Content(string.Empty);
            }
            return Content(string.Empty);
        }

    }
}
