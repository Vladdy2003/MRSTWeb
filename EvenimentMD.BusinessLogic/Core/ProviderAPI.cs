using System;
using System.Data.Entity;
using System.Linq;
using EvenimentMD.BusinessLogic.DatabaseContext;
using EvenimentMD.Domain.Models.Provider;
using EvenimentMD.Domain.Models.User.UserActionResp;

namespace EvenimentMD.BusinessLogic.Core
{
    public class ProviderAPI
    {
        public bool EditBusinessProfileLogic(BusinessProfileData data, int userId)
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

                    using(var db = new ProviderProfileContext())
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

        public BusinessProfileData GetBusinessProfileLogic(int userId)
        {
            ProviderDbTable profile;

            using (var db = new ProviderProfileContext())
            {
                profile = db.Providers.FirstOrDefault(p => p.userId == userId);
            }

            if(profile != null)
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
    }
}
