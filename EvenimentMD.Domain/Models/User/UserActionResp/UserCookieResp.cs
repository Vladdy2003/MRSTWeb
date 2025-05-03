using System;
using System.Web;

namespace EvenimentMD.Domain.Models.User.UserActionResp
{
    public class UserCookieResp
    {
        public HttpCookie cookie { get; set; }
        public DateTime validUntil { get; set; }
        public int userId { get; set; }

    }
}
