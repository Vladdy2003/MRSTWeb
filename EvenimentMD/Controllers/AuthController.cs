using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EvenimentMD.BusinessLogic;
using EvenimentMD.BusinessLogic.Interface;
using EvenimentMD.Domain.Model;
using EvenimentMD.Web.Models.SignUp;
using EvenimentMD.Models.LogIn;
using EvenimentMD.Domain.Models;

namespace EvenimentMD.Controllers
{
    public class AuthController : Controller
    {
        private readonly ISignUp _signUp;
        private readonly ILogIn _logIn;

        public AuthController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _signUp = bl.GetSignUpBL();
            _logIn = bl.GetLoginBL();
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
        public ActionResult SignUp(UserDataSignUp signUp)
        {
            ViewBag.HideFooter = true;
            if (ModelState.IsValid)
            {
                var data = new UserSignUpData
                {
                    firstName = signUp.firstName,
                    lastName = signUp.lastName,
                    email = signUp.email,
                    phoneNumber = signUp.phoneNumber,
                    password = signUp.password,
                    userRole = signUp.userRole,
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
            return View("AuthIndex", signUp);
        }


        //LogIn

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(UserDataLogIn login)
        {
            ViewBag.HideFooter = true;
            if (ModelState.IsValid)
            {
                try
                {
                    var data = new UserLogInData
                    {
                        Password = login.Password,
                        Email = login.Email,
                        UserIp = Request.UserHostAddress ?? "localhost"
                    };

                    string token = _logIn.UserLogInLogic(data);

                    if (!string.IsNullOrEmpty(token))
                    {
                        // Autentificare reușită
                        // Poți stoca token-ul în sesiune/cookie pentru autentificare persistentă
                        Session["AuthToken"] = token;
                        TempData["SuccessMessage"] = "Autentificare reușită!";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Autentificare eșuată
                        TempData["ErrorMessage"] = "Email sau parolă incorecte. Vă rugăm să încercați din nou.";
                    }
                }
                catch (Exception ex)
                {
                    // Eroare la procesarea cererii
                    TempData["ErrorMessage"] = "A apărut o eroare în sistem. Vă rugăm să încercați mai târziu.";
                    // Loghează excepția pentru depanare
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

            // If something went wrong go back to LogIn page
            return View("AuthIndex", login);
        }

        public ActionResult LogOut()
        {
            // Clear the authentication token and any user-related session data
            Session.Clear();
            Session.Abandon();

            // Redirect to the login page
            return RedirectToAction("AuthIndex");
        }
    }
}