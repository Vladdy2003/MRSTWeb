using System;
using System.Web.Mvc;
using EvenimentMD.BusinessLogic.Interface;
using EvenimentMD.BusinessLogic.Interface.Provider;
using EvenimentMD.Domain.Models.Provider;
using EvenimentMD.LogicHelper.Atributes;
using EvenimentMD.Models.Provider;
using static System.Net.WebRequestMethods;

namespace EvenimentMD.Controllers
{
    public class ProviderController : BaseController
    {
        private readonly IBusinsessProfile _editBusinessProfile;

        public ProviderController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _editBusinessProfile = bl.GetBusinessProfileBL();
        }
        // GET: Provider

        [IsProvider]
        public ActionResult Index()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("AuthIndex", "Auth");
            }
            return View();
        }

        [IsProvider]
        public ActionResult BusinessProfile()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("AuthIndex", "Auth");
            }

            int userId = (int)System.Web.HttpContext.Current.Session["UserId"];
            var businessProfile = _editBusinessProfile.GetBusinessProfile(userId);

            BusinessProfileInfo model = new BusinessProfileInfo();

            if (businessProfile != null)
            {
                model.providerName = businessProfile.providerName;
                model.email = businessProfile.email;
                model.phoneNumber = businessProfile.phoneNumber;
                model.website = businessProfile.website;
                model.serviceType = businessProfile.serviceType;
                model.description = businessProfile.description;

                // Extragem numele de utilizator din URL-urile complete
                model.facebookURL = businessProfile.facebookURL != null ?
                    businessProfile.facebookURL.Replace("https://facebook.com/", "") : "";
                model.instagramURL = businessProfile.instagramURL != null ?
                    businessProfile.instagramURL.Replace("https://instagram.com/", "") : "";
                model.tiktokURL = businessProfile.tiktokURL != null ?
                    businessProfile.tiktokURL.Replace("https://www.tiktok.com/", "") : "";

                // Logo-ul existent va trebui gestionat în view
                model.logo = businessProfile.logo; // Adăugați acest câmp în model
            }

            return View(model);
        }

        [IsProvider]
        public ActionResult BusinessProfileGallery()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("AuthIndex", "Auth");
            }
            return View();
        }

        [IsProvider]
        public ActionResult BusinessProfileReview()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("AuthIndex", "Auth");
            }
            return View();
        }

        [IsProvider]
        public ActionResult BusinessProfileServices()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("AuthIndex", "Auth");
            }
            return View();
        }

        [IsProvider]
        public ActionResult BusinessProfileMembers()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("AuthIndex", "Auth");
            }
            return View();
        }
        
        [IsProvider]
        public ActionResult Agenda()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("AuthIndex", "Auth");
            }
            return View();
        }

        [IsProvider]
        public ActionResult Messages()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("AuthIndex", "Auth");
            }
            return View();
        }

        [IsProvider]
        public ActionResult UserProfile()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("AuthIndex", "Auth");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBusinessProfileData(BusinessProfileInfo info)
        {
            int userId = (int)System.Web.HttpContext.Current.Session["UserId"];

            if (ModelState.IsValid)
            {
                var data = new BusinessProfileData
                {
                    providerName = info.providerName,
                    email = info.email,
                    phoneNumber = info.phoneNumber,
                    website = info.website,
                    serviceType = info.serviceType,
                    description = info.description,
                    facebookURL = "https://facebook.com/" + info.facebookURL,
                    instagramURL = "https://instagram.com/" + info.instagramURL,
                    tiktokURL = "https://www.tiktok.com/" + info.tiktokURL,
                    logo = info.logo,
                    editedAt = DateTime.Now
                };

                try
                {
                    bool isSaved = _editBusinessProfile.EditBusinessProfile(data, userId);

                    if (isSaved)
                    {
                        TempData["SuccessMessage"] = "Datele au fost salvate cu succes.";
                        return RedirectToAction("BusinessProfile");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Vă rugăm să completați câmpurile obligatorii";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "A apărut o eroare în sistem. Vă rugăm să încercați mai târziu.";
                    System.Diagnostics.Debug.WriteLine($"Error in EditBusinessProfileData: {ex.Message}");
                }
            }
            return View("BusinessProfile", info);
        }
    }
}