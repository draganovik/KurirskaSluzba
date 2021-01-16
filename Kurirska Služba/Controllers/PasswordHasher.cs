using System;
using System.Globalization;
using System.Text;

namespace KurirskaSluzba.Controllers
{
    internal class PasswordHasher
    {
        public static string Encode(string value)
        {
            System.Security.Cryptography.SHA512 hash = System.Security.Cryptography.SHA512.Create();
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] combined = encoder.GetBytes(value ?? "");
            string output = BitConverter.ToString(hash.ComputeHash(combined)).ToLower(new CultureInfo("en-US", false)).Replace("-", "", StringComparison.Ordinal);
            hash.Dispose();
            return output;

        }
    }
}
