using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EvenimentMD.Domain.Models.Provider
{
    public class ProviderDbTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "User Id")]
        public int userId { get; set; }

        [Required]
        [Display(Name = "Provider Name")]
        public string providerName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string phoneNumber { get; set; }

        [Required]
        [Display(Name = "Service Type")]
        public string serviceType { get; set; }

        [Display(Name = "WebSite")]
        public string website { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string description { get; set; }

        [Display(Name = "Facebook URL")]
        public string facebookURL { get; set; }

        [Display(Name = "Instagram URL")]
        public string instagramURL { get; set; }

        [Display(Name = "TikTok URL")]
        public string tiktokURL { get; set; }

        [Display(Name = "Logo")]
        public string logo { get; set; }

        [Display(Name = "Edited At")]
        public DateTime editedAt { get; set; }

    }
}
