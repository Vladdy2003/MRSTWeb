using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using EvenimentMD.BusinessLogic.Interface;
using EvenimentMD.BusinessLogic.Interface.Provider;
using EvenimentMD.Domain.Enums;
using EvenimentMD.Domain.Models.Provider;
using EvenimentMD.LogicHelper.Atributes;
using EvenimentMD.Models.Provider;
using static System.Net.WebRequestMethods;
using System.Linq;
using System.Web;


namespace EvenimentMD.Controllers
{
    [IsProvider]
    public class ProviderController : BaseController
    {
        private readonly IBusinsessProfile _businessProfile;

        public ProviderController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _businessProfile = bl.GetBusinessProfileBL();

        }
        // GET: Provider

        public ActionResult Index()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("AuthIndex", "Auth");
            }
            return View();
        }

        public ActionResult BusinessProfile()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("AuthIndex", "Auth");
            }

            int userId = (int)System.Web.HttpContext.Current.Session["UserId"];
            var businessProfile = _businessProfile.GetBusinessProfile(userId);

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

        public ActionResult BusinessProfileGallery()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("AuthIndex", "Auth");
            }

            // Get existing media from database
            int userId = (int)System.Web.HttpContext.Current.Session["UserId"];
            int profileId = _businessProfile.GetBusinessProfileId(userId);

            var existingMedia = new List<ProviderMediaModel>();

            if (profileId > 0)
            {
                existingMedia = _businessProfile.GetProviderMediaList(profileId);
            }

            ViewBag.ExistingMedia = existingMedia;
            return View();
        }

        public ActionResult BusinessProfileReview()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("AuthIndex", "Auth");
            }
            return View();
        }

        public ActionResult BusinessProfileServices()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("AuthIndex", "Auth");
            }

            int userId = (int)System.Web.HttpContext.Current.Session["UserId"];
            int profileId = _businessProfile.GetBusinessProfileId(userId);

            var existingServices = new List<ProviderServicesData>();

            if (profileId > 0)
            {
                existingServices = _businessProfile.GetProviderServices(profileId);
            }

            ViewBag.ExistingServices = existingServices;
            return View();
        }

        public ActionResult BusinessProfileMembers()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("AuthIndex", "Auth");
            }
            return View();
        }

        public ActionResult Agenda()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("AuthIndex", "Auth");
            }
            return View();
        }

        public ActionResult Messages()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("AuthIndex", "Auth");
            }
            return View();
        }

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
                    bool isSaved = _businessProfile.EditBusinessProfile(data, userId);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UploadMedia()
        {
            try
            {
                int userId = (int)System.Web.HttpContext.Current.Session["UserId"];
                System.Diagnostics.Debug.WriteLine($"UploadMedia called for user {userId}");

                // Get files from request
                var imageFiles = new List<HttpPostedFileBase>();
                var videoFiles = new List<HttpPostedFileBase>();

                System.Diagnostics.Debug.WriteLine($"Request.Files.Count: {Request.Files.Count}");

                // Process all files in request
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];

                    if (file != null && file.ContentLength > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"File: {file.FileName}, ContentType: {file.ContentType}, Size: {file.ContentLength}");

                        // Check if it's an image or video based on content type AND file extension
                        if (file.ContentType.StartsWith("image/") ||
                            file.FileName.ToLower().EndsWith(".jpg") ||
                            file.FileName.ToLower().EndsWith(".jpeg") ||
                            file.FileName.ToLower().EndsWith(".png") ||
                            file.FileName.ToLower().EndsWith(".gif"))
                        {
                            imageFiles.Add(file);
                            System.Diagnostics.Debug.WriteLine($"Added as image: {file.FileName}");
                        }
                        else if (file.ContentType.StartsWith("video/") ||
                                file.FileName.ToLower().EndsWith(".mp4") ||
                                file.FileName.ToLower().EndsWith(".avi") ||
                                file.FileName.ToLower().EndsWith(".mov") ||
                                file.FileName.ToLower().EndsWith(".wmv") ||
                                file.FileName.ToLower().EndsWith(".webm"))
                        {
                            videoFiles.Add(file);
                            System.Diagnostics.Debug.WriteLine($"Added as video: {file.FileName}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Unknown file type: {file.FileName} - {file.ContentType}");
                        }
                    }
                }

                System.Diagnostics.Debug.WriteLine($"Received {imageFiles.Count} images and {videoFiles.Count} videos");

                // Process media using business logic
                var result = await _businessProfile.ProcessAndSaveMedia(imageFiles, videoFiles, userId);

                // Log the result details
                System.Diagnostics.Debug.WriteLine($"Process result - Success: {result.Success}");
                System.Diagnostics.Debug.WriteLine($"Message: {result.Message}");
                System.Diagnostics.Debug.WriteLine($"Images processed: {result.ImagesProcessed}");
                System.Diagnostics.Debug.WriteLine($"Videos processed: {result.VideosProcessed}");

                if (result.Errors != null && result.Errors.Any())
                {
                    System.Diagnostics.Debug.WriteLine("Errors:");
                    foreach (var error in result.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"- {error}");
                    }
                }

                return Json(new
                {
                    success = result.Success,
                    message = result.Message,
                    imagesProcessed = result.ImagesProcessed,
                    videosProcessed = result.VideosProcessed,
                    errors = result.Errors
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in UploadMedia: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                return Json(new
                {
                    success = false,
                    message = $"A apărut o eroare în sistem: {ex.Message}",
                    errors = new[] { ex.ToString() }
                });
            }
        }

        public JsonResult DeleteMedia(int mediaId)
        {
            try
            {
                int userId = (int)System.Web.HttpContext.Current.Session["UserId"];

                // Call the business logic method
                bool result = _businessProfile.DeleteMedia(mediaId, userId);

                if (result)
                {
                    return Json(new { success = true, message = "Media a fost ștearsă cu succes." });
                }
                else
                {
                    return Json(new { success = false, message = "Media nu a fost găsită sau nu aveți permisiunea să o ștergeți." });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting media: {ex.Message}");
                return Json(new { success = false, message = $"Eroare la ștergerea media: {ex.Message}" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddService(ProviderServicesViewModel data)
        {
            int userId = (int)System.Web.HttpContext.Current.Session["UserId"];

            if (ModelState.IsValid)
            {
                var serviceData = new ProviderServicesData
                {
                    serviceName = data.serviceName,
                    servicePrice = data.servicePrice,
                    serviceDescription = data.serviceDescription,
                    currency = data.currency
                };
                try
                {
                    bool isSaved = _businessProfile.AddService(serviceData, userId);
                    if (isSaved)
                    {
                        TempData["SuccessMessage"] = "Serviciul a fost adăugat cu succes.";
                        return RedirectToAction("BusinessProfileServices");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Vă rugăm să completați câmpurile obligatorii";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "A apărut o eroare în sistem. Vă rugăm să încercați mai târziu.";
                    System.Diagnostics.Debug.WriteLine($"Error in AddService: {ex.Message}");
                }
            }
            return RedirectToAction("BusinessProfileServices");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateService(ProviderServicesViewModel data, int serviceId)
        {
            int userId = (int)System.Web.HttpContext.Current.Session["UserId"];

            if (ModelState.IsValid)
            {
                var serviceData = new ProviderServicesData
                {
                    Id = serviceId,
                    serviceName = data.serviceName,
                    servicePrice = data.servicePrice,
                    serviceDescription = data.serviceDescription,
                    currency = data.currency
                };

                try
                {
                    bool isUpdated = _businessProfile.UpdateService(serviceData, userId);
                    if (isUpdated)
                    {
                        TempData["SuccessMessage"] = "Serviciul a fost actualizat cu succes.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Nu s-a putut actualiza serviciul.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "A apărut o eroare în sistem. Vă rugăm să încercați mai târziu.";
                    System.Diagnostics.Debug.WriteLine($"Error in UpdateService: {ex.Message}");
                }
            }

            return RedirectToAction("BusinessProfileServices");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteService(int serviceId)
        {
            try
            {
                int userId = (int)System.Web.HttpContext.Current.Session["UserId"];

                bool isDeleted = _businessProfile.DeleteService(serviceId, userId);

                if (isDeleted)
                {
                    return Json(new { success = true, message = "Serviciul a fost șters cu succes." });
                }
                else
                {
                    return Json(new { success = false, message = "Nu s-a putut șterge serviciul." });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting service: {ex.Message}");
                return Json(new { success = false, message = $"Eroare la ștergerea serviciului: {ex.Message}" });
            }
        }
    }
}