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
    public class ProviderController : BaseController
    {
        private readonly IBusinsessProfile _businessProfile;

        public ProviderController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _businessProfile = bl.GetBusinessProfileBL();
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

        [IsProvider]
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
                try
                {
                    using (var db = new BusinessLogic.DatabaseContext.ProviderProfileMediaContext())
                    {
                        var mediaList = db.Media.Where(m => m.providerId == profileId).ToList();

                        foreach (var media in mediaList)
                        {
                            existingMedia.Add(new ProviderMediaModel
                            {
                                Id = media.Id,
                                providerId = media.providerId,
                                mediaType = media.mediaType,
                                filePath = media.filePath,
                                addedAt = media.addedAt
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading existing media: {ex.Message}");
                }
            }

            ViewBag.ExistingMedia = existingMedia;
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

        [IsProvider]
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

        [IsProvider]
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

        [IsProvider]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteMedia(int mediaId)
        {
            try
            {
                int userId = (int)System.Web.HttpContext.Current.Session["UserId"];
                int profileId = _businessProfile.GetBusinessProfileId(userId);

                if (profileId == 0)
                {
                    return Json(new { success = false, message = "Profilul de business nu a fost găsit." });
                }

                ProviderMediaDb mediaToDelete = null;

                // Get media from database
                using (var db = new BusinessLogic.DatabaseContext.ProviderProfileMediaContext())
                {
                    mediaToDelete = db.Media.FirstOrDefault(m => m.Id == mediaId && m.providerId == profileId);
                }

                if (mediaToDelete == null)
                {
                    return Json(new { success = false, message = "Media nu a fost găsită." });
                }

                // Delete physical file
                string physicalPath = Server.MapPath(mediaToDelete.filePath);
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }

                // Delete from database
                using (var db = new BusinessLogic.DatabaseContext.ProviderProfileMediaContext())
                {
                    var media = db.Media.FirstOrDefault(m => m.Id == mediaId);
                    if (media != null)
                    {
                        db.Media.Remove(media);
                        db.SaveChanges();
                    }
                }

                return Json(new { success = true, message = "Media a fost ștearsă cu succes." });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting media: {ex.Message}");
                return Json(new { success = false, message = $"Eroare la ștergerea media: {ex.Message}" });
            }
        }
    }
}