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
            //Bootstrap CSS
            bundles.Add(new StyleBundle("~/bundles/bootstrap/css").Include("~/Content/bootstrap.min.css", new CssRewriteUrlTransform()));

            //Bootstrap JS
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/services").Include("~/Scripts/services.js", "~/Scripts/login.js"));
        }
    }
}