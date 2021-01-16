using System.Text.RegularExpressions;

namespace KurirskaSluzba.Controllers
{
    internal class InputValidation
    {
        public static bool IsTextASCII(string str)
        {
            Regex reg = new Regex("[^0-9a-z]");
            return reg.IsMatch(str);
        }
        public static bool IsNumeric(string str)
        {
            Regex reg = new Regex("[^0-9]");
            return reg.IsMatch(str);
        }
        public static bool IsPhone(string str)
        {
            Regex reg = new Regex("[^0-9+]");
            return reg.IsMatch(str);
        }
    }
}
