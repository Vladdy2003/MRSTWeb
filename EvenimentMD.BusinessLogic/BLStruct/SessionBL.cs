using System;
using System.Web;
using EvenimentMD.BusinessLogic.Core;
using EvenimentMD.BusinessLogic.Interface;
using EvenimentMD.Domain.Models;
using EvenimentMD.Domain.Models.User.UserActionResp;

namespace EvenimentMD.BusinessLogic.BLStruct
{
    public class SessionBL : UserAPI, ISession
    {

        public UserResp LogInLogic(UserLogInData data)
        {
            return UserLogInLogic(data);
        }

        public UserCookieResp GenerateCookieByUser(int id)
        {
            return GenerateCookieByUserAction(id);
        }

        public UserResp GetUserByCookie(string sessionKey)
        {
            return GetUserByCookieAction(sessionKey);
        }
    }
}