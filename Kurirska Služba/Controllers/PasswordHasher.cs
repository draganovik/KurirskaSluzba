using System;
using System.Text;

namespace Kurirska_Služba.Controllers
{
    class PasswordHasher
    {
        public static string Encode(string value)
        {
            var hash = System.Security.Cryptography.SHA512.Create();
            var encoder = new ASCIIEncoding();
            var combined = encoder.GetBytes(value ?? "");
            return BitConverter.ToString(hash.ComputeHash(combined)).ToLower().Replace("-", "");

        }
    }
}
