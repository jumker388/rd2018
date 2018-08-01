using System.Web;
using System.Web.Mvc;

namespace Web365
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
    public class HTMLCompress : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var lstParam = "";
            var url = HttpContext.Current.Request.Url.AbsoluteUri;
            var method = HttpContext.Current.Request.HttpMethod;
            var response = filterContext.HttpContext.Response;

            var responseaction = filterContext.ActionDescriptor.ActionName;
            var responsecontroler = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            foreach (var parameter in filterContext.ActionParameters)
            {
                lstParam += string.Format("{0}: {1}", parameter.Key, parameter.Value);
            }
            var x = 1;
            //response.Filter = new MinifiedStream(response.Filter);
        }
    }

}