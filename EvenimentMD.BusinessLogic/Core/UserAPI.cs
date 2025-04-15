using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using EvenimentMD.Domain.Model;
using EvenimentMD.Domain.Enums;
using EvenimentMD.BusinessLogic.DatabaseContext;
using EvenimentMD.Domain.Models.User;
using EvenimentMD.Domain.Models;

namespace EvenimentMD.BusinessLogic.Core
{
    public class UserAPI
    {
        public string UserSignUpLogic(UserSignUpData data)
        {
            // Validează datele primite
            if (data == null ||
                string.IsNullOrEmpty(data.firstName) ||
                string.IsNullOrEmpty(data.lastName) ||
                string.IsNullOrEmpty(data.email) ||
                string.IsNullOrEmpty(data.password))
            {
                return null; // Returnează null dacă datele sunt invalide
            }

            try
            {
                // Creează contextul de bază de date
                using (var dbContext = new UserContext())
                {
                    // Verifică dacă există deja un utilizator cu același email
                    var existingEmailUser = dbContext.Users.FirstOrDefault(u => u.email == data.email);
                    if (existingEmailUser != null)
                    {
                        return null; // Utilizatorul cu acest email există deja
                    }

                    // Verifică dacă există deja un utilizator cu același număr de telefon
                    if (!string.IsNullOrEmpty(data.phoneNumber))
                    {
                        var existingPhoneUser = dbContext.Users.FirstOrDefault(u => u.phoneNumber == data.phoneNumber);
                        if (existingPhoneUser != null)
                        {
                            return null; // Utilizatorul cu acest număr de telefon există deja
                        }
                    }

                    // Hashing pentru parolă
                    string hashedPassword = HashPassword(data.password);

                    // Parsează rolul utilizatorului
                    URole role;
                    if (!Enum.TryParse(data.userRole, out role))
                    {
                        role = URole.organizer;
                    }

                    // Creează un nou utilizator
                    var newUser = new UDbTable
                    {
                        firstName = data.firstName,
                        lastName = data.lastName,
                        email = data.email,
                        phoneNumber = data.phoneNumber,
                        password = hashedPassword,
                        userRole = role,
                        signUpTime = DateTime.Now,
                        userIP = System.Web.HttpContext.Current?.Request.UserHostAddress ?? "Unknown",
                        LastLoginGateTime = DateTime.Now
                    };

                    // Adaugă utilizatorul în baza de date
                    dbContext.Users.Add(newUser);
                    dbContext.SaveChanges();

                    // Generează un token pentru utilizator
                    string token = GenerateToken(newUser.Id);

                    return token;
                }
            }
            catch (Exception ex)
            {
                // Loghează excepția
                Console.WriteLine($"Error in UserSignUpLogic: {ex.Message}");
                return null;
            }
        }

        // Metodă pentru hasharea parolei
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Convertește parola în bytes
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Calculează hash-ul
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Convertește hash-ul în string hexadecimal
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        // Metodă pentru generarea unui token
        private string GenerateToken(int userId)
        {
            // Combinație de userId și timestamp pentru a face token-ul unic
            string tokenData = $"{userId}:{DateTime.UtcNow.Ticks}";

            // Hashează tokenData pentru a crea token-ul
            using (var sha256 = SHA256.Create())
            {
                byte[] tokenBytes = Encoding.UTF8.GetBytes(tokenData);
                byte[] hashBytes = sha256.ComputeHash(tokenBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
        public string UserLogInLogicAction(UserLogInData data)
        {
            return "token-key";
        }
    }
}