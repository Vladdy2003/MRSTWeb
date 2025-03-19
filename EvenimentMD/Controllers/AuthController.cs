using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EvenimentMD.BusinessLogic.Interface;
using EvenimentMD.Domain.Model;
using EvenimentMD.Models.SingUp;

namespace EvenimentMD.Controllers
{
    public class AuthController : Controller
    {
        private readonly ISignUp _signUp;

        public AuthController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _signUp = bl.GetSignUpBL();
        }

        // GET: Auth
        public ActionResult AuthIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(UserSignUpData signUp)
        {
            var data = new UserSignUpData
            {
                FirstName = signUp.FirstName,
                LastName = signUp.LastName,
                Email = signUp.Email,
                Password = signUp.Password,
                ConfirmPassword = signUp.ConfirmPassword,
                userRole = signUp.userRole
            };

            string token = _signUp.SignUpLogic(data);

            return View();
        }
    }
}