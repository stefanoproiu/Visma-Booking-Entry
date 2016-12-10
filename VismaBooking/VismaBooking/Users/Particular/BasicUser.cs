using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VismaBooking.Users.General;
using VismaBooking.GUI;

namespace VismaBooking.Users.Particular
{
    //Can only make one booking at a time - ok
    //Has access to Classrooms only - ok
    //Cannot view when a certain room will be open to booking - ok
    class BasicUser:User
    {
        private short _id;
        public override short id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }
        private AccountType _account_type;
        public override AccountType account_type
        {
            get
            {
                return AccountType.BasicUser;
            }

            set
            {
                _account_type = value;
            }
        }
        private string _username;
        public override string username
        {
            get
            {
                return _username;
            }

            set
            {
                _username = value;
            }
        }
        private string _password;
        public override string password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
            }
        }
        private string _first_name;
        public override string first_name
        {
            get
            {
                return _first_name;
            }

            set
            {
                _first_name = value;
            }
        }
        private string _last_name;
        public override string last_name
        {
            get
            {
                return _last_name;
            }

            set
            {
                _last_name = value;
            }
        }
        private string _company;
        public override string company
        {
            get
            {
                return _company;
            }

            set
            {
                _company = value;
            }
        }
        private string _phone;
        public override string phone
        {
            get
            {
                return _phone;
            }

            set
            {
                _phone = value;
            }
        }
        private string _email;
        public override string email
        {
            get
            {
                return _email;
            }

            set
            {
                _email = value;
            }
        }
        private string _booked_rooms;
        public override string booked_rooms
        {
            get
            {
                return _booked_rooms;
            }

            set
            {
                _booked_rooms = value;
            }
        }
        
        public override void createAccountOptions(ComboBox comboBox)
        {
            comboBox.Items.Add("Basic User");
            comboBox.SelectedIndex = 0;
        }
        public override void buildGUI(Form form)
        {
            var tabs = form.Controls.OfType<TabControl>().ToList();
            GUIBuilder.addRoomsTab(tabs[0], this);
            GUIBuilder.addMenuBar(form,this);
            GUIBuilder.addMyBookingsTab(tabs[0], this);
            form.BackColor = System.Drawing.Color.Blue;
            foreach (TabPage tp in tabs[0].TabPages)
                tp.BackColor = System.Drawing.Color.AliceBlue;
        }
    }
}
