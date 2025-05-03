using EvenimentMD.Web.Models.LogIn;
using EvenimentMD.Web.Models.SignUp;

namespace EvenimentMD.Web.Models.AuthModel
{
    public class AuthModel
    {
        public UserDataSignUp signUp { get; set; }
        public UserDataLogIn logIn { get; set; }

        public AuthModel()
        {
            signUp = new UserDataSignUp();
            logIn = new UserDataLogIn();
        }
    }
}