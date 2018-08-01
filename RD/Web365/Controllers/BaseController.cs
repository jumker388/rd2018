using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Web365Business.Front_End.IRepository;

namespace Web365.Controllers
{
    
    public class BaseController : Controller
    {
        protected override void ExecuteCore()
        {
            //var cultureName = "en";

            //// Attempt to read the culture cookie from Request
            //HttpCookie cultureCookie = Request.Cookies["_culture"];

            //// If there is a cookie already with the language, use the value for the translation, else uses the default language configured.
            //if (cultureCookie != null)
            //    cultureName = cultureCookie.Value;
            //else
            //{
            //    //cultureName = "vi-VN";

            //    cultureName = "en";

            //    cultureCookie = new HttpCookie("_culture");
            //    cultureCookie.HttpOnly = false; // Not accessible by JS.
            //    cultureCookie.Expires = DateTime.Now.AddYears(1);
            //}

            // Sets the new language to the cookie.
            //cultureCookie.Value = cultureName;

            // Sets the cookie on the response.
            //Response.Cookies.Add(cultureCookie);

            // Modify current thread's cultures            
            //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            //Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            base.ExecuteCore();
        }

        public string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        public class HtmlCompress : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var response = filterContext.HttpContext.Response;
                response.Filter = new MinifiedStream(response.Filter);
            }
        }

        public class MinifiedStream : MemoryStream
        {
            private readonly Stream _output;
            public MinifiedStream(Stream stream)
            {
                _output = stream;
            }
            private static readonly Regex Whitespace = new Regex(@"(?<=[^])\t{2,}|(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,11}(?=[<])|(?=[\n])\s{2,}", RegexOptions.Compiled);
            public override void Write(byte[] buffer, int offset, int count)
            {
                var html = Encoding.UTF8.GetString(buffer);
                html = Whitespace.Replace(html, string.Empty);
                html = html.Trim();
                _output.Write(Encoding.UTF8.GetBytes(html), offset, Encoding.UTF8.GetByteCount(html));
            }
        }

    }
}
