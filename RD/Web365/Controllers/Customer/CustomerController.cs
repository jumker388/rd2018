using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web365Base;
using Web365Business.Front_End.IRepository;
using Web365Domain;
using Web365Models;
using Web365Utility;

namespace Web365.Controllers
{
    public class CustomerController : BaseController
    {
        ICustomerRepositoryFE customerRepositoryFE;
        IOrderRepositoryFE orderRepositoryFE;

        public CustomerController(ICustomerRepositoryFE _customerRepositoryFE,
            IOrderRepositoryFE _orderRepositoryFE)
        {
            this.customerRepositoryFE = _customerRepositoryFE;
            this.orderRepositoryFE = _orderRepositoryFE;
        }

        [Cache("Home")]
        public ActionResult Index()
        {
            var customer = new CustomerModel()
            {
                ListOrder = orderRepositoryFE.GetListOrderByCustomer(CustomerIdentity.Customer.Info.ID)
            };

            return View(customer);
        }

        [Cache("Home")]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            try
            {
                var customer = new CustomerItem();

                if (customerRepositoryFE.TryGetByUserNameAndPassword(out customer, username, password))
                {

                    if (!customer.IsActive)
                    {
                        return Json(new
                        {
                            error = true,
                            message = "Tài khoản của bạn đã bị khóa."
                        }, JsonRequestBehavior.AllowGet);
                    }

                    var customerModel = new CustomerLoggedModel()
                    {
                        ID = customer.ID,
                        FullName = customer.LastName,
                        Email = customer.Email,
                        Phone = customer.Phone,
                        UserName = customer.UserName,
                        Address = customer.Address
                    };

                    Session["_cus"] = customerModel;

                    Response.Cookies.Add(new HttpCookie("_cus")
                    {
                        Value = Web365Utility.Web365Utility.StringToBase64(JsonConvert.SerializeObject(customerModel)),
                        Expires = DateTime.Now.AddDays(1)
                    });

                    return Json(new
                    {
                        error = false,
                        message = "Đăng nhập thành công"
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    error = true,
                    message = "Sai tên đăng nhập hoặc mật khẩu, bạn hãy thử lại."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
            }

            return Json(new
            {
                error = true,
                message = "Đăng nhập không thành công, bạn hãy thử lại"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangePassword(string username, string currentpassword, string password, string repassword)
        {
            try
            {
                var customer = new CustomerItem();

                if (customerRepositoryFE.TryGetByUserNameAndPassword(out customer, username, currentpassword))
                {

                    customer.Password = password;

                    customerRepositoryFE.UpdateCustomer(customer);

                    return Json(new
                    {
                        error = false,
                        message = "Thay đổi mật khẩu thành công"
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    error = false,
                    message = "Mật khẩu hiện tại không đúng"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
            }

            return Json(new
            {
                error = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangeInfo(string email, string phone, string address)
        {
            try
            {
                var customer = new CustomerItem();

                if (customerRepositoryFE.TryGetById(out customer, CustomerIdentity.Customer.Info.ID))
                {

                    customer.Email = email;

                    customer.Phone = phone;

                    customer.Address = address;

                    customerRepositoryFE.UpdateCustomer(customer);

                    return Json(new
                    {
                        error = false,
                        message = "Thay đổi thông tin thành công."
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    error = false,
                    message = "Có lỗi xảy ra, bạn hãy thử lại."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
            }

            return Json(new
            {
                error = true,
                message = "Có lỗi xảy ra, bạn hãy thử lại."
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            try
            {
                Session.Abandon();

                if (Request.Cookies["_cus"] != null)
                {
                    Response.Cookies["_cus"].Expires = DateTime.Now.AddDays(-1);
                }

                return Json(new
                {
                    error = false
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
            }

            return Json(new
            {
                error = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Register(string name, string email, string password)
        {
            try
            {
                var customerId = 0;

                var customer = new tblCustomer()
                {
                    UserName = email,
                    LastName = name,
                    Email = email,
                    Password = password
                };

                if (customerRepositoryFE.AddCustomer(out customerId, customer))
                {
                    if (customerId == 0)
                    {
                        return Json(new
                        {
                            error = true,
                            message = "Email của bạn đã tồn tại."
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var customerModel = new CustomerLoggedModel()
                        {
                            ID = customerId,
                            FullName = customer.LastName,
                            Email = customer.Email,
                            Phone = customer.Phone,
                            UserName = customer.UserName,
                            Address = customer.Address
                        };

                        Session["_cus"] = customerModel;

                        Response.Cookies.Add(new HttpCookie("_cus")
                        {
                            Value = Web365Utility.Web365Utility.StringToBase64(JsonConvert.SerializeObject(customerModel)),
                            Expires = DateTime.Now.AddDays(1)
                        });

                        Web365Utility.Web365Utility.SendMail(email, ConfigWeb.Domain + ": Đăng ký thành công", "Bạn đã đăng ký thành công tại " + ConfigWeb.Domain);

                        return Json(new
                        {
                            error = false
                        }, JsonRequestBehavior.AllowGet);
                    }
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
            }


            return Json(new
            {
                error = true,
                message = "Đăng ký không thành công bạn hãy thử lại."
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ForgetPassword(string email)
        {
            try
            {
                var token = string.Empty;

                if (customerRepositoryFE.ForgetPassword(out token, email))
                {

                    if (token == "LOCK")
                    {
                        return Json(new
                        {
                            error = true,
                            message = "Tài khoản của bạn đã bị khóa"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Web365Utility.Web365Utility.SendMail(email, "Thông tin lấy lại mật khẩu tại Myina", "Bạn click vào link dưới để lấy lại mật khẩu: <br /><br /> http://" + ConfigWeb.Domain + "/thanh-vien/lay-lai-mat-khau?email=" + email + "&k=" + token);

                        return Json(new
                        {
                            error = false,
                            message = "Bạn hãy kiểm tra mail để lấy lại mật khẩu"
                        }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json(new
                    {
                        error = true,
                        message = "Email không tồn tại trong hệ thống"
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
            }


            return Json(new
            {
                error = true,
                message = "Có lỗi xảy ra! bạn hãy thử lại."
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ResetPassword(string email, string key, string password, string repassword)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return Json(new
                    {
                        error = true,
                        message = "Có lỗi xảy ra! bạn hãy thử lại."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (customerRepositoryFE.ResetPassword(email, key, password))
                {
                    return Json(new
                    {
                        error = false,
                        message = "Đổi mật khẩu thành công."
                    }, JsonRequestBehavior.AllowGet);

                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
            }


            return Json(new
            {
                error = true,
                message = "Có lỗi xảy ra! bạn hãy thử lại."
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
