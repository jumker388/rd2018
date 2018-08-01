using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web365.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Admin/Home/

        public ActionResult Index()
        {
            return View();
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
        public ActionResult PagingNoPageSize(int page, int total)
        {
            return View(new[]
                                                                                              {
                                                                                                  page.ToString(CultureInfo.InvariantCulture),
                                                                                                  total.ToString(CultureInfo.InvariantCulture)
                                                                                              });
        }

    }
}
