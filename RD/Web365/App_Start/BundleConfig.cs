
using System.Web;
using System.Web.Optimization;
using Web365Utility;

namespace Web365
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/Style/eke/js").Include(
                        "~/Content/DrPepper/js/libs/jquery1112min.js", 
                        "~/Content/DrPepper/js/libs/bootstrapmin.js",
                        "~/Content/DrPepper/js/swipermin.js",
                        "~/Content/DrPepper/js/toastMess/js/jquerytoast.js",
                        "~/Content/Eke/js/functions.js"
                        ));


            bundles.Add(new ScriptBundle("~/Style/jstq").Include(
                        "~/Content/tq9v2/js/functions.js"
                        ));

            bundles.Add(new StyleBundle("~/Style/eke/css").Include(
                        "~/Content/Eke/css/bootstrapmin.css", 
                        "~/Content/Eke/css/swipermin.css",
                        "~/Content/DrPepper/js/toastMess/css/jquerytoast.css",
                        "~/Content/Eke/css/freset.css",
                        "~/Content/Eke/css/style.css" 
                ));

            BundleTable.EnableOptimizations = ConfigWeb.IsBundle;
        }
    }
}