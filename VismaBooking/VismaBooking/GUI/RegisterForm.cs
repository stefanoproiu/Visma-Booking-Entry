using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VismaBooking.Users.General;
using VismaBooking.Users.Particular;
using VismaBooking.Connection;
using VismaBooking;

namespace VismaBooking
{
    public partial class RegisterForm : Form
    {
        //the type of user given as a parameter determines which types of accounts can be created
        public RegisterForm(User user, Form form)
        {
            InitializeComponent();
            user.createAccountOptions(combo_acc_type);
            this.FormClosed += (s, e) => { form.Show(); this.Dispose(); };
        }
        //check if there are no empty textbox
        private bool not_empty(ref string error_string)
        {
            bool valid = true;
            List<TextBox> list = this.Controls.OfType<TextBox>().ToList();
            foreach (TextBox tb in list)
                if (tb.Text == String.Empty)
                {
                    valid = false;
                    error_string += "All fields must be completed.\r\n";
                    break;
                }
            return valid;
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            string error_string = String.Empty;
            bool[] input_validity =
            {
                Check.name_valid(txt_fname.Text, ref error_string),
                Check.name_valid(txt_lname.Text, ref error_string),
                Check.username_valid(txt_user.Text, ref error_string),
                Check.password_valid(txt_pwd.Text, ref error_string),
                Check.identical_passwords(txt_pwd.Text, txt_pwd2.Text, ref error_string),
                Check.email_valid(txt_email.Text, ref error_string),
                Check.phone_valid(txt_phone.Text, ref error_string),
                Check.all_chars_are_valid(txt_company.Text, ref error_string, "Company"),
                not_empty(ref error_string)
            };
            bool bool_result = true;
            foreach(bool b in input_validity) if (b==false) { bool_result = false;  break; }
            if (bool_result)
            {
                short account_type = 0;
                switch (combo_acc_type.Text)
                {
                    case "Basic User": account_type = 0; break;
                    case "Premium User": account_type = 1; break;
                    case "Manager": account_type = 2; break;
                    case "CEO": account_type = 3; break;
                    default: break;
                }
                MyConnection.updateQuery("INSERT INTO PendingAccounts " +
                    "([Username],[Password],[First Name],[Last Name],[Email],[Phone Number],[Company],[Account Type]) "+
                    "VALUES ("+
                    "'"+txt_user.Text+"',"+
                    "'"+txt_pwd.Text+"',"+
                    "'"+txt_fname.Text+"',"+
                    "'"+txt_lname.Text+"',"+
                    "'"+txt_email.Text+"',"+
                    "'"+txt_phone.Text+"',"+
                    "'"+txt_company.Text+"',"+
                    account_type.ToString()+
                    ")");
                MessageBox.Show("Your account has been successfully registered."+
                    Environment.NewLine+
                    "Please wait until a CEO validates your account.");
            }
            else MessageBox.Show(error_string, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
