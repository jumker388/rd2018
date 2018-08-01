using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using API_NganLuong;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Web365.Models;
using Web365.WNL;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Business.Front_End.IRepository;
using Web365Domain;
using Web365Domain.Game;
using Web365Models;
using Web365;
using Web365Utility;

namespace Web365.Controllers
{
    public class HomeController : BaseController
    {
        readonly IArticleRepositoryFE _articleRepositoryFe;
        private readonly IAdvertiesRepositoryFE _advertiesRepositoryFe;
        readonly ILayoutContentRepositoryFE _layoutContentRepositoryFe;
        readonly IOtherRepositoryFE _otherRepositoryFe;
        readonly IArticleTypeRepositoryFE _articleTypeRepositoryFe;
        readonly IGamePlayerRepository _iGamePlayerRepository;

        public HomeController(IArticleRepositoryFE articleRepositoryFe, IGamePlayerRepository iGamePlayerRepository,
            ILayoutContentRepositoryFE layoutContentRepositoryFe,
            IArticleTypeRepositoryFE articleTypeRepositoryFe,
            IAdvertiesRepositoryFE advertiesRepositoryFe,
            IOtherRepositoryFE otherRepositoryFe
            )
        {
            this._articleRepositoryFe = articleRepositoryFe;
            this._layoutContentRepositoryFe = layoutContentRepositoryFe;
            _otherRepositoryFe = otherRepositoryFe;
            _advertiesRepositoryFe = advertiesRepositoryFe;
            _articleTypeRepositoryFe = articleTypeRepositoryFe;
            _iGamePlayerRepository = iGamePlayerRepository;
        }

        [Cache("Home")]
        public ActionResult Index()
        {
            return View();
            //var a2 = HttpContext.Request.ServerVariables["HTTP_USER_AGENT"];
            //var mobile2 = Web365Utility.Web365Utility.IsRequestMobile(a2);
            //if (mobile2)
            //{
            //    return View(@"~/Views/Home/MIndex.cshtml");
            //}
        }

        [Cache("Home")]
        public ActionResult Contact()
        {
            var result = _articleRepositoryFe.TopOneNewsByType("lien-he");
            return View(result);
        }

        [Cache("Home")]
        public ActionResult Register()
        {
            return View(@"~/Views/Mobile/Register.cshtml");
        }

