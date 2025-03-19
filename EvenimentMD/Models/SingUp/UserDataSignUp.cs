using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvenimentMD.Models.SingUp
{
    public class UserDataSignUp
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string userRole { get; set; }

    }
}