using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenimentMD.Domain.Model
{
    public class UserSignUpData
    {   
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string password { get; set; }
        public string userRole { get; set; }
        public DateTime signUpTime { get; set; }
    }
}
