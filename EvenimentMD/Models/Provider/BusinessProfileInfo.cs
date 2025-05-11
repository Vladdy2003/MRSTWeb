using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvenimentMD.Models.Provider
{
    public class BusinessProfileInfo
    {
        public string providerName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string website { get; set; }
        public string serviceType { get; set; }

        [AllowHtml]
        public string description { get; set; }
        public string facebookURL { get; set; }
        public string instagramURL { get; set; }
        public string tiktokURL { get; set; }
        public string logo { get; set; }
    }
}