using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EvenimentMD.BusinessLogic.DatabaseContext;
using EvenimentMD.Domain.Enums;
using EvenimentMD.Domain.Models.Provider;

namespace EvenimentMD.Web.Controllers
{
    public class ServicesController : Controller
    {
        // GET: Services
        public ActionResult Index()
        {
            // Obține toți prestatorii de servicii
            List<ProviderDbTable> providers = new List<ProviderDbTable>();
            using (var db = new ProviderProfileContext())
            {
                providers = db.Providers.ToList();
            }

            // Dicționar pentru a stoca prima imagine a fiecărui prestator
            var providerFirstImages = new Dictionary<int, string>();

            // Obține prima imagine pentru fiecare prestator
            using (var db = new ProviderProfileMediaContext())
            {
                foreach (var provider in providers)
                {
                    // Caută prima imagine a prestatorului în baza de date
                    var firstImage = db.Media
                        .Where(m => m.providerId == provider.Id && m.mediaType == MediaType.Image)
                        .OrderBy(m => m.Id) // Asigură că selectăm prima imagine încărcată
                        .Select(m => new { m.providerId, m.filePath })
                        .FirstOrDefault();

                    // Dacă prestator are o imagine, o adăugăm în dicționar
                    if (firstImage != null)
                    {
                        providerFirstImages.Add(provider.Id, firstImage.filePath);
                    }
                }
            }

            // Stochează dicționarul cu imagini în ViewBag pentru a-l accesa în view
            ViewBag.ProviderFirstImages = providerFirstImages;

            return View(providers);
        }
    }
}