using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvenimentMD.Domain.Enums;

namespace EvenimentMD.Domain.Models.Provider
{
    public class ProviderServicesDbTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Provider Id")]
        public int providerId { get; set; }

        [Display(Name = "Service Name")]
        public string serviceName { get; set; }

        [Display(Name = "Description")]
        public string serviceDescription { get; set; }

        [Display(Name = "Price")]
        public float servicePrice { get; set; }

        [Display(Name = "Currency")]
        public Currency currency { get; set; }
    }
}
