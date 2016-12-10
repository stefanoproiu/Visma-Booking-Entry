using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VismaBooking.Connection;
using VismaBooking.Users.General;
using VismaBooking.Users.Particular;
using System.Data.OleDb;
using VismaBooking.Rooms;
using VismaBooking.GUI;

namespace VismaBooking
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm(new BasicUser(),this);
            this.Hide();
            registerForm.Show();
        }
        User user;
        private void btnSignIn_Click(object sender, EventArgs e)
        {
            string error_string = String.Empty;
            if (Check.all_chars_are_valid(txt_user.Text, ref error_string, "Username") && (Check.all_chars_are_valid(txt_user.Text, ref error_string, "Password")))
            {
                object[] resultSet = MyConnection.getResultSet("SELECT * FROM Accounts WHERE " +
                    "[Username]='" + txt_user.Text + "' " +
                    "AND [Password]='" + txt_pwd.Text + "'");
                if (resultSet != null)
                {
                    switch ((int)resultSet[8])
                    {
                        case 0: user = new BasicUser(); break;
                        case 1: user = new PremiumUser(); break;
                        case 2: user = new Manager(); break;
                        case 3: user = new CEO(); break;
                        default: break;
                    }
                    user.id = (short)(int)resultSet[0];
                    user.username = (string)resultSet[1];
                    user.password = (string)resultSet[2];
                    user.first_name = (string)resultSet[3];
                    user.last_name = (string)resultSet[4];
                    user.company = (string)resultSet[5];
                    user.phone = (string)resultSet[6];
                    user.email = (string)resultSet[7];
                    if (!(resultSet[9] is System.DBNull))
                        user.booked_rooms = (string)resultSet[9];
                    else user.booked_rooms = String.Empty;
                    MainStage stage = new MainStage(user, this);
                    stage.Show();
                    this.Hide();
                    txt_pwd.Text = String.Empty;
                    txt_user.Text = String.Empty;
                }
                else
                {
                    MessageBox.Show("The username or the password is incorrect");
                    txt_pwd.Text = String.Empty;
                    txt_user.Text = String.Empty;
                }
            }
            else MessageBox.Show(error_string);
        }
        
    }
}
