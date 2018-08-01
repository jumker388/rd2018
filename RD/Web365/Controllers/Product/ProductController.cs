using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web365Base;
using Web365Business.Front_End.IRepository;
using Web365Models;

namespace Web365.Controllers
{
    public class ProductController : BaseController
    {
        IProductTypeRepositoryFE productTypeRepositoryFE;
        IProductStatusRepositoryFE productStatusRepositoryFE;
        IProductRepositoryFE productRepositoryFE;

        public ProductController(IProductTypeRepositoryFE _productTypeRepositoryFE,
            IProductRepositoryFE _productRepositoryFE,
            IProductStatusRepositoryFE _productStatusRepositoryFE)
        {
            this.productTypeRepositoryFE = _productTypeRepositoryFE;
            this.productRepositoryFE = _productRepositoryFE;
            this.productStatusRepositoryFE = _productStatusRepositoryFE;
        }

        [Cache("Home")]
        public ActionResult Index(string parentAscii, string ascii)
        {
            parentAscii = "san-pham";
            ascii = "san-pham";
            var type = productTypeRepositoryFE.GetByParentAsciiAndAscii(parentAscii, ascii);

            var homeModel = new ListTypeModel()
            {
                Type = type,
                ListProductType = productTypeRepositoryFE.GetListByParent(type.ID),
                ListProduct = productRepositoryFE.GetListByTypeAscii(0, 10, ascii)
            };

            return View(homeModel);
        }

        [Cache("Home")]
        public ActionResult List(string parentAscii, string ascii, string typeAscii)
        {

            var typeParent = productTypeRepositoryFE.GetByParentAsciiAndAscii(parentAscii, ascii);

            var type = productTypeRepositoryFE.GetByAsciiAndParent(typeAscii, typeParent.ID);

            var listModel = new ListPoductModel()
            {
                TypeParent = typeParent,
                Type = type,
                ListProduct = productRepositoryFE.GetListByTypeAscii(0, 10, typeAscii),
                ListProductType = productTypeRepositoryFE.GetListByParent(typeParent.ID)
            };

            return View(listModel);
        }

        [Cache("Home")]
        public ActionResult Group(string group)
        {
            var listModel = new ListPoductModel()
            {
                ProductStatus = productStatusRepositoryFE.GetByAscii(group),
                ListProduct = productRepositoryFE.GetListByGroupAscii(group)
            };

            return View(listModel);
        }

        [Cache("Home")]
        public ActionResult Detail(string parentAscii, string ascii, string typeAscii, string product)
        {
            var typeRoot = productTypeRepositoryFE.GetByParentAsciiAndAscii(parentAscii, ascii);

            var typeParentList = productTypeRepositoryFE.GetByParentAsciiAndAscii(ascii, typeAscii);

            var typeList = productTypeRepositoryFE.GetByAsciiAndParent(product, typeParentList.ID);

            if (typeList != null)
            {
                var listModel = new ListPoductModel()
                {
                    TypeParent = typeRoot,
                    Type = typeList,
                    ListProduct = productRepositoryFE.GetListByTypeAscii(0, 10, product),
                    ListProductType = productTypeRepositoryFE.GetListByParent(typeParentList.ID)
                };

                return View("~/Views/Product/ListThreeLevel.cshtml", listModel);
            }


            var productDetail = productRepositoryFE.GetItemByAscii(product);

            var typeParent = productTypeRepositoryFE.GetByParentAsciiAndAscii(parentAscii, ascii);

            var type = productTypeRepositoryFE.GetByAsciiAndParent(typeAscii, typeParent.ID);

            var detailModel = new ProductDetailModel()
            {
                TypeParent = typeParent,
                Type = type,
                ListProduct = productRepositoryFE.GetListByTypeAscii(0, 10, typeAscii),
                ListProductType = productTypeRepositoryFE.GetListByParent(typeParent.ID),
                Product = productDetail
            };

            return View(detailModel);
        }
    }
}
