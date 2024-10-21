using SalesMart.Domain.Common.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SalesMart.Infrastructure.Utilities
{
    public static class ValidatorService
    {
        public static bool isValidEmail(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return regex.IsMatch(email);
           
        }
        public static bool isValidPassword(string password)
        {
            return (!(password.All(char.IsLetter)) && !(password.All(char.IsNumber)));            
        }
    }
}
