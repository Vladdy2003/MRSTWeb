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
        public string email { get; set; }
        public string password { get; set; }
        public string userIp { get; set; }
        public DateTime lastLogInTime { get; set; }
    }
}
