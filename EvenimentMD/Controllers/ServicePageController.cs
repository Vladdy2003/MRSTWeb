using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EvenimentMD.Domain.Models.Provider;
using EvenimentMD.BusinessLogic.DatabaseContext;
using EvenimentMD.Domain.Enums;

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

                // Get provider images
                List<ProviderMediaModel> providerImages = new List<ProviderMediaModel>();
                using (var db = new ProviderProfileMediaContext())
                {
                    providerImages = db.Media
                        .Where(m => m.providerId == id && m.mediaType == MediaType.Image)
                        .Select(m => new ProviderMediaModel
                        {
                            Id = m.Id,
                            providerId = m.providerId,
                            mediaType = m.mediaType,
                            filePath = m.filePath,
                            addedAt = m.addedAt
                        })
                        .ToList();
                }

                // Store provider images in ViewBag to access in the view
                ViewBag.ProviderImages = providerImages;

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