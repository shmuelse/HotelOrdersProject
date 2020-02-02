using System;
using System.Text.RegularExpressions;

namespace Utilities
{
    public static class Validation
    {
        //check if the Phone number are valid or not.
        public static bool IsValidPhoneNumber(string phoneNumber) =>
            Regex.IsMatch(phoneNumber, @"^\+?(972|0)(\-)?0?(([23489]{1}\d{7})|[5]{1}\d{8})$");

        //check if the ID is valid or not
        public static bool IsValidId(string idNum)
        {
            // Validate correct input
            if (!Regex.IsMatch(idNum, @"^\d{5,9}$"))
                return false;

            // The number is too short - add leading 0000
            if (idNum.Length < 9)
                while (idNum.Length < 9)
                    idNum = '0' + idNum;

            // CHECK THE ID NUMBER
            var counter = 0;
            for (var i = 0; i < 9; i++)
            {
                var incNum = Convert.ToInt32(idNum[i].ToString());
                incNum *= (i % 2) + 1;
                if (incNum > 9)
                    incNum -= 9;
                counter += incNum;
            }

            return counter % 10 == 0;
        }

        //check if the name are valid
        public static bool IsValidName(string name) => Regex.IsMatch(name, "^([^20]|[a-zA-Zא-ת]){2,35}$");

        //check if the email address are Valid or not.
        public static bool IsValidEmail(string email) =>
            Regex.IsMatch(email, "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");

        /// <summary>
        /// Checks whether a password has at least one uppercase letter, whether there
        /// is at least one digit, and whether the password has exactly 8 digits
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsValidPassword(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            return hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMinimum8Chars.IsMatch(password);
        }

        /// <summary>
        /// * Must start with a letter or number
        /// * Must consist of between 3 to 15 allowed characters
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool IsValidUserName(string userName) =>
            Regex.IsMatch(userName, @"^ (?=[A - Za - z0 - 9])[A-Za-z0-9._()\[\]-]{3,15}$");


    }
}