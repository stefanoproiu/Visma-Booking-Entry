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

namespace VismaBooking.GUI
{
    public partial class MainStage : Form
    {
        public MainStage(User user, LoginForm form)
        {
            InitializeComponent();
            MyConnection.updateWithParameterDateTime("UPDATE BookingHistory SET Status='Ongoing' WHERE [Meeting Start]<@DateTime AND [Meeting End]>@DateTime AND Status<>'Cancelled'", DateTime.Now);
            MyConnection.updateWithParameterDateTime("UPDATE BookingHistory SET Status='Finished' WHERE [Meeting End]<@DateTime AND Status<>'Cancelled'", DateTime.Now);
            this.FormClosed += (s, e) => { form.Show(); this.Dispose(); };
            user.buildGUI(this);

        }
    }
}
