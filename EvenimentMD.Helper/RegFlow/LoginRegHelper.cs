using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EvenimentMD.Helper.RegFlow
{
    public class LoginRegHelper
    {
        private const string salt = "OzPgYCVJx6SAhdO4Ai98ocxQEbE7cBYw";

        public static string HashPassword(string password) 
        {
            string saltedPassword = $"{password}{salt}";

            using (var sha256 = SHA256.Create())
            {
                // Convertește parola în bytes
                byte[] passwordBytes = Encoding.UTF8.GetBytes(saltedPassword);

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

        public static string GenerateToken(int userId)
        {
            // Combinație de userId și timestamp
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
    }
}
