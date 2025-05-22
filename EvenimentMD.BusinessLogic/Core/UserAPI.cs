using System;
using System.Linq;
using EvenimentMD.Domain.Model;
using EvenimentMD.Domain.Enums;
using EvenimentMD.BusinessLogic.DatabaseContext;
using EvenimentMD.Domain.Models.User;
using EvenimentMD.Domain.Models;
using EvenimentMD.Helper.RegFlow;
using EvenimentMD.Domain.Models.User.UserActionResp;
using System.Web;
using EvenimentMD.Helper.Session;
using EvenimentMD.Domain.Models.Session;
using System.Data.Entity;
using EvenimentMD.Domain.Models.Provider;
using System.Collections.Generic;

namespace EvenimentMD.BusinessLogic.Core
{
    public class UserAPI
    {
        public string UserSignUpLogic(UserSignUpData data)
        {
            // Validează datele primite
            if (data == null ||
                string.IsNullOrEmpty(data.firstName) ||
                string.IsNullOrEmpty(data.lastName) ||
                string.IsNullOrEmpty(data.email) ||
                string.IsNullOrEmpty(data.password))
            {
                return null; // Returnează null dacă datele sunt invalide
            }

            try
            {
                // Creează contextul de bază de date
                using (var dbContext = new UserContext())
                {
                    // Verifică dacă există deja un utilizator cu același email
                    var existingEmailUser = dbContext.Users.FirstOrDefault(u => u.email == data.email);
                    if (existingEmailUser != null)
                    {
                        return null; // Utilizatorul cu acest email există deja
                    }

                    // Verifică dacă există deja un utilizator cu același număr de telefon
                    if (!string.IsNullOrEmpty(data.phoneNumber))
                    {
                        var existingPhoneUser = dbContext.Users.FirstOrDefault(u => u.phoneNumber == data.phoneNumber);
                        if (existingPhoneUser != null)
                        {
                            return null; // Utilizatorul cu acest număr de telefon există deja
                        }
                    }

                    // Hashing pentru parolă
                    string hashedPassword = LoginRegHelper.HashPassword(data.password);

                    // Parsează rolul utilizatorului
                    URole role;
                    if (!Enum.TryParse(data.userRole, out role))
                    {
                        role = URole.organizer;
                    }

                    // Creează un nou utilizator
                    var newUser = new UDbTable
                    {
                        firstName = data.firstName,
                        lastName = data.lastName,
                        email = data.email,
                        phoneNumber = data.phoneNumber,
                        password = hashedPassword,
                        userRole = role,
                        signUpTime = DateTime.Now,
                        lastUserIP = System.Web.HttpContext.Current?.Request.UserHostAddress ?? "Unknown",
                        LastLoginGateTime = DateTime.Now
                    };

                    // Adaugă utilizatorul în baza de date
                    dbContext.Users.Add(newUser);
                    dbContext.SaveChanges();

                    // Generează un token pentru utilizator
                    string token = LoginRegHelper.GenerateToken(newUser.Id);

                    return token;
                }
            }
            catch (Exception ex)
            {
                // Loghează excepția
                Console.WriteLine($"Error in UserSignUpLogic: {ex.Message}");
                return null;
            }
        }

        internal UserResp UserLogInLogic(UserLogInData credentials)
        {
            try
            {
                string hashedPassword = LoginRegHelper.HashPassword(credentials.password);
                UDbTable user;

                using (var dbContext = new UserContext())
                {
                    // Verifică dacă utilizatorul există în baza de date
                    user = dbContext.Users.FirstOrDefault(u => u.email == credentials.email);

                    // Daca utilizatorul nu există, se returnează EmailNotFound
                    if (user == null)
                    {
                        return new UserResp 
                        { 
                            Status = false, 
                            Result = LoginResult.EmailNotFound 
                        };
                    }

                    // Daca parola nu estec corectă, se returnează PasswordIncorrect
                    if (user.password != hashedPassword)
                    {
                        return new UserResp 
                        { 
                            Status = false,
                            Result = LoginResult.PasswordIncorrect 
                        };
                    }
                }

                // Ambele metode de mai sus sunt corecte, deci utilizatorul este validat
                // Se actualizează ora ultimei autentificări și IP-ul utilizatorului
                user.LastLoginGateTime = DateTime.Now;
                user.lastUserIP = System.Web.HttpContext.Current?.Request.UserHostAddress ?? "Unknown";

                using (var db = new UserContext())
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }

                // Generează un cookie pentru utilizator
                return new UserResp 
                { 
                    Status = true,
                    Result = LoginResult.Success,
                    userId = user.Id,
                };
            }
            catch (Exception ex)
            {
                // Loghează excepția
                Console.WriteLine($"Error in UserLogInLogic: {ex.Message}");
                return new UserResp { 
                    Status = false,
                    Result = LoginResult.Error 
                };
            }
        }

        internal UserCookieResp GenerateCookieByUserAction(int userId)
        {
            var cookieString = new HttpCookie("X-KEY")
            {
                Value = CookieGenerator.Create(userId + System.Web.HttpContext.Current?.Request.UserHostAddress)
            };

            var dateTime = DateTime.Now;

            USessionDbTable session;

            using (var db = new SessionContext())
            {
                session = db.Sessions.FirstOrDefault(u => u.userId == userId);
            }

            if (session != null)
            {
                // Update existing table
                session.cookie = cookieString.Value;
                session.isValidTime = dateTime.AddHours(3);

                using (var db = new SessionContext())
                {
                    db.Entry(session).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                // Insert table
                session = new USessionDbTable()
                {
                    userId = userId,
                    cookie = cookieString.Value,
                    isValidTime = dateTime.AddHours(3)
                };

                using (var db = new SessionContext())
                {
                    db.Sessions.Add(session);
                    db.SaveChanges();
                }
            }

            return new UserCookieResp() {userId = userId, cookie = cookieString, validUntil = dateTime };
        }

        internal UserResp GetUserByCookieAction(string cookieKey)
        {
            USessionDbTable session;
            UDbTable user;
            using (var db = new SessionContext())
            {
                session = db.Sessions.FirstOrDefault(s => s.cookie.Contains(cookieKey));
            }

            if(session != null)
            {
                using (var db = new UserContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Id == session.userId);
                }

                if(user != null)
                {
                    return new UserResp()
                    {
                        Status = true,
                        userId = user.Id,
                        role = user.userRole,
                        firstName = user.firstName,
                        lastName = user.lastName
                    };
                }
            }

            
            return new UserResp()
            {
                Status = false,
            };
        }

        public List<ProviderDbTable> GetAllProvidersLogic()
        {
            List<ProviderDbTable> providers = new List<ProviderDbTable>();

            using (var db = new ProviderProfileContext())
            {
                providers = db.Providers.ToList();
            }

            return providers;
        }

        public Dictionary<int, string> GetProviderFirstImagesLogic(List<ProviderDbTable> providers)
        {
            var providerFirstImages = new Dictionary<int, string>();

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

            return providerFirstImages;
        }

        public ProviderDbTable GetProviderByIdLogic(int id)
        {
            ProviderDbTable provider = null;

            using (var db = new ProviderProfileContext())
            {
                provider = db.Providers.FirstOrDefault(p => p.Id == id);
            }

            return provider;
        }

        public List<ProviderMediaModel> GetProviderImagesLogic(int providerId)
        {
            List<ProviderMediaModel> providerImages = new List<ProviderMediaModel>();

            using (var db = new ProviderProfileMediaContext())
            {
                providerImages = db.Media
                    .Where(m => m.providerId == providerId && m.mediaType == MediaType.Image)
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

            return providerImages;
        }

        public List<ProviderServicesData> GetProviderServicesForServicePageLogic(int providerId)
        {
            List<ProviderServicesData> providerServices = new List<ProviderServicesData>();

            try
            {
                using (var db = new ProviderProfileServicesContext())
                {
                    var services = db.Services.Where(s => s.providerId == providerId).ToList();

                    foreach (var service in services)
                    {
                        providerServices.Add(new ProviderServicesData
                        {
                            Id = service.Id,
                            providerId = service.providerId,
                            serviceName = service.serviceName,
                            serviceDescription = service.serviceDescription,
                            servicePrice = service.servicePrice,
                            currency = service.currency
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading provider services: {ex.Message}");
            }

            return providerServices;
        }
    }
}