using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EvenimentMD.BusinessLogic.DatabaseContext;
using EvenimentMD.Domain.Enums;
using EvenimentMD.Domain.Models.Provider;
using EvenimentMD.BusinessLogic.Interface.Provider;
using System.Collections.Generic;

namespace EvenimentMD.BusinessLogic.Core
{
    public class ProviderAPI
    {
        internal bool EditBusinessProfileLogic(BusinessProfileData data, int userId)
        {
            if (data == null ||
                string.IsNullOrEmpty(data.providerName) ||
                string.IsNullOrEmpty(data.phoneNumber) ||
                string.IsNullOrEmpty(data.email) ||
                string.IsNullOrEmpty(data.serviceType) ||
                string.IsNullOrEmpty(data.description))
            {
                return false;
            }

            try
            {
                var editTime = DateTime.Now;
                ProviderDbTable profile;

                using (var db = new ProviderProfileContext())
                {
                    profile = db.Providers.FirstOrDefault(p => p.userId == userId);
                }

                if (profile != null)
                {
                    //Actualizarea informatiilor
                    profile.providerName = data.providerName;
                    profile.phoneNumber = data.phoneNumber;
                    profile.email = data.email;
                    profile.website = data.website;
                    profile.serviceType = data.serviceType;
                    profile.description = data.description;
                    profile.facebookURL = data.facebookURL;
                    profile.instagramURL = data.instagramURL;
                    profile.tiktokURL = data.tiktokURL;
                    profile.logo = data.logo;
                    profile.editedAt = editTime;

                    using (var db = new ProviderProfileContext())
                    {
                        db.Entry(profile).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    profile = new ProviderDbTable()
                    {
                        userId = userId,
                        providerName = data.providerName,
                        phoneNumber = data.phoneNumber,
                        email = data.email,
                        website = data.website,
                        serviceType = data.serviceType,
                        description = data.description,
                        facebookURL = data.facebookURL,
                        instagramURL = data.instagramURL,
                        tiktokURL = data.tiktokURL,
                        logo = data.logo,
                        editedAt = editTime
                    };

                    using (var db = new ProviderProfileContext())
                    {
                        db.Providers.Add(profile);
                        db.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in EditBusinessProfileLogic {ex.Message}");
                return false;
            }

            return true;
        }

        internal BusinessProfileData GetBusinessProfileLogic(int userId)
        {
            ProviderDbTable profile;

            using (var db = new ProviderProfileContext())
            {
                profile = db.Providers.FirstOrDefault(p => p.userId == userId);
            }

            if (profile != null)
            {
                return new BusinessProfileData()
                {
                    providerName = profile.providerName,
                    phoneNumber = profile.phoneNumber,
                    email = profile.email,
                    website = profile.website,
                    serviceType = profile.serviceType,
                    description = profile.description,
                    facebookURL = profile.facebookURL,
                    instagramURL = profile.instagramURL,
                    tiktokURL = profile.tiktokURL,
                    logo = profile.logo
                };
            }

            return new BusinessProfileData()
            {
                providerName = string.Empty,
                phoneNumber = string.Empty,
                email = string.Empty,
                website = string.Empty,
                serviceType = string.Empty,
                description = string.Empty,
                facebookURL = string.Empty,
                instagramURL = string.Empty,
                tiktokURL = string.Empty,
                logo = string.Empty
            };
        }

        internal int GetBusinessProfileIdLogic(int userId)
        {
            ProviderDbTable profile;

            using (var db = new ProviderProfileContext())
            {
                profile = db.Providers.FirstOrDefault(p => p.userId == userId);
            }

            if (profile != null)
            {
                return profile.Id;
            }
            else
            {
                return 0;
            }

        }

        internal int BusinessProfileMediaCountLogic(int profileId, MediaType mediaType)
        {
            int mediaCount;

            using (var db = new ProviderProfileMediaContext())
            {
                mediaCount = db.Media.Count(m => m.providerId == profileId && m.mediaType == mediaType);
            }

            return mediaCount;
        }

        internal async Task<MediaUploadResult> ProcessAndSaveMediaLogic(List<HttpPostedFileBase> images, List<HttpPostedFileBase> videos, int userId)
        {
            var result = new MediaUploadResult();

            try
            {
                // Get business profile
                int profileId = GetBusinessProfileIdLogic(userId);
                if (profileId == 0)
                {
                    result.Success = false;
                    result.Message = "Profilul de business nu a fost găsit. Vă rugăm să completați mai întâi informațiile de business.";
                    return result;
                }

                // Get business profile details
                var businessProfile = GetBusinessProfileLogic(userId);

                // Create directory structure
                string providerFolderName = $"{profileId}_{businessProfile.providerName}";
                // Clean folder name from invalid characters
                providerFolderName = string.Join("_", providerFolderName.Split(Path.GetInvalidFileNameChars()));

                string baseMediaPath = HttpContext.Current.Server.MapPath("~/Resources/ProviderProfileMedia");
                string providerPath = Path.Combine(baseMediaPath, providerFolderName);
                string imagesPath = Path.Combine(providerPath, "Images");
                string videosPath = Path.Combine(providerPath, "Videos");

                // Create directories if they don't exist
                System.Diagnostics.Debug.WriteLine($"Base media path: {baseMediaPath}");
                System.Diagnostics.Debug.WriteLine($"Provider path: {providerPath}");
                System.Diagnostics.Debug.WriteLine($"Images path: {imagesPath}");
                System.Diagnostics.Debug.WriteLine($"Videos path: {videosPath}");

                if (!Directory.Exists(baseMediaPath))
                {
                    Directory.CreateDirectory(baseMediaPath);
                    System.Diagnostics.Debug.WriteLine("Created base media directory");
                }

                if (!Directory.Exists(providerPath))
                {
                    Directory.CreateDirectory(providerPath);
                    System.Diagnostics.Debug.WriteLine("Created provider directory");
                }

                if (!Directory.Exists(imagesPath))
                {
                    Directory.CreateDirectory(imagesPath);
                    System.Diagnostics.Debug.WriteLine("Created images directory");
                }

                if (!Directory.Exists(videosPath))
                {
                    Directory.CreateDirectory(videosPath);
                    System.Diagnostics.Debug.WriteLine("Created videos directory");
                }

                // Verify directories were created successfully
                System.Diagnostics.Debug.WriteLine($"Base path exists: {Directory.Exists(baseMediaPath)}");
                System.Diagnostics.Debug.WriteLine($"Provider path exists: {Directory.Exists(providerPath)}");
                System.Diagnostics.Debug.WriteLine($"Images path exists: {Directory.Exists(imagesPath)}");
                System.Diagnostics.Debug.WriteLine($"Videos path exists: {Directory.Exists(videosPath)}");

                // Test write permissions
                try
                {
                    string testFile = Path.Combine(videosPath, "test_write.tmp");
                    File.WriteAllText(testFile, "test");
                    File.Delete(testFile);
                    System.Diagnostics.Debug.WriteLine("Write permissions verified for videos directory");
                }
                catch (Exception permEx)
                {
                    System.Diagnostics.Debug.WriteLine($"WARNING: Write permission test failed: {permEx.Message}");
                }

                // Check current media count
                int currentImageCount = BusinessProfileMediaCountLogic(profileId, MediaType.Image);
                int currentVideoCount = BusinessProfileMediaCountLogic(profileId, MediaType.Video);

                // Validate limits
                if (images != null && currentImageCount + images.Count > 10)
                {
                    result.Errors.Add($"Nu puteți încărca mai mult de 10 imagini. Aveți {currentImageCount} imagini și încercați să adăugați {images.Count}.");
                }

                if (videos != null && currentVideoCount + videos.Count > 3)
                {
                    result.Errors.Add($"Nu puteți încărca mai mult de 3 videouri. Aveți {currentVideoCount} videouri și încercați să adăugați {videos.Count}.");
                }

                if (result.Errors.Any())
                {
                    result.Success = false;
                    result.Message = string.Join(" ", result.Errors);
                    return result;
                }

                if (images != null && images.Any())
                {
                    foreach (var image in images)
                    {
                        if (image != null && image.ContentLength > 0)
                        {
                            try
                            {
                                // Check file size (5MB limit)
                                if (image.ContentLength > 5 * 1024 * 1024)
                                {
                                    result.Errors.Add($"Imaginea {image.FileName} depășește limita de 5MB.");
                                    continue;
                                }

                                // Generate unique filename
                                string fileExtension = Path.GetExtension(image.FileName);
                                string fileName = $"{Guid.NewGuid()}{fileExtension}";
                                string filePath = Path.Combine(imagesPath, fileName);

                                // Save image directly without compression
                                image.SaveAs(filePath);

                                // Save to database
                                var mediaRecord = new ProviderMediaDb
                                {
                                    providerId = profileId,
                                    mediaType = MediaType.Image,
                                    filePath = $"~/Resources/ProviderProfileMedia/{providerFolderName}/Images/{fileName}",
                                    addedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                };

                                using (var db = new ProviderProfileMediaContext())
                                {
                                    db.Media.Add(mediaRecord);
                                    db.SaveChanges();
                                }

                                result.ImagesProcessed++;
                            }
                            catch (Exception ex)
                            {
                                result.Errors.Add($"Eroare la procesarea imaginii {image.FileName}: {ex.Message}");
                            }
                        }
                    }
                }

                if (videos != null && videos.Any())
                {
                    System.Diagnostics.Debug.WriteLine($"Starting to process {videos.Count} videos");

                    foreach (var video in videos)
                    {
                        if (video != null && video.ContentLength > 0)
                        {
                            try
                            {
                                System.Diagnostics.Debug.WriteLine($"Processing video: {video.FileName}");
                                System.Diagnostics.Debug.WriteLine($"Size: {video.ContentLength} bytes");
                                System.Diagnostics.Debug.WriteLine($"ContentType: {video.ContentType}");

                                // Check file size (100MB limit)
                                if (video.ContentLength > 100 * 1024 * 1024)
                                {
                                    var errorMsg = $"Videoul {video.FileName} depășește limita de 100MB.";
                                    System.Diagnostics.Debug.WriteLine($"ERROR: {errorMsg}");
                                    result.Errors.Add(errorMsg);
                                    continue;
                                }

                                // Validate video format
                                var allowedVideoTypes = new[] { "video/mp4", "video/avi", "video/mov", "video/wmv", "video/webm", "video/quicktime" };
                                var fileName = video.FileName.ToLower();
                                var isValidVideo = allowedVideoTypes.Any(type => video.ContentType.ToLower().Contains(type)) ||
                                                  fileName.EndsWith(".mp4") || fileName.EndsWith(".avi") ||
                                                  fileName.EndsWith(".mov") || fileName.EndsWith(".wmv") ||
                                                  fileName.EndsWith(".webm");

                                System.Diagnostics.Debug.WriteLine($"Video format validation: {isValidVideo}");

                                if (!isValidVideo)
                                {
                                    var errorMsg = $"Formatul video nu este suportat pentru {video.FileName}. Folosiți MP4, AVI, MOV, WMV sau WebM.";
                                    System.Diagnostics.Debug.WriteLine($"ERROR: {errorMsg}");
                                    result.Errors.Add(errorMsg);
                                    continue;
                                }

                                // Generate unique filename
                                string fileExtension = Path.GetExtension(video.FileName);
                                if (string.IsNullOrEmpty(fileExtension))
                                {
                                    fileExtension = ".mp4"; // Default extension if missing
                                    System.Diagnostics.Debug.WriteLine("No extension found, using .mp4 as default");
                                }

                                string savedFileName = $"{Guid.NewGuid()}{fileExtension}";
                                string filePath = Path.Combine(videosPath, savedFileName);

                                System.Diagnostics.Debug.WriteLine($"Generated filename: {savedFileName}");
                                System.Diagnostics.Debug.WriteLine($"Full path: {filePath}");
                                System.Diagnostics.Debug.WriteLine($"Directory exists: {Directory.Exists(videosPath)}");

                                // Save video directly without compression
                                System.Diagnostics.Debug.WriteLine("Starting file save...");
                                video.SaveAs(filePath);
                                System.Diagnostics.Debug.WriteLine("File saved successfully");

                                // Verify file was created
                                if (!File.Exists(filePath))
                                {
                                    var errorMsg = $"Eroare la salvarea videoului {video.FileName} - fișierul nu a fost creat.";
                                    System.Diagnostics.Debug.WriteLine($"ERROR: {errorMsg}");
                                    result.Errors.Add(errorMsg);
                                    continue;
                                }

                                var fileInfo = new FileInfo(filePath);
                                System.Diagnostics.Debug.WriteLine($"File created successfully. Size: {fileInfo.Length} bytes");

                                // Save to database
                                System.Diagnostics.Debug.WriteLine("Saving to database...");
                                var mediaRecord = new ProviderMediaDb
                                {
                                    providerId = profileId,
                                    mediaType = MediaType.Video,
                                    filePath = $"~/Resources/ProviderProfileMedia/{providerFolderName}/Videos/{savedFileName}",
                                    addedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                };

                                using (var db = new ProviderProfileMediaContext())
                                {
                                    db.Media.Add(mediaRecord);
                                    db.SaveChanges();
                                    System.Diagnostics.Debug.WriteLine("Database record saved successfully");
                                }

                                System.Diagnostics.Debug.WriteLine($"Video processed successfully: {video.FileName}");
                                result.VideosProcessed++;
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"ERROR processing video {video.FileName}: {ex.Message}");
                                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                                if (ex.InnerException != null)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                                }
                                result.Errors.Add($"Eroare la procesarea videoului {video.FileName}: {ex.Message}");
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Skipping null or empty video file");
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"Finished processing videos. Processed: {result.VideosProcessed}");
                }

                // Set final result
                if (result.ImagesProcessed > 0 || result.VideosProcessed > 0)
                {
                    result.Success = true;
                    result.Message = $"Media procesată cu succes: {result.ImagesProcessed} imagini, {result.VideosProcessed} videouri.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Nu s-a procesat niciun fișier media.";
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Eroare la procesarea media: {ex.Message}";
                result.Errors.Add(ex.ToString());
            }

            return result;
        }
    }
}