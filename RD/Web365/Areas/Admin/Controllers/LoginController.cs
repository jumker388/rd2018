using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web365.App_Start;
using Web365.Filters;
using Web365Business.Back_End.IRepository;
using Web365Domain;
using Web365Utility;
using WebMatrix.WebData;

namespace Web365.Areas.Admin.Controllers
{
    [InitializeSimpleMembership]
    public class LoginController : Controller
    {
        //
        // GET: /Admin/Login/
        public LoginController()
        {
        }
        public ActionResult Index()
        {
            return View();
        }

        public static string Md5Hash(string input)
        {
            var hash = new StringBuilder();
            var md5Provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5Provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            foreach (var t in bytes)
            {
                hash.Append(t.ToString("x2"));
            }
            return hash.ToString();
        }

        [HttpPost]
        public ActionResult Login(string userName, string password, bool psesistCookie, string strOtp = "")
        {
            try
            {
                var result = WebSecurity.Login(userName, password, psesistCookie);

                if (result)
                {
                    return Json(new
                    {
                        status = true,
                        message = "Đăng nhập thành công."
                    }, JsonRequestBehavior.AllowGet);   
                }
                else
                {
                    return Json(new
                    {
                        status = false,
                        message = "Tên đăng nhập hoặc mật khẩu không đúng."
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                return Json(new
                {
                    status = false,
                    message = "Đã có lỗi xảy ra."
                }, JsonRequestBehavior.AllowGet);
            }
        }

        private string GetIPUser()
        {
            var currentRequest = HttpContext.Request;
            var ipAddress = currentRequest.ServerVariables["HTTP_CF_CONNECTING_IP"];

            if (ipAddress == null || ipAddress.ToLower() == "unknown")
                ipAddress = currentRequest.ServerVariables["REMOTE_ADDR"];

            return ipAddress;
        }

        [HttpPost]
        public ActionResult Logout()
        {

            WebSecurity.Logout();

            return Json(new
            {
                result = true,
                message = string.Empty
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangePassword(string currentPass, string newPass)
        {

            var result = WebSecurity.ChangePassword(WebSecurity.CurrentUserName, currentPass, newPass);




            return Json(new
            {
                result = result,
                message = string.Empty
            },
            JsonRequestBehavior.AllowGet);
        }

    }
}
