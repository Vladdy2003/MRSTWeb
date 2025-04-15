using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvenimentMD.Web.Models.SignUp
{
    public class UserDataSignUp
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public string userRole { get; set; }

    }
}