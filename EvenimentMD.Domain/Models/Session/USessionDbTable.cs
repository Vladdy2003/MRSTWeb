using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EvenimentMD.Domain.Models.Session
{
    public class USessionDbTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int userId { get; set; }
        public string cookie { get; set; }
        public DateTime isValidTime { get; set; }
    }
}
