using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EvenimentMD.Domain.Models.Provider;
using EvenimentMD.BusinessLogic.DatabaseContext;

namespace EvenimentMD.Controllers
{
    public class ServicePageController : BaseController
    {
        public ActionResult Index(int id)
        {
            SessionStatus();
            ProviderDbTable provider = null;

            try
            {
                using (var db = new ProviderProfileContext())
                {
                    provider = db.Providers.FirstOrDefault(p => p.Id == id);

                    if (provider == null)
                    {
                        // Provider not found, redirect to services list
                        return RedirectToAction("Index", "Services");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine($"Error fetching provider: {ex.Message}");
                return RedirectToAction("Index", "Services");
            }

            return View(provider);
        }
    }
}