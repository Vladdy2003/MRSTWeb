using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EvenimentMD.Domain.Enums;

namespace EvenimentMD.Models.Provider
{
    public class ProviderServicesViewModel
    {
        public string serviceName { get; set; }

        public float servicePrice { get; set; }

        [AllowHtml]
        public string serviceDescription { get; set; }

        public Currency currency { get; set; }

    }
}