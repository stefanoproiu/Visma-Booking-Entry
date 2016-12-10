using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VismaBooking.Users.General;

namespace VismaBooking.Connection
{
    public static class Check
    {
        private static bool username_exists(string username, ref string error_string)
        {
            bool exists = false;
            //check if the e-mail exists either in Accounts or PendingAccounts
            if (MyConnection.countApparitions("SELECT * FROM Accounts WHERE Username='" + username + "'") > 0
                || MyConnection.countApparitions("SELECT * FROM PendingAccounts WHERE Username='" + username + "'") > 0)
                exists = true;
            return exists;
        }
        private static bool email_exists(string email, ref string error_string)
        {
            bool exists = false;
            //check if the e-mail exists either in Accounts or PendingAccounts
            if (MyConnection.countApparitions("SELECT * FROM Accounts WHERE Email='" + email + "'") > 0
                || MyConnection.countApparitions("SELECT * FROM PendingAccounts WHERE Email='" + email + "'") > 0)
                exists = true;
            return exists;
        }
        public static bool username_valid(string username, ref string error_string)
        {
            bool valid = true;
            //checking if the username is alphanumeric + chars '-' or '_' to prevent SQL Injection
            Regex alphanumeric = new Regex(@"^[a-zA-Z0-9\-_]*$");
            if (!(alphanumeric.IsMatch(username)))
            {
                valid = false;
                error_string += "This username contains invalid characters.\r\n";
            }
            if (username.Length < 8 || username.Length > 24)
            {
                valid = false;
                error_string += "The username should have no less than 8 characters and no more than 24 characters.\r\n";
            }
            //checking if the username exists in the database
            if (username_exists(username, ref error_string))
            {
                valid = false;
                error_string += "This username is already registered or pending confirmation.\r\n";
            }
            return valid;
        }
        public static bool all_chars_are_valid(string str, ref string error_string, string field)
        {
            //Chars that could lead into SQL Injection
            char[] invalid_chars = { ';', '=', '<', '>', ',' };
            bool valid = true;
            foreach (char c in str)
                if (invalid_chars.Contains(c))
                {
                    valid = false;
                    error_string += "The "+field+" field contains invalid characters.\r\n";
                    break;
                }
            return valid;
        }

        public static bool password_valid(string password, ref string error_string)
        {
            bool valid = true;
            if (password.Length < 8 || password.Length > 24)
            {
                valid = false;
                error_string += "The password should have no less than 8 characters and no more than 24 characters.\r\n";
            }
            if (!(all_chars_are_valid(password, ref error_string, "Password")))
                valid = false;
            return valid;
        }
        public static bool email_valid(string email, ref string error_string)
        {
            //checking if the e-mail has the correct format
            bool valid = true;
            if (!(email.IndexOf('@') > 0 && 
                email.IndexOf('@', email.IndexOf('@') + 1) <= 0 &&
                email.LastIndexOf('.') > email.IndexOf('@') &&
                email.LastIndexOf('.') != email.Length - 1 &&
                email.Substring(email.IndexOf('@')).Count(c => c == '.') <= 2 &&
                (email.IndexOf('.',email.IndexOf('@'))==email.LastIndexOf('.') ||
                email.LastIndexOf('.')-email.Substring(email.IndexOf('@')).IndexOf('.')>1)))
            {
                valid = false;
                error_string += "The E-mail does not have the correct format.\r\n";
            }
            if (email_exists(email,ref error_string))
            {
                valid = false;
                error_string += "This E-mail is already registered or pending confirmation.\r\n";
            }
            if (!(all_chars_are_valid(email, ref error_string, "E-mail")))
                valid = false;
            return valid;
        }
        public static bool identical_passwords(string password1, string password2, ref string error_string)
        {
            bool valid = true;
            if (password1 != password2)
            {
                valid = false;
                error_string += "The passwords must be identical.\r\n";
            }
            return valid;
        }
        public static bool phone_valid(string phonenumber, ref string error_string)
        {
            bool valid = true;
            Regex numeric = new Regex(@"^[0-9]*$");
                if (!(numeric.IsMatch(phonenumber)))
                {
                    valid = false;
                    error_string += "The phone number should only contain digits.\r\n";
                }
            if (phonenumber.Length != 10)
            {
                valid = false;
                error_string += "The phone number should have exactly 10 digits.\r\n";
            }
            return valid;
        }
        public static bool name_valid(string name, ref string error_string)
        {
            bool valid = true;
            Regex alpha = new Regex(@"^[a-zA-Z\s]*$");
            if (!(alpha.IsMatch(name)))
            {
                valid = false;
                error_string += "The name should only contain alphabetic characters.\r\n";
            }
            return valid;
        }
        public static bool timespan_valid(DateTime d1, DateTime d2,User user,ref string invalid_message)
        {
            //Check if the user can book a room for a certain time span
            bool valid = true;
            invalid_message = "This type of user cannot book meeting rooms for no more than ";
            switch (user.account_type)
            {
                case AccountType.BasicUser:
                    if ((d2 - d1).TotalHours >= 2 && (d2 - d1).TotalMinutes > 0)
                    { valid = false; invalid_message += "2 hours."; }
                    break;
                case AccountType.PremiumUser:
                    if ((d2 - d1).TotalHours >= 4 && (d2 - d1).TotalMinutes > 0)
                    { valid = false; invalid_message += "4 hours."; }
                    break;
                case AccountType.Manager:
                    if ((d2 - d1).TotalHours >= 2 && (d2 - d1).TotalMinutes > 0)
                    { valid = false; invalid_message += "8 hours."; }
                    break;
                default: break;
            }
            return valid;
        }
        private static int countWords(string text)
        {
            if (text == String.Empty) return 0;
            else
            {
                string[] separators = { "/" };
                string[] words = text.Split(separators, StringSplitOptions.None);
                return words.Length;
            }
        }
        public static bool rooms_booked_valid(User user, ref string invalid)
        {
            invalid = "This type of user cannot book more than ";
            bool valid = true;
            switch (user.account_type)
            {
                case AccountType.BasicUser:if (countWords(user.booked_rooms) > 1) valid = false; invalid += "1 room."; break;
                case AccountType.PremiumUser: if (countWords(user.booked_rooms) > 3) valid = false; invalid += "3 rooms."; break;
                case AccountType.Manager: if (countWords(user.booked_rooms) > 6) valid = false; invalid += "6 room."; break;
                default: break;
            }
            return valid;
        }
    }
}
