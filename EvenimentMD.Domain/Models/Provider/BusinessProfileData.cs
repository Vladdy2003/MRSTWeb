using System;
using EvenimentMD.Domain.Models.User.UserActionResp;

namespace EvenimentMD.Domain.Models.Provider
{
    public class BusinessProfileData
    {
        public UserResp userId { get; set; }
        public string providerName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string website { get; set; }
        public string serviceType { get; set; }
        public string description { get; set; }
        public string facebookURL { get; set; }
        public string instagramURL { get; set; }
        public string tiktokURL { get; set; }
        public string logo { get; set; }
        public DateTime editedAt { get; set; }
    }
}
