using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvenimentMD.Domain.Enums;

namespace EvenimentMD.Domain.Models.Provider
{
    public class ProviderServicesData
    {
        public int Id { get; set; }

        public int providerId { get; set; }

        public string serviceName { get; set; }

        public float servicePrice { get; set; }

        public string serviceDescription { get; set; }

        public Currency currency { get; set; }
    }
}
