using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web365Base;
using Web365Business.Front_End.IRepository;
using Web365Models;
using Web365Utility;

namespace Web365.Controllers
{
    public class CartController : BaseController
    {
        ILayoutContentRepositoryFE layoutContentRepositoryFE;
        IOrderRepositoryFE orderRepositoryFE;

        public CartController(ILayoutContentRepositoryFE _layoutContentRepositoryFE,
            IOrderRepositoryFE _orderRepositoryFE)
        {
            this.layoutContentRepositoryFE = _layoutContentRepositoryFE;
            this.orderRepositoryFE = _orderRepositoryFE;
        }

        [Cache("Home")]
        public ActionResult Index()
        {
            var cartModel = new CartModel();

            //cartModel.Content = layoutContentRepositoryFE.GetItemById(Convert.ToInt32(Resources.ResourceID.ContentCart));

            //cartModel.ContentOrderSuccess = layoutContentRepositoryFE.GetItemById(Convert.ToInt32(Resources.ResourceID.ContentCartOrderSuccess));

            //if (Request.Cookies["_coc"] != null)
            //{

            //    var str = Server.UrlDecode(Request.Cookies["_coc"].Value);

            //    cartModel.ListOrder = JsonConvert.DeserializeObject<List<OrderItemModel>>(Web365Utility.Web365Utility.StringBase64ToString(str));
            //}

            return View(cartModel);
        }

        [HttpPost]
        public ActionResult AddOrder(string name, string phone)
        {
            try
            {
                const string address = "Hà Nội";
                const string email = "cva0211@gmail.com";

                var order = new tblOrder()
                {
                    Address = address,
                    CustomerName = name,
                    Phone = phone,
                    Email = email,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    OrderStatusID = 1,
                    TotalCost = decimal.Zero,
                    IsViewed = false,
                    IsDeleted = false
                };

                if (CustomerIdentity.Customer.IsLogged)
                {
                    order.CustomerID = CustomerIdentity.Customer.Info.ID;
                }

                var orderShipping = new tblOrder_Shipping()
                {
                    Address = address,
                    CustomerName = name,
                    Phone = phone,
                    Email = email
                };

                var orderDetail = new tblOrderDetail()
                {
                    Price = 0,
                    ProductID = 155,
                    ProductVariantID = 303,
                    Quantity = 1
                };

                orderRepositoryFE.AddOrder(order, orderDetail, orderShipping);

                return Json(new
                {
                    error = false,
                    message = "Đặt hàng thành công !"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
            }

            return Json(new
            {
                error = true,
                message = "Đặt hàng không thành công. Bạn hãy thử lại !"
            }, JsonRequestBehavior.AllowGet);

        }


        [Cache("Home")]
        public ActionResult OrderSuccess()
        {
            return View();
        }
    }
}
