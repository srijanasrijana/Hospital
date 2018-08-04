using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic;
using System.Data;
using System.Text.RegularExpressions;

namespace BusinessLogic
{
   public class UserValidation
    {
        public static bool Validatename(string name)
        {
            if (Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                return true;
            else
                return false;
        }

        public static bool validatemiddlename(string middlename)
        {
            if (middlename.Length == 0)
                return true;
            else if (middlename.Length > 0)
            {
                if (Regex.IsMatch(middlename, @"^[a-zA-Z]+$"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else return false;
        }
        public static bool validatecontactnumber(string contactnumber)
        {

            if (Regex.IsMatch(contactnumber, "^[0-9]"))
                return true;
            else
                return false;

        }
        public static bool validateInt(string number)
        {

            if (Regex.IsMatch(number, "^\\d+$"))
                return true;
            else
                return false;

        }

        public static bool validateEmail(string email)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(email))
                return (true);
            else
                return (false);
        }

       /// <summary>
       /// returns true if the string is decimal.
       /// </summary>
       /// <param name="str"></param>
       /// <returns></returns>
        public static bool validDecimal(string str)
        {

            if (Regex.IsMatch(str, @"^\d{1,9}([.]\d{1,5})?$") || str == ".00")
                return true;
            else
                return false;
        }

        public static bool validatepassword(string password, string confirmpassword)
        {

            if (password == confirmpassword && password != "" && confirmpassword != "")
            {
                return true;
            }


            else
            {
                return false;
            }

        }

        public static bool validateusername(string username)
        {
            if (Regex.IsMatch(username, @"^[_\p{L}\p{N}]+$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //static bool IsValidSqlDatetime(string someval)
        //{
        //    bool valid = false;
        //    DateTime testDate = DateTime.MinValue;
        //    DateTime minDateTime = DateTime.MaxValue;
        //    DateTime maxDateTime = DateTime.MinValue;

        //    minDateTime = new DateTime(1753, 1, 1);
        //    maxDateTime = new DateTime(9999, 12, 31, 23, 59, 59, 997);

        //    if (DateTime.TryParse(someval, out testDate))
        //    {
        //        if (testDate >= minDateTime && testDate <= maxDateTime)
        //        {
        //            valid = true;
        //        }
        //    }

        //    return valid;
        //}
    }
}
