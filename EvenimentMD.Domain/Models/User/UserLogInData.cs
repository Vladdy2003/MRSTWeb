using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenimentMD.Domain.Models
{
    public class UserLogInData
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserIp { get; set; }
    }
}
