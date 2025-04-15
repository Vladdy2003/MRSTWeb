using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using EvenimentMD.App_Start;
using EvenimentMD.BusinessLogic.DatabaseContext;

namespace EvenimentMD
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegistrationBundles(BundleTable.Bundles);

            Database.SetInitializer(new CreateDatabaseIfNotExists<UserContext>());
        }
    }
}