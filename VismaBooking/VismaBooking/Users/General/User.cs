using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VismaBooking.Users.General
{
    public abstract class User:IUser
    {
        public abstract short id { get; set; }
        public abstract AccountType account_type { get; set; }
        public abstract string username { get; set; }
        public abstract string password { get; set; }
        public abstract string first_name { get; set; }
        public abstract string last_name { get; set; }
        public abstract string company { get; set; }
        public abstract string phone { get; set; }
        public abstract string email { get; set; }
        public abstract string booked_rooms { get; set; }

        public abstract void createAccountOptions(ComboBox b);
        public abstract void buildGUI(Form form);
        
    }
}
