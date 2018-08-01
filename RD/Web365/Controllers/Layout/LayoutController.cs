using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web365Business.Front_End.IRepository;
using Web365Models;

namespace Web365.Controllers
{
    public class LayoutController : BaseController
    {
        //
        // GET: /Layout/

        ILayoutContentRepositoryFE layoutContentRepositoryFE;
        IAdvertiesRepositoryFE advertiesRepositoryFE;
        IArticleRepositoryFE articleRepositoryFE;
        IArticleTypeRepositoryFE articleTypeRepositoryFE;
        IMenuRepositoryFE menuRepositoryFE;
        IProductTypeRepositoryFE productTypeRepositoryFE;
        IProductFilterRepositoryFE productFilterRepositoryFE;

        public LayoutController(ILayoutContentRepositoryFE _layoutContentRepositoryFE,
            IAdvertiesRepositoryFE _advertiesRepositoryFE,
            IArticleRepositoryFE _articleRepositoryFE,
            IArticleTypeRepositoryFE _articleTypeRepositoryFE,
            IMenuRepositoryFE _menuRepositoryFE,
            IProductTypeRepositoryFE _productTypeRepositoryFE,
            IProductFilterRepositoryFE _productFilterRepositoryFE)
        {
            this.layoutContentRepositoryFE = _layoutContentRepositoryFE;
            this.advertiesRepositoryFE = _advertiesRepositoryFE;
            this.articleRepositoryFE = _articleRepositoryFE;
            this.articleTypeRepositoryFE = _articleTypeRepositoryFE;
            this.menuRepositoryFE = _menuRepositoryFE;
            this.productTypeRepositoryFE = _productTypeRepositoryFE;
            this.productFilterRepositoryFE = _productFilterRepositoryFE;
        }

        public ActionResult Logo()
        {
            var result = layoutContentRepositoryFE.GetItemById(Convert.ToInt32(Resources.ResourceID.Logo));

            return View(result);
        }

        public ActionResult Header()
        {
            var headerModel = new Web365Models.HeaderModel() { 
                Content = layoutContentRepositoryFE.GetItemById(Convert.ToInt32(Resources.ResourceID.Header)),
                ListLinkTop = menuRepositoryFE.GetListByParent(Resources.ResourceID.MenuHeaderTop),
                ListLinkCustomer = menuRepositoryFE.GetListByParent(Resources.ResourceID.MenuHeaderCustomer)
            };

            return View(headerModel);
        }

        public ActionResult Sologan()
        {
            var content = layoutContentRepositoryFE.GetItemById(Convert.ToInt32(Resources.ResourceID.SologanContent));

            return View(content);
        }

        public ActionResult Footer()
        {
            var footer = new FooterModel() {
                Content = layoutContentRepositoryFE.GetItemById(Convert.ToInt32(Resources.ResourceID.Footer)),
                Social = layoutContentRepositoryFE.GetItemById(Convert.ToInt32(Resources.ResourceID.ContentSocialFooter)),
                ListMenu = menuRepositoryFE.GetListByParent(Resources.ResourceID.MenuFooter)
            };

            return View(footer);
        }

        public ActionResult Navbar()
        {
            var list = menuRepositoryFE.GetListByParent(Resources.ResourceID.MenuHeader);

            return View(list);
        }

        #region Left

        public ActionResult Filter()
        {

            var filter = new FilterLayoutModel() 
            {
                ListProductType = productTypeRepositoryFE.GetByGroup(Convert.ToInt32(Resources.ResourceID.GroupTypeProductFilterID)),
                ListProductFilter = productFilterRepositoryFE.GetByParent(null)
            };

            return View(filter);
        }

        public ActionResult MenuProduct()
        {

            var list = productTypeRepositoryFE.GetAllChildByParent(Convert.ToInt32(Resources.ResourceID.ProductRoot));

            var menu = new MenuProductModel()
            {
                ListProductType = list.Where(t => t.ID != Convert.ToInt32(Resources.ResourceID.ProductRoot)).ToList(),
                RootType = list.FirstOrDefault(t => t.ID == Convert.ToInt32(Resources.ResourceID.ProductRoot))
            };

            return View(menu);
        }

        public ActionResult Library()
        {
            var list = menuRepositoryFE.GetListByParent(Resources.ResourceID.MenuLibrary);

            return View(list);
        }

        public ActionResult Support()
        {
            var result = layoutContentRepositoryFE.GetItemById(Convert.ToInt32(Resources.ResourceID.ContentSupport));

            return View(result);
        }

        #endregion

        #region Right

        #endregion

        #region Ads

        #endregion
    }
}
