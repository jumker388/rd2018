using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web365Business.Front_End.IRepository;
using Web365Domain;
using Web365Models;

namespace Web365.Controllers
{
    public class SearchController : BaseController
    {
        //
        // GET: /TestOnline/

        IProductRepositoryFE productRepositoryFE;

        public SearchController(IProductRepositoryFE _productRepositoryFE)
        {
            this.productRepositoryFE = _productRepositoryFE;
        }

        public ActionResult Index()
        {
            var keyword = Web365Utility.Web365Utility.ConvertToAscii(Request.QueryString["q"].ToLower().Trim());

            var list = productRepositoryFE.SearchProduct(keyword, 0, 10);

            return View(list);
        }

        public ActionResult SearchAdvance(string type, string filter)
        {
            var list = productRepositoryFE.SearchProductAdvance(type, filter, 0, 10);

            return View(list);
        } 
    }
}
