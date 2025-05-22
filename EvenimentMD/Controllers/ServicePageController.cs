using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EvenimentMD.Domain.Models.Provider;
using EvenimentMD.BusinessLogic.Interface.Services;

namespace EvenimentMD.Controllers
{
    public class ServicePageController : BaseController
    {
        private readonly IServices _serviceOperations;

        public ServicePageController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _serviceOperations = bl.GetServiceOperationsBL();
        }

        public ActionResult Index(int id)
        {
            SessionStatus();
            ProviderDbTable provider = _serviceOperations.GetProviderById(id);
            try
            {
                // Get provider by ID
                if (provider == null)
                {
                    // Provider not found, redirect to services list
                    return RedirectToAction("Index", "Services");
                }

                // Get provider images
                List<ProviderMediaModel> providerImages = _serviceOperations.GetProviderImages(id);

                // Get provider services
                List<ProviderServicesData> providerServices = _serviceOperations.GetProviderServices(id);

                // Store provider images in ViewBag to access in the view
                ViewBag.ProviderImages = providerImages;

                // Store provider services in ViewBag to access in the view
                ViewBag.ProviderServices = providerServices;
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