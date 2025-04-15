using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace EvenimentMD.App_Start
{
    public static class BundleConfig
    {
        public static void RegistrationBundles(BundleCollection bundles)
        {
            //CSS
            bundles.Add(new StyleBundle("~/bundles/bootstrap/css").Include("~/Content/bootstrap.min.css", new CssRewriteUrlTransform()));
            bundles.Add(new StyleBundle("~/bundles/custom_style").Include("~/Content/Styles/style.css"));

            //JS
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom_script").Include("~/Scripts/services.js", "~/Scripts/login.js"));
        }
    }
}