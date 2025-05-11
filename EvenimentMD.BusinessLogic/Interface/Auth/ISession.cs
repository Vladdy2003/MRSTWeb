using EvenimentMD.Domain.Models;
using EvenimentMD.Domain.Models.User.UserActionResp;

namespace EvenimentMD.BusinessLogic.Interface
{
    public interface ISession
    {
        UserResp LogInLogic(UserLogInData data);

        UserCookieResp GenerateCookieByUser(int id);

        UserResp GetUserByCookie(string sessionKey);
    }
}
