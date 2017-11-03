using System.Web;
using System.Web.Optimization;

namespace Przuchodnia_Medyczna_Inz
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2.js",
                        "~/Scripts/jquery.swipebox.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/owl.carousel.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/navi").Include(
                      "~/Scripts/move-top.js",
                      "~/Scripts/easing.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/css/owl.carousel.css",
                      "~/Content/css/swipebox.css",
                      "~/Content/css/datepicker.css",
                      "~/Content/css/Site.css"));
        }
    }
}
