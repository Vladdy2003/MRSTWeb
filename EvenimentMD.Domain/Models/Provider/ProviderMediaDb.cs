using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EvenimentMD.Domain.Enums;

namespace EvenimentMD.Domain.Models.Provider
{
    public class ProviderMediaDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Provider Id")]
        public int providerId { get; set; }

        [Display(Name = "Media Type")]
        public MediaType mediaType { get; set; }

        [Display(Name = "File Path")]
        public string filePath { get; set; }

        [Display(Name = "Added At")]
        public string addedAt { get; set; }
    }
}