        [Cache("Home")]
        public ActionResult NotFound()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddContact(string name, string email, string phone, string title, string content)
        {
            var contact = new tblContact()
            {
                Address = string.Empty,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Email = email,
                IsDeleted = false,
                IsViewed = false,
                Message = content,
                Name = name,
                Phone = phone,
                Title = title
            };

            var result = _otherRepositoryFe.AddContact(contact);

            return Json(new
            {
                error = !result

            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RegisterAccGame(string userName, string passWord)
        {
            try
            {
                var objSubmit = new UserInfo
                {
                    dateRegister = DateTime.Now.ToShortDateString(),
                    userName = userName,
                    fullName = userName,
                    passWord = passWord
                };
                var result = _iGamePlayerRepository.Add(objSubmit.userName, objSubmit.fullName, objSubmit.phone, objSubmit.email, objSubmit.cmnd, objSubmit.isMale ? 1 : 0, objSubmit.passWord);
                if (result == null)
                {
                    return Json(new
                    {
                        mess = false,
                        note = "Tài khoản đã tồn tại."

                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new
                {
                    mess = true,
                    note = "Đăng ký thành công"

                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    mess = false,
                    note = "Đã có lỗi xảy ra, vui lòng thử lại sau."

                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult RegisterEmail(string email)
        {
            var obj = new tblReceiveInfo()
            {
                Name = email,
                Email = email,
                Phone = string.Empty,
                DateCreated = DateTime.Now,
                GroupID = 1,
                IsShow = true,
                IsDeleted = false
            };

            var result = _otherRepositoryFe.AddEmailReciveInfo(obj);

            return Json(new
            {
                error = !result

            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Paging(int page, int total)
        {
            return View(new[]
                                                                                              {
                                                                                                  page.ToString(CultureInfo.InvariantCulture),
                                                                                                  total.ToString(CultureInfo.InvariantCulture)
                                                                                              });
        }


        [HttpGet]
        public ActionResult PagingHome(int page, int total)
        {
            return View(new[]
                                                                                              {
                                                                                                  page.ToString(CultureInfo.InvariantCulture),
                                                                                                  total.ToString(CultureInfo.InvariantCulture)
                                                                                              });
        }

        public ActionResult Footer()
        {
            var model = _articleRepositoryFe.FooterInfo(3341, 3339, 3342);
            return View(model);
        }

        public ActionResult Slide()
        {
            var list = _advertiesRepositoryFe.GetItemById(12);
            return Request.Browser.IsMobileDevice ? View(@"~/Views/Mobile/_partiaSlider.cshtml", list) : View(@"~/Views/Home/_partiaSlider.cshtml", list);
        }

        public ActionResult RightBanner()
        {
            var list = _advertiesRepositoryFe.GetItemById(14);
            return View(@"~/Views/Home/_partiaRightAds.cshtml", list);
        }
        public ActionResult LastestNews()
        {
            var model = _articleRepositoryFe.GetTopByType(3324, 0, 5);
            return View(model);
        }

        public ActionResult ListGame()
        {
            var model = _articleRepositoryFe.GetTopByType(3325, 0, 50);
            return Request.Browser.IsMobileDevice ? View(@"~/Views/Mobile/ListGame.cshtml", model) : View(model);
        }

        public ActionResult Noti()
        {
            var model = _articleRepositoryFe.GetTopByType(3344, 0, 1);
            return View(model);
        }

        public ActionResult Header()
        {
            ViewBag.GroupMenu = _articleTypeRepositoryFe.GetGroupItemIsShowMenu().OrderByDescending(x => x.Number);
            ViewBag.Top = _articleTypeRepositoryFe.GetListByParentAscii("menu-top").OrderByDescending(x => x.Number);
            ViewBag.Bot = _articleTypeRepositoryFe.GetListByParentAscii("menu-bot").OrderByDescending(x => x.Number);
            return View(@"~/Views/Home/_partiaHeader.cshtml");
        }

        public ActionResult ThanhToan()
        {
            var uid = Request["uid"];
            var user = new UserInfo();
            if (!string.IsNullOrEmpty(uid))
            {
                user = _iGamePlayerRepository.GetItemById(Convert.ToInt64(uid));
            }

            return View(user);
        }


        [HttpPost]
        public JsonResult GetLinkNl(string paymentMethod, string strBankcode, string name, string email, string mobile, int uid, string money)
        {
            var info = new RequestInfo
            {
                Merchant_id = ConfigWeb.MerchantId,
                Merchant_password = ConfigWeb.MerchantPassword,
                Receiver_email = ConfigWeb.ReceiverEmail,
                cur_code = "vnd",
                bank_code = strBankcode,
                Order_code = DateTime.Now.Ticks + "-UID:" + uid,
                Total_amount = money,
                fee_shipping = "0",
                Discount_amount = "0",
                order_description = "Thanh toan SDT : " + mobile,
                return_url = ConfigWeb.ReturnUrl,
                cancel_url = ConfigWeb.CancelUrl,
                Buyer_fullname = name,
                Buyer_email = email,
                Buyer_mobile = mobile,
                Payment_type = "1" // Thanh toan ngay
            };

            var objNlChecout = new APICheckoutV3();
            var result = objNlChecout.GetUrlCheckout(info, paymentMethod);

            if (result.Error_code == "00")
            {
                var objPayment = new PaymentItem
                {
                    buyer_uid = uid,
                    buyer_fullname = info.Buyer_fullname,
                    buyer_mobile = info.Buyer_mobile,
                    total_amount = Convert.ToInt32(info.Total_amount),
                    order_code = info.Order_code,
                    payment_method = info.Payment_method,
                    bank_code = info.bank_code,
                    payment_type = info.Payment_type,
                    transaction_status = "01",
                    token = result.Token,
                    time_request = DateTime.Now,
                    time_receive = DateTime.Now
                };

                _iGamePlayerRepository.InsertPayment(objPayment);

                return Json(new
                {
                    content = result.Checkout_url,
                    error = false
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                content = result.Description,
                error = false
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ThanhToanThanhCong()
        {
            var model = new PaymentItem();
            var status = Request["error_code"];
            var token = Request["token"];

            if (status == "00")
            {
                var objCheck = new RequestCheckOrder
                {
                    Merchant_id = ConfigWeb.MerchantId,
                    Merchant_password = ConfigWeb.MerchantPassword,
                    Token = token
                };
                var objNlChecout = new APICheckoutV3();
                var detailResult = objNlChecout.GetTransactionDetail(objCheck);

                var detailPayment = _iGamePlayerRepository.GetDetailPaymentByToken(detailResult.token);
                if (detailPayment != null && detailPayment.transaction_status != "00" && detailResult.transactionStatus == "00")
                {
                    detailPayment.total_amount = Convert.ToInt32(detailResult.paymentAmount);
                    detailPayment.transaction_status = detailResult.transactionStatus;
                    detailPayment.transaction_id = detailResult.transactionId;
                    detailPayment.time_receive = DateTime.Now;
                    var updatePayment = _iGamePlayerRepository.UpdatePayment(detailPayment);

                    if (ConfigWeb.PercentSale > 0)
                    {
                        detailPayment.total_amount = detailPayment.total_amount +
                                                     (detailPayment.total_amount / ConfigWeb.PercentSale);
                    }

                    if (detailResult.transactionStatus == "00")
                    {
                        var updateMoney = _iGamePlayerRepository.InsertMoney(detailPayment);
                    }
                }
                return View(detailPayment);
            }


            return View(model);
        }

    }
}
