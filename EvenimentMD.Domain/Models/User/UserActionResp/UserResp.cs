using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvenimentMD.Domain.Enums;

namespace EvenimentMD.Domain.Models.User.UserActionResp
{
    public class UserResp
    {
        public bool Status { get; set; }
        public LoginResult Result { get; set; }
        public int userId { get; set; }

    }
}
