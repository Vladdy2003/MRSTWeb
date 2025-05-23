﻿using System;
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

            //JS
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.bundle.js"));
        }
    }
}