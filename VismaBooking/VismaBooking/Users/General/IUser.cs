using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VismaBooking.Users.General
{
    public interface IUser
    {
        short id {get;set;}
        AccountType account_type { get; set; }
        string username { get; set; }
        string password { get; set; }
        string first_name { get; set; }
        string last_name { get; set; }
        string company { get; set; }
        string phone { get; set; }
        string email { get; set; }
        string booked_rooms { get; set; }
        void createAccountOptions(ComboBox comboBox);
        void buildGUI(Form form);
    }
    public enum AccountType
    {
        BasicUser,PremiumUser,Manager,CEO
    }
}
