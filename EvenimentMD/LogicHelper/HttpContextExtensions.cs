using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EvenimentMD.Domain.Models.User.UserActionResp;

namespace EvenimentMD.LogicHelper
{
    public static class HttpContextExtensions
    {
        public static UserResp GetUserProfile(this HttpContext context)
        {
            return (UserResp)context?.Session["__SessionObject"];
        }

        public static void SetUserProfile(this HttpContext context, UserResp data)
        {
            context.Session.Add("__SesionObject", data);
        }
    }
}