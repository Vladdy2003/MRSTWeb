using System;
using System.Linq;
using System.Web.Mvc;
using EvenimentMD.BusinessLogic.Interface;
using EvenimentMD.Domain.Model;
using EvenimentMD.Domain.Models;
using EvenimentMD.Domain.Models.User.UserActionResp;
using System.Web;
using EvenimentMD.Domain.Enums;
using EvenimentMD.Web.Models.AuthModel;

namespace EvenimentMD.Controllers
{
    public class AuthController : Controller
    {
        private readonly ISignUp _signUp;
        private readonly ISession _session;

        public AuthController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _signUp = bl.GetSignUpBL();
            _session = bl.GetLogInBL();
        }

        // GET: Auth
        public ActionResult AuthIndex()
        {
            ViewBag.HideFooter = true;
            return View();
        }


        //SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(AuthModel signUpData)
        {
            ViewBag.HideFooter = true;
            if (ModelState.IsValid)
            {
                var data = new UserSignUpData
                {
                    firstName = signUpData.signUp.firstName,
                    lastName = signUpData.signUp.lastName,
                    email = signUpData.signUp.email,
                    phoneNumber = signUpData.signUp.phoneNumber,
                    password = signUpData.signUp.password,
                    userRole = signUpData.signUp.userRole,
                    signUpTime = DateTime.Now
                };
                try
                {
                    // Calling BL
                    string token = _signUp.SignUpLogic(data);
                    if (!string.IsNullOrEmpty(token))
                    {
                        // Înregistrare reușită
                        TempData["SuccessMessage"] = "Înregistrarea a fost realizată cu succes!";
                        return RedirectToAction("AuthIndex");
                    }
                    else
                    {
                        // Verifică dacă există un utilizator cu același email
                        if (_signUp.EmailExists(data.email))
                        {
                            TempData["ErrorMessage"] = "Există deja un utilizator înregistrat cu acest email.";
                        }
                        // Verifică dacă există un utilizator cu același număr de telefon
                        else if (_signUp.PhoneNumberExists(data.phoneNumber))
                        {
                            TempData["ErrorMessage"] = "Există deja un utilizator înregistrat cu acest număr de telefon.";
                        }
                        // Alte erori necunoscute
                        else
                        {
                            TempData["ErrorMessage"] = "A apărut o eroare în timpul procesării cererii. Vă rugăm să încercați din nou.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Eroare la procesarea cererii
                    TempData["ErrorMessage"] = "A apărut o eroare în sistem. Vă rugăm să încercați mai târziu.";
                    // Loghează excepția pentru depanare
                    System.Diagnostics.Debug.WriteLine($"Error in SignUp: {ex.Message}");
                }
            }
            else
            {
                // Model invalid - colectează erorile de validare
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                string errorMessage = string.Join(". ", errorMessages);
                TempData["ErrorMessage"] = "Datele introduse nu sunt valide. " + errorMessage;
            }
            // If something went wrong go back to SignUp page
            return View("AuthIndex", signUpData);
        }


        //LogIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(AuthModel logInData)
        {
            ViewBag.HideFooter = true;
            if (ModelState.IsValid)
            {
                try
                {
                    var data = new UserLogInData
                    {
                        password = logInData.logIn.password,
                        email = logInData.logIn.email,
                        lastLogInTime = DateTime.Now,
                    };

                    var resp = _session.LogInLogic(data);

                    if (resp.Status)
                    {
                        // Autentificare reușită
                        UserCookieResp respCookie = _session.GenerateCookieByUser(resp.userId);

                        HttpCookie cookie = respCookie.cookie;
                        ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Autentificare eșuată
                        switch (resp.Result)
                        {
                            case LoginResult.EmailNotFound:
                                TempData["ErrorMessage"] = "Nu există utilizator cu acest email.";
                                break;
                            case LoginResult.PasswordIncorrect:
                                TempData["ErrorMessage"] = "Parola introdusă este incorectă.";
                                break;
                            default:
                                TempData["ErrorMessage"] = "Autentificarea a eșuat. Vă rugăm să încercați din nou.";
                                break;
                        }
                        return View("AuthIndex", logInData);
                    }
                }
                catch (Exception ex)
                {
                    // Eroare la procesarea cererii
                    TempData["ErrorMessage"] = "A apărut o eroare în sistem. Vă rugăm să încercați mai târziu.";
                    System.Diagnostics.Debug.WriteLine($"Error in LogIn: {ex.Message}");
                }
            }
            else
            {
                // Model invalid - colectează erorile de validare
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                string errorMessage = string.Join(". ", errorMessages);
                TempData["ErrorMessage"] = "Datele introduse nu sunt valide. " + errorMessage;
            }

            // In caz de eroare sau date invalide, revenim la pagina de autentificare
            return View("AuthIndex", logInData);
        }

        public ActionResult LogOut()
        {
            // Sterge cookie-ul de autentificare X-KEY
            if (Request.Cookies["X-KEY"] != null)
            {
                var cookie = new HttpCookie("X-KEY");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            // Sterge sesiunea
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }
    }
}