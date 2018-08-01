using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Web365Models;

namespace CustomerIdentity
{
    /// <summary>
    ///  07-01-2014
    /// </summary>
    public static class Customer
    {
        /// <summary>
        /// return status login of customer
        /// </summary>
        public static bool IsLogged
        {
            get
            {
                try
                {
                    return HttpContext.Current.Session["_cus"] != null || HttpContext.Current.Request.Cookies["_cus"] != null;
                }
                catch (Exception)
                {
                    return false;
                }
                
            }
        }

        /// <summary>
        /// return object info customer when them logged
        /// </summary>
        public static CustomerLoggedModel Info
        {
            get
            {
                try
                {
                    if (HttpContext.Current.Session["_cus"] != null)
                    {
                        return (CustomerLoggedModel)HttpContext.Current.Session["_cus"];
                    }

                    return JsonConvert.DeserializeObject<CustomerLoggedModel>(Web365Utility.Web365Utility.StringBase64ToString(HttpContext.Current.Request.Cookies["_cus"].Value));
                }
                catch (Exception)
                {
                    return new CustomerLoggedModel() 
                    { 
                        Avartar = string.Empty,
                        Email = string.Empty,
                        ID = 0,
                        Phone = string.Empty,
                        UserName = string.Empty,
                        Address = string.Empty
                    };
                }

            }
        }
    }
}