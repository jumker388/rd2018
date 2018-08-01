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
    public class GamePlayerController : BaseController
    {

        private IGamePlayerRepository gamePlayerRepository;

        // GET: /Admin/ProductType/

        public GamePlayerController(IGamePlayerRepository _gamePlayerRepository)
        {
            this.gamePlayerRepository = _gamePlayerRepository;
        }

        public ActionResult CheckNoHu(int gameId = 0, int mathId = 0)
        {
            var model = gamePlayerRepository.GameSlotGetDetail(gameId, mathId);
            return View(model);
        }


        [HttpPost]
        public ActionResult Block(string username)
        {
            try
            {
                if (ConfigWeb.UserSetDaiLy.Contains("," + User.Identity.Name + ","))
                {
                    gamePlayerRepository.BlockUserMember(username);
                    return Json(new
                    {
                        err = false,
                        msg = username.Contains("_isLock") ? "Mở khóa thành công" : "Khóa thành công"
                    }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new
                    {
                        err = true,
                        msg = "Bạn không có quyền block user"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new
                {
                    err = true,
                    msg = "Đã có lỗi xảy ra, vui lòng thử lại sau."
                }, JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetRetention(string fDateReg, string tDateReg, string fDateReciveMoney, string tDateReciveMoney)
        {
            var list = new RetetionItem();
            if (!string.IsNullOrEmpty(fDateReg) && !string.IsNullOrEmpty(fDateReciveMoney))
            {
                list = gamePlayerRepository.GetRetention(fDateReg, tDateReg, fDateReciveMoney, tDateReciveMoney);
            }
            return Json(new
            {
                total = 0,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetList(string key, int currentRecord, int numberRecord, string fDate, string tDate, int lastlogin = 0)
        {
            var total = 0;
            var list = gamePlayerRepository.GetList(out total, fDate, tDate, key, currentRecord, numberRecord, lastlogin);

            return View(list);
        }

        [HttpGet]
        public ActionResult GameBank()
        {

            var list = gamePlayerRepository.GetGameBank();

            return Json(new
            {
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GiftCodeChecker(string name, int currentPage, int currentRecord, int numberRecord)
        {
            var total = 0;
            if (string.IsNullOrEmpty(name))
            {
                return Json(new
                {
                    total,
                    list = gamePlayerRepository.GetGiftCodeUsed(out total, currentRecord, numberRecord),
                }, JsonRequestBehavior.AllowGet);
            }

            var list = gamePlayerRepository.CheckGiftCode(name);

            return Json(new
            {
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CardChecker(string name = "", int uid = 0, string mathe = "", string seri = "", int currentPage = 1, int currentRecord = 0, int numberRecord = 10)
        {
            var total = 0;
            var list = gamePlayerRepository.CardChecker(out total, name, uid, mathe, seri, currentPage, currentRecord,
                numberRecord);
            return Json(new
            {
                total,
                list = list
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetGiftCodeUsed(int currentRecord, int numberRecord)
        {
            var total = 0;
            var list = gamePlayerRepository.GetGiftCodeUsed(out total, currentRecord, numberRecord);

            return Json(new
            {
                total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetTopPlayer(string name, int currentRecord, int numberRecord, string fDate, string tDate, int lastlogin = 0, decimal moneyGiaoDich = 0)
        {

            var total = 0;

            if (!string.IsNullOrEmpty(name))
            {
                moneyGiaoDich = Convert.ToDecimal(name);
            }

            var list = gamePlayerRepository.GetTopPlayer(out total, fDate, tDate, name, currentRecord, numberRecord, lastlogin, moneyGiaoDich);

            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetListDaiLyShow()
        {

            var list = gamePlayerRepository.GetDaiLyShow();

            return Json(new
            {
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditFormDaiLyShow(int? id)
        {
            var obj = new DaiLyShowItem();
            obj.IsShow = true;
            if (id.HasValue)
            {
                obj = gamePlayerRepository.GetDaiLyShowByID(id.Value);
            }
            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ActionDaiLyShow(DaiLyShowItem obj)
        {
            try
            {
                if (obj.ID > 0)
                {
                    gamePlayerRepository.UpdatetDaiLyShow(obj);
                }
                else
                {
                    gamePlayerRepository.InserUpdatetDaiLyShow(obj);
                }


                return Json(new
                {
                    Error = false,
                    msg = "Đã cập nhật thành công"

                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new
                {
                    Error = true,
                    msg = "Đã có lỗi xẩy ra"

                }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult DeleteDaiLy(int id)
        {
            try
            {
                gamePlayerRepository.ActioDaiLy(id, 3);

                return Json(new
                {
                    Error = false,
                    msg = "Đã xóa thành công"

                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new
                {
                    Error = true,
                    msg = "Đã có lỗi xẩy ra"

                }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult GetGameHistory(long uid, int gameid = 0, string mathid = "", string fDate = "", string tDate = "", int currentPage = 1, int currentRecord = 0, int numberRecord = 10)
        {
            var mat = 0;
            if (!string.IsNullOrEmpty(mathid))
            {
                mat = Convert.ToInt32(mathid);
            }
            var fromdate = "";
            var todate = "";

            if (!string.IsNullOrEmpty(fDate))
            {
                fromdate = Convert.ToDateTime(fDate).ToString("yyyy-MM-dd H:mm:00");
            }

            if (!string.IsNullOrEmpty(tDate))
            {
                todate = Convert.ToDateTime(tDate).ToString("yyyy-MM-dd H:mm:59");
            }


            var total = 0;
            var list = gamePlayerRepository.GetGameHistory(out total, uid, gameid, mat, currentRecord, numberRecord, fromdate, todate);
            var currentUserName = uid;
            return Json(new
            {
                total = total,
                list = list,
                uid = uid,
                currentUserName = currentUserName
            },
            JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetGameHistoryTaiXiu(long uid, int gameid = 0, string mathid = "", string fDate = "", string tDate = "", int currentPage = 1, int currentRecord = 0, int numberRecord = 10)
        {
            var mat = 0;
            if (!string.IsNullOrEmpty(mathid))
            {
                mat = Convert.ToInt32(mathid);
            }
            var fromdate = "";
            var todate = "";

            if (!string.IsNullOrEmpty(fDate))
            {
                fromdate = Convert.ToDateTime(fDate).ToString("yyyy-MM-dd 00:00:00");
            }
            if (!string.IsNullOrEmpty(tDate))
            {
                todate = Convert.ToDateTime(tDate).ToString("yyyy-MM-dd 23:59:59");
            }


            var total = 0;
            var list = gamePlayerRepository.GetGameHistoryTaiXiu(out total, uid, gameid, mat, currentRecord, numberRecord, fromdate, todate);
            var currentUserName = uid;
            return Json(new
            {
                total = total,
                list = list,
                uid = uid,
                currentUserName = currentUserName
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetGameHistoryMoney(long uid, int gameid = 0, string fDate = "", string tDate = "", int currentPage = 1, int currentRecord = 0, int numberRecord = 10)
        {
            var fromdate = "";
            var todate = "";

            if (!string.IsNullOrEmpty(fDate))
            {
                fromdate = Convert.ToDateTime(fDate).ToString("yyyy-MM-dd 00:00:00");
            }
            if (!string.IsNullOrEmpty(tDate))
            {
                todate = Convert.ToDateTime(tDate).ToString("yyyy-MM-dd 23:59:59");
            }

            var currentUserName = uid;
            var total = 0;
            var list = gamePlayerRepository.GetGameHistoryMoney(out total, uid, gameid, currentRecord, numberRecord, fromdate, todate);

            if (ConfigWeb.fbAdmin.Contains(User.Identity.Name) && list != null)
            {
                if (list.FirstOrDefault().ShopLevel > 0)
                {
                    return Json(new
                    {
                        total = total,
                        list = new List<GameHistoryItem>(),
                        uid = uid,
                        currentUserName = currentUserName
                    }, JsonRequestBehavior.AllowGet);
                }
            }


            return Json(new
            {
                total = total,
                list = list,
                uid = uid,
                currentUserName = currentUserName
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDetailGameHistory(int mathid = 0)
        {
            var list = gamePlayerRepository.GetDetailGameHistory(mathid);
            return Json(new
            {
                data = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult BlockUser(int id)
        {
            try
            {
                gamePlayerRepository.BlockUser(id);

                return Json(new
                {
                    Error = false,
                    msg = "Đã khóa thành công"

                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new
                {
                    Error = true,
                    msg = "Đã có lỗi xẩy ra"

                }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult UnBlockUser(int id)
        {
            try
            {
                gamePlayerRepository.UnBlockUser(id);

                return Json(new
                {
                    Error = false,
                    msg = "Đã mở khóa thành công"

                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new
                {
                    Error = true,
                    msg = "Đã có lỗi xẩy ra"

                }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public string ExportExcel(SearchItem obj)
        {
            var fileName = string.Format("Player_{0}.xlsx", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            var filePath = Path.Combine(Request.PhysicalApplicationPath, "File\\ExportImport", fileName);
            var folder = Request.PhysicalApplicationPath + "File\\ExportImport";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var total = 0;
            var list = gamePlayerRepository.PlayerExportExcel(obj.FromDate, obj.ToDate);

            ExportToExcel(list, filePath);

            return Request.Url.Authority + "/File/ExportImport/" + fileName;
        }

        [HttpGet]
        public ActionResult EditForm(long? id)
        {
            var obj = new UserInfo2();

            if (id.HasValue)
            {
                obj = gamePlayerRepository.SelectOne(id.Value);
            }
            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(UserInfo objSubmit)
        {

            //if (objSubmit.uid == 0)
            //{
            //    objSubmit.dateRegister = DateTime.Now.ToShortDateString();
            //    gamePlayerRepository.Add(objSubmit.userName, objSubmit.fullName, objSubmit.phone, objSubmit.email, objSubmit.cmnd, objSubmit.isMale ? 1 : 0, objSubmit.passWord);
            //}
            //else
            //{
            try
            {
                objSubmit.sex = false;
                if (ConfigWeb.UserSetDaiLy.Contains(User.Identity.Name))
                {
                    objSubmit.sex = true;
                }

                gamePlayerRepository.Update(objSubmit);

                //var javaScriptSerializer = new  System.Web.Script.Serialization.JavaScriptSerializer();
                //string jsonString = javaScriptSerializer.Serialize(objSubmit);

                var logDes = User.Identity.Name + " update UID : " + objSubmit.id + " Mobile: " + objSubmit.mobile + " Pass: " + objSubmit.passWord;
                gamePlayerRepository.InsertLogsAdmin(logDes);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Elmah.Error(ex));
            }
            //var obj = gamePlayerRepository.GetItemById(objSubmit.uid);
            //UpdateModel(obj);


            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertByExcel(object data)
        {
            var file = Request["file"];
            var namef = "ImportCard-" + DateTime.Now.Ticks;
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
                                var obj = new CardItem
                                {
                                    cardNo = currentWorksheet.Cells[i, 1].Value.ToString().Trim(),
                                    serial = currentWorksheet.Cells[i, 2].Value.ToString().Trim(),
                                    value = Convert.ToInt32(currentWorksheet.Cells[i, 3].Value.ToString().Trim().Replace(".", "")),
                                    dateInput = DateTime.Now,
                                    dateExpired = DateTime.Now.AddYears(1)
                                };
                                var telname = currentWorksheet.Cells[i, 4].Value.ToString().Trim().ToLower();

                                if (telname.Contains("viettel"))
                                {
                                    obj.telcoId = 3;
                                }
                                if (telname.Contains("vina"))
                                {
                                    obj.telcoId = 2;
                                }
                                if (telname.Contains("mobi"))
                                {
                                    obj.telcoId = 1;
                                }
                                gamePlayerRepository.CardInsertItem(obj);

                            }

                        }
                    }
                }
            }
            return Content(string.Empty);

        }

        [HttpGet]
        public ActionResult GetAllCard(int used = 0, int telcoId = 0, int value = 0, int currentPage = 1, int currentRecord = 0, int numberRecord = 10, string seri = "")
        {
            var total = 0;
            var list = gamePlayerRepository.GetAllCard(out total, (currentPage - 1) * numberRecord, numberRecord, used, telcoId, value, seri);

            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

    }
}
