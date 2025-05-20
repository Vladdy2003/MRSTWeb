using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EvenimentMD.BusinessLogic.Interface;
using EvenimentMD.BusinessLogic.Interface.Services;
using EvenimentMD.Domain.Models.Provider;

namespace EvenimentMD.Web.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServices _serviceOperations;

        public ServicesController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _serviceOperations = bl.GetServiceOperationsBL();
        }

        // GET: Services
        public ActionResult Index()
        {
            // Obține toți prestatorii de servicii
            List<ProviderDbTable> providers = _serviceOperations.GetAllProviders();

            // Obține dicționarul cu primele imagini pentru fiecare prestator
            var providerFirstImages = _serviceOperations.GetProviderFirstImages(providers);

            // Stochează dicționarul cu imagini în ViewBag pentru a-l accesa în view
            ViewBag.ProviderFirstImages = providerFirstImages;

            return View(providers);
        }
    }
}