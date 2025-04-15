using EvenimentMD.BusinessLogic.Interface;
using EvenimentMD.Domain.Models;
using EvenimentMD.Models.LogIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvenimentMD.Controllers
{
    public class LogInController : Controller
    {
        private readonly ILogIn _logIn;
        public LogInController() 
        {
            var bl = new BusinessLogic.BusinessLogic();
            _logIn = bl.GetLoginBL();
        }
        // GET: LogIn
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(UserDataLogIn login)
        {
            var data = new UserLogInData
            {
                Password = login.Password,
                Email = login.Email,
                UserIp = "localhost"
            };

            string token = _logIn.UserLogInLogic(data);


            return View();
        }
    }
}