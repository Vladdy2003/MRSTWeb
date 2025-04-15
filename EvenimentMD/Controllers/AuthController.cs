using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EvenimentMD.BusinessLogic.Interface;
using EvenimentMD.Domain.Model;
using EvenimentMD.Web.Models.SingUp;

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
            ViewBag.HideFooter = true;
            return View();
        }

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
    }
}