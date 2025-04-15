using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvenimentMD.Domain.Enums;

namespace EvenimentMD.Domain.Models.User
{
    public class UDbTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string firstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string phoneNumber { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Required]
        [Display(Name = "Role")]
        public  URole userRole{ get; set; }

        [Display(Name ="IP")]
        public string userIP { get; set; }

        [Display(Name = "Registration Date")]
        public DateTime signUpTime { get; set; }

        [Display(Name = "Last Login Date")]
        public DateTime LastLoginGateTime { get; set; }

    }
}
