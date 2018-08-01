using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Elmah.ContentSyndication;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Web365Utility;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Domain;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Net;
namespace Web365.Areas.Admin.Controllers
{
    public class GameLogsController : BaseController
    {

        private IGamePlayerRepository gamePlayerRepository;

        // GET: /Admin/ProductType/

        public GameLogsController(IGamePlayerRepository _gamePlayerRepository)
        {
            this.gamePlayerRepository = _gamePlayerRepository;
        }

        public ActionResult Index()
        {
            return View();

        }

        [HttpGet]
        public ActionResult GetList(int type = 1, int currentPage = 0, int numberRecord = 0, int currentRecord = 0, string key = "", int statusId = 1, string fDate = "", string tDate = "")
        {
            var total = 0;
            var list = gamePlayerRepository.DlGetListGameLogs(out total, User.Identity.Name, type, currentPage, numberRecord, key, statusId, fDate, tDate);
            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

       [HttpGet]
        public ActionResult GetListLogs(int id = 0, int currentPage = 0, int numberRecord = 0, int currentRecord = 0, string sendname = "", string recivename = "")
        {
            var total = 0;
            var list = gamePlayerRepository.GetFilterLogsPoc(out total, sendname, recivename, id, currentPage, numberRecord);
            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }



       [HttpGet]
       public ActionResult GetThongKePoc(int currentPage = 0, int numberRecord = 0, int currentRecord = 0)
       {
           var total = 0;
           var list = gamePlayerRepository.GetThongKePoc2(out total, currentPage, numberRecord, currentRecord);
           return Json(new
           {
               total = total,
               list = list
           },JsonRequestBehavior.AllowGet);
       }

       [HttpPost]
       public void Checking(string title)
       {
           var content = User.Identity.Name + " đang xem " + title;
           using (WebClient client = new WebClient())
           {
               client.Headers["User-Agent"] =
                   "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0) " +
                   "(compatible; MSIE 6.0; Windows NT 5.1; " +
                   ".NET CLR 1.1.4322; .NET CLR 2.0.50727)";

               // Download data.
               var arr = client.DownloadData("http://207.148.79.18/telegram_send.php?content=" + content);

           }
       }

       [HttpPost]
       public ActionResult UpdateLevelDaiLy(string username, int level, string parent = "")
       {
           try
           {
               var list = gamePlayerRepository.UpdateLevelDaiLy(username, level, parent);

               return Json(new
               {
                   status = true,
                   msg = "Cập nhật thành công",
                   data = list
               }, JsonRequestBehavior.AllowGet);
           }
           catch (Exception)
           {
               return Json(new
               {
                   status = false,
                   msg = "Truy thu thất bại",
               }, JsonRequestBehavior.AllowGet);
           }
       }
      


       [HttpPost]
       public ActionResult RefundMoney(int id, int type)
       {
           try
           {
               var list = gamePlayerRepository.RefundPoc(id, type);

               return Json(new
               {
                   status = true,
                   msg = "Truy thu thành công",
                   data = list
               },JsonRequestBehavior.AllowGet);
           }
           catch (Exception)
           {
               return Json(new
               {
                   status = false,
                   msg = "Truy thu thất bại",
               }, JsonRequestBehavior.AllowGet);
           }
      
       }


        [HttpGet]
        public ActionResult GetUserCurrent()
        {
            var list = gamePlayerRepository.GetDetailPlayer(User.Identity.Name);
            list.gameCashFormat = Web365Utility.Web365Utility.FormatPrice(list.gameCash);
            return Json(new
            {
                data = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetDaiLy(string userc1 = "", string userc2 = "")
        {
            try
            {
                if (ConfigWeb.UserSetDaiLy.Contains("," + User.Identity.Name + ","))
                {
                    if (!string.IsNullOrEmpty(userc1))
                    {
                        var objC1 = gamePlayerRepository.GetPortalUser(userc1);
                        if (objC1.ShopLevel == 0)
                        {
                            var newDl = gamePlayerRepository.SetDaiLy(userc1, 1);

                        }
                        if (!string.IsNullOrEmpty(userc2))
                        {
                            var newDl2 = gamePlayerRepository.SetDaiLy(userc2, 2, userc1);
                        }
                        return Json(new
                        {
                            Error = false,
                            msg = "Set thành công"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
              
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Error = true,
                    msg = ex.ToString()
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                Error = true,
                msg = "Đã có lỗi xảy ra"
            }, JsonRequestBehavior.AllowGet);
            
        }

        [HttpPost]
        public ActionResult CheckPlayerName(string name)
        {
            var list = gamePlayerRepository.CheckPlayerName(name);
            return Json(new
            {
                data = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateLogsGameDetail(int logDetailId, int statusId, string note)
        {
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                var list = gamePlayerRepository.UpdateLogsGameDetail(logDetailId, statusId, note);
                return Json(new
                {
                    data = list
                },
                JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                data = ""
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckDaiLy(string name)
        {
            var list = gamePlayerRepository.CheckDaiLy(name);
            return Json(new
            {
                data = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChuyenKhoan(ChuyenKhoanItem itemCk)
        {
            try
            {
                
                if (!string.IsNullOrEmpty(User.Identity.Name) && itemCk.MoneySend > 0  && !string.IsNullOrEmpty(itemCk.UserRecive))
                {
                    var currentUser = gamePlayerRepository.GetDetailPlayer(User.Identity.Name);

                    if (currentUser.gameCash > itemCk.MoneySend)
                    {

                        var phe = (float)2.0;
                        var pheSystem = (float)0.2;
                        var pheParent = (float)1.8;

                        var reciveUser = gamePlayerRepository.CheckDaiLy(itemCk.UserRecive);
                        if (reciveUser != null)
                        {
                            itemCk.StatusID = 1;
                            itemCk.UserSend = User.Identity.Name;
                            
                            if (currentUser.ShopLevel > 0)
                            {
                                if (reciveUser.ShopLevel > 0)
                                {
                                    // các đại lý chuyển tiền cho nhau
                                    itemCk.Tax = phe;
                                    itemCk.TaxParent = pheSystem;
                                    itemCk.TaxSystem = pheParent;
                                    itemCk.TaxMoney = 0;
                                    itemCk.TaxMoneyParent = 0;
                                    itemCk.TaxMoneySystem = 0;
                                    itemCk.MoneyRecive = itemCk.MoneySend - itemCk.TaxMoney;
                                }
                                else
                                {
                                    //Đại lý chuyển cho người chơi
                                    if (currentUser.ShopLevel == 1)
                                    {
                                        itemCk.Tax = phe;
                                        itemCk.TaxMoney = (itemCk.MoneySend * pheSystem) / 100;

                                        itemCk.TaxSystem = pheSystem;
                                        itemCk.TaxMoneySystem = itemCk.TaxMoney;

                                        itemCk.TaxParent = 0;
                                        itemCk.TaxMoneyParent = 0;

                                        itemCk.MoneyRecive = itemCk.MoneySend - itemCk.TaxMoney;

                                        // user là đại lý cấp 1    
                                        // bản thân user đc 1,8% - hệ thống 0,2%
                                    }
                                    if (currentUser.ShopLevel == 2)
                                    {
                                        // user là đại lý cấp 2
                                        // user parent đc 1,8% - hệ thống 0,2%

                                        itemCk.Tax = phe;
                                        itemCk.TaxMoney = (itemCk.MoneySend * phe) / 100;

                                        itemCk.TaxSystem = pheSystem;
                                        itemCk.TaxMoneySystem = (itemCk.MoneySend * pheSystem) / 100;

                                        itemCk.TaxParent = pheParent;
                                        itemCk.TaxMoneyParent = (itemCk.MoneySend * pheParent) / 100;

                                        itemCk.MoneyRecive = itemCk.MoneySend - itemCk.TaxMoney;

                                    }
                                }
                               
                            }

                            gamePlayerRepository.ChuyenKhoan(itemCk);
                            return Json(new
                            {
                                Error = false,
                                Status = "Chuyển thành công."
                            }, JsonRequestBehavior.AllowGet);
                        }

                    }

                }

            }
            catch (Exception)
            {
                return Json(new
                {
                    Error = true,
                    Status = "Đã có lỗi xảy ra!"
                },
                JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                Error = true,
                Status = "Đã có lỗi xảy ra!"
            },
             JsonRequestBehavior.AllowGet);

        }




        [HttpPost]
        [ValidateInput(false)]
        public bool Action(int total, int value, string name, string dateExpired, int isVCash)
        {
            try
            {
                //gamePlayerRepository.GeneratorGiftCode(total, value, name, Convert.ToDateTime(dateExpired), isVCash);
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
                            if (currentWorksheet.Cells[i, 2].Value != null)
                            {

                                var name = currentWorksheet.Cells[i, 1].Value.ToString();
                                var code = currentWorksheet.Cells[i, 2].Value.ToString();
                                var value = Convert.ToInt32(currentWorksheet.Cells[i, 3].Value.ToString());
                                var xx = currentWorksheet.Cells[i, 4].Value.ToString();
                                var bit = Convert.ToInt32(currentWorksheet.Cells[i, 4].Value.ToString());
                                var dateExpired = Convert.ToDateTime(xx);

                                gamePlayerRepository.InsertOneItemGameGift(code, value, name, dateExpired, bit);

                            }

                        }
                    }
                }
            }
            return Content(string.Empty);

        }

    }
}
