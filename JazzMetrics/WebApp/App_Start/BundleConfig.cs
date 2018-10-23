using System.Web.Optimization;

namespace WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Scripts/jquery-3.3.1.min.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/respond.min.js",
                        "~/Scripts/sweetalert.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/site.css"));

            //bundles pro jednotlive stranky
            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                        "~/Scripts/_custom/processing.js"));

            bundles.Add(new ScriptBundle("~/bundles/error").Include(
                        "~/Scripts/_custom/error/error.js",
                        "~/Scripts/_custom/processing.js"));
        }
    }
}
