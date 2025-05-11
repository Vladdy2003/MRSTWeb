using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EvenimentMD.BusinessLogic.DatabaseContext;
using EvenimentMD.Domain.Models.Provider;

namespace EvenimentMD.Controllers
{
    public class ServicesController : BaseController
    {
        // GET: Services
        public ActionResult Index()
        {
            SessionStatus();

            List<ProviderDbTable> providers = new List<ProviderDbTable>();

            try
            {
                using (var db = new ProviderProfileContext())
                {
                    providers = db.Providers.ToList();
                }
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine($"Error fetching providers: {ex.Message}");
            }

            return View(providers);
        }
    }
}