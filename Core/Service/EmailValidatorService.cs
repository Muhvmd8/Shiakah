using ServiceAbstraction;
using System.Text.RegularExpressions;

namespace Service
{
    public class EmailValidatorService:IEmailValidatorService
    {
        private static readonly Regex EmailRegex = new Regex(
        @"^[A-Za-z0-9]+([._%+\-]?[A-Za-z0-9]+)*@[A-Za-z0-9\-]+(\.[A-Za-z0-9\-]+)*\.[A-Za-z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public bool Validate(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            if (email.Length > 254)
                return false;

            var atIndex = email.LastIndexOf('@');
            if (atIndex <= 0 || atIndex == email.Length - 1)
                return false;

            var local = email.Substring(0, atIndex);
            if (local.Length > 64)
                return false;

            if (!EmailRegex.IsMatch(email))
                return false;

            return true;
        }
    }
}
