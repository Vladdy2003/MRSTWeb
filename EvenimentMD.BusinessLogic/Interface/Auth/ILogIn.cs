using EvenimentMD.Domain.Models;
using EvenimentMD.Domain.Models.User.UserActionResp;

namespace EvenimentMD.BusinessLogic.Interface
{
    public interface ILogIn
    {
        UserResp LogInLogic(UserLogInData data);

        UserCookieResp GenerateCookieByUser(int id);
    }
}
