using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using VismaBooking.Connection;
using VismaBooking.Users.General;
using VismaBooking.Users.Particular;
using VismaBooking.Rooms;

namespace VismaBooking.GUI
{
    public static class GUIBuilder
    {
        public static void addMenuBar(Form form, User user)
        {
            MainMenu mainMenu = new MainMenu();
            MenuItem userInfo = new MenuItem("User Info");
            userInfo.Click += (s, e) =>
            {
                string user_info = String.Empty;
                user_info += "User ID: " + user.id.ToString() + Environment.NewLine +
                "Username: " + user.username + Environment.NewLine +
                "Password: You're not allowed to view this." + Environment.NewLine +
                "First Name: " + user.first_name + Environment.NewLine +
                "Last Name: " + user.last_name + Environment.NewLine +
                "Company: " + user.company + Environment.NewLine +
                "Phone Number: " + user.phone + Environment.NewLine +
                "E-mail: " + user.email + Environment.NewLine +
                "Account Type: " + user.account_type.ToString() + Environment.NewLine +
                "Booked Rooms: " + user.booked_rooms;
                MessageBox.Show(user_info);
            };
            MenuItem logOut = new MenuItem("Log Out");
            logOut.Click += (s, e) => form.Close();
            switch(user.account_type)
            {
                case AccountType.Manager:
                case AccountType.CEO:
                    {
                        MenuItem regAccount = new MenuItem("Register New User");
                        regAccount.Click += (s, e) =>
                        {
                            RegisterForm regForm = new RegisterForm(user, form);
                            regForm.Show();
                            form.Hide();
                        };
                        mainMenu.MenuItems.Add(regAccount);
                        break;
                    }
                default:break;
            }
            mainMenu.MenuItems.Add(userInfo);
            mainMenu.MenuItems.Add(logOut);
            form.Menu = mainMenu;
        }

        public static void addManageRoomsTab(TabControl tabControl)
        {
            TabPage tabPage = new TabPage();
            tabPage.Text = "Manage Rooms";

            Label meetingRoomsLabel = new Label();
            meetingRoomsLabel.Text = "Meeting Rooms: ";
            meetingRoomsLabel.Parent = tabPage;
            meetingRoomsLabel.Location = new Point(20, 10);

            string statement = "SELECT [Room ID],[Room Name],[Equipment],[Room Type],[Maximum Capacity],[Floor] FROM Rooms";
            DataGridView roomsView = new DataGridView();
            roomsView.Parent = tabPage;
            roomsView.ReadOnly = true;
            roomsView.Size = new Size(650, 200);
            roomsView.Location = new Point(20, 35);
            CTRL.refreshDGV(roomsView, statement);
            
            Label[] roomInfoLabel = new Label[6];
            for(int i = 0; i < 6; i++)
            {
                roomInfoLabel[i] = new Label();
                roomInfoLabel[i].Parent = tabPage;
                roomInfoLabel[i].Location = new Point(20, 275 + 25 * i);
                roomInfoLabel[i].AutoSize = true;
            }
            roomInfoLabel[0].Text = "Room Name:";
            roomInfoLabel[1].Text = "Room Picture:";
            roomInfoLabel[2].Text = "Equipment:";
            roomInfoLabel[3].Text = "Room Type:";
            roomInfoLabel[4].Text = "Max Capacity:";
            roomInfoLabel[5].Text = "Floor:";

            TextBox[] roomInfoTextBox = new TextBox[3];
            for(int i=0; i < 3; i++)
            {
                roomInfoTextBox[i] = new TextBox();
                roomInfoTextBox[i].Parent = tabPage;
                roomInfoTextBox[i].Location = new Point(150, 275 + 25 * i);
            }
            roomInfoTextBox[1].ReadOnly = true;
            roomInfoTextBox[1].Click += (s, e) =>
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                roomInfoTextBox[1].Text = ofd.FileName;
            };

            NumericUpDown[] numUpDown = new NumericUpDown[3];
            for(int i = 0; i < 3; i++)
            {
                numUpDown[i] = new NumericUpDown();
                numUpDown[i].Parent = tabPage;
                numUpDown[i].Location = new Point(150, 350 + 25 * i);
            }
            numUpDown[0].Maximum = 2;
            numUpDown[1].Minimum = 1;
            numUpDown[1].Maximum = 100;
            numUpDown[2].Minimum = 1;
            numUpDown[2].Maximum = 10;
            
            Button buttonModify = new Button();
            buttonModify.Text = "Modify Room";
            buttonModify.Size = new Size(90, buttonModify.Size.Height);
            buttonModify.Parent = tabPage;
            buttonModify.Location = new Point(300, 275);
            buttonModify.Click += (s, e) =>
            {
                bool valid = true;
                foreach (TextBox tb in tabPage.Controls.OfType<TextBox>())
                    if (tb.Text == "")
                    {
                        valid = false; break;
                    }
                if (valid)
                {
                        MyConnection.updateWithParameter("UPDATE Rooms SET [Room Name]='" + roomInfoTextBox[0].Text + "'," +
                            "[Room Picture]=@picture," +
                            "[Equipment]='" + roomInfoTextBox[2].Text + "'," +
                            "[Room Type]="+numUpDown[0].Value.ToString()+"," +
                            "[Maximum Capacity]=" + numUpDown[1].Value.ToString() + "," +
                            "[Floor]=" + numUpDown[2].Value.ToString() +
                            " WHERE [Room ID]=" + roomsView.Rows[(int)roomsView.CurrentCell.RowIndex].Cells[0].Value.ToString(),
                            (CTRL.imageToByteArray(Image.FromFile(roomInfoTextBox[1].Text))));
                        MessageBox.Show("The room has been successfully modified.");
                        CTRL.refreshDGV(roomsView, statement);
                }
                else MessageBox.Show("All fields must be completed.");

            };

            Button buttonAdd = new Button();
            buttonAdd.Text = "Add Room";
            buttonAdd.Size = new Size(90, buttonAdd.Size.Height);
            buttonAdd.Parent = tabPage;
            buttonAdd.Location = new Point(300, 300);
            buttonAdd.Click += (s, e) =>
            {
                bool valid = true;
                foreach (TextBox tb in tabPage.Controls.OfType<TextBox>())
                    if (tb.Text == "")
                    {
                        valid = false; break;
                    }
                if (valid)
                {
                    MyConnection.updateWithParameter("INSERT INTO Rooms ([Room Name],[Room Picture],[Equipment],[Room Type],[Maximum Capacity],[Floor]) VALUES (" +
                        "'" + roomInfoTextBox[0].Text + "'," +
                        "@picture," +
                        "'" + roomInfoTextBox[2].Text + "'," +
                        "" + numUpDown[0].Value.ToString() + "," +
                        "" + numUpDown[1].Value.ToString() + "," +
                        "" + numUpDown[2].Value.ToString() + ")",CTRL.imageToByteArray(Image.FromFile(roomInfoTextBox[1].Text)));
                    MessageBox.Show("The room has been successfully added.");
                    CTRL.refreshDGV(roomsView, statement);
                }
                else MessageBox.Show("All fields must be completed.");
            };

            Button buttonDelete = new Button();
            buttonDelete.Text = "Delete Room";
            buttonDelete.Size = new Size(90, buttonAdd.Size.Height);
            buttonDelete.Parent = tabPage;
            buttonDelete.Location = new Point(300, 325);
            buttonDelete.Click += (s, e) =>
            {
                MyConnection.updateQuery("DELETE FROM Rooms WHERE [Room ID]=" + roomsView.Rows[(int)roomsView.CurrentCell.RowIndex].Cells[0].Value.ToString());
                MessageBox.Show("This room has been successfully deleted.");
                CTRL.refreshDGV(roomsView, statement);
            };

            tabPage.Parent = tabControl;
            tabControl.SelectedIndexChanged += (s, e) =>
            {
                CTRL.refreshDGV(roomsView, statement);
            };
        }
        public static void addManageUsersTab(TabControl tabControl, User user)
        {
            TabPage tabPage = new TabPage();
            tabPage.Text = "Manage Users";

            Label pendingUsersLabel = new Label();
            pendingUsersLabel.Text = "Pending Users:";
            pendingUsersLabel.Parent = tabPage;
            pendingUsersLabel.Location = new Point(20, 10);

            string statement = "SELECT * FROM PendingAccounts";
            DataGridView pendingUsers = new DataGridView();
            pendingUsers.Parent = tabPage;
            pendingUsers.ReadOnly = true;
            pendingUsers.Size = new Size(650, 200);
            pendingUsers.Location = new Point(20, 35);
            CTRL.refreshDGV(pendingUsers, "Accept", "Accept", "Decline","Decline", statement);


            string statement2 = "SELECT * FROM Accounts";
            DataGridView users = new DataGridView();
            users.Parent = tabPage;
            users.ReadOnly = true;
            users.Size = new Size(650, 200);
            users.Location = new Point(20, 275);
            CTRL.refreshDGV(users, "Delete Account", "Delete", "Promote", "Promote", "Demote", "Demote", statement2);
            
            pendingUsers.CellContentClick += (s, e) =>
            {
                DataGridView dgv = s as DataGridView;
                User toValidate = new BasicUser();
                try
                {
                    switch (dgv.Rows[e.RowIndex].Cells[8].Value.ToString())
                    {
                        case "0": toValidate = new BasicUser(); break;
                        case "1": toValidate = new PremiumUser(); break;
                        case "2": toValidate = new Manager(); break;
                        case "3": toValidate = new CEO(); break;
                        default: break;
                    }
                    DataGridViewRow row = dgv.Rows[e.RowIndex];
                    switch (dgv.Columns[e.ColumnIndex].HeaderText)
                    {
                        case "Accept":
                            toValidate.username = row.Cells[1].Value.ToString();
                            toValidate.password = row.Cells[2].Value.ToString();
                            toValidate.first_name = row.Cells[3].Value.ToString();
                            toValidate.last_name = row.Cells[4].Value.ToString();
                            toValidate.company = row.Cells[5].Value.ToString();
                            toValidate.phone = row.Cells[6].Value.ToString();
                            toValidate.email = row.Cells[7].Value.ToString();
                            toValidate.account_type = (AccountType)row.Cells[8].Value;
                            MyConnection.updateQuery("INSERT INTO Accounts ([Username],[Password],[First Name],[Last Name],[Company],[Phone Number],[Email],[Account Type]) " +
                                "VALUES (" +
                                "'" + toValidate.username + "'," +
                                "'" + toValidate.password + "'," +
                                "'" + toValidate.first_name + "'," +
                                "'" + toValidate.last_name + "'," +
                                "'" + toValidate.company + "'," +
                                "'" + toValidate.phone + "'," +
                                "'" + toValidate.email + "'," +
                                "'" + toValidate.account_type.GetHashCode().ToString() + "')");
                            MyConnection.updateQuery("DELETE FROM PendingAccounts WHERE [ID]=" + row.Cells[0].Value.ToString() + "");
                            MessageBox.Show("This account has been successfully added to the database.");
                            CTRL.refreshDGV(pendingUsers, "Accept", "Accept", "Decline", "Decline", statement);
                            CTRL.refreshDGV(users, "Delete Account", "Delete", "Promote", "Promote", "Demote", "Demote", statement2);
                            break;
                        case "Decline":
                            MyConnection.updateQuery("DELETE FROM PendingAccounts WHERE [ID]=" + row.Cells[0].Value.ToString() + "");
                            MessageBox.Show("This user's register attempt has been declined.");
                            CTRL.refreshDGV(pendingUsers, "Accept", "Accept", "Decline", "Decline", statement);
                            break;
                        default: break;
                    }
                }
                catch (ArgumentOutOfRangeException) { }
                catch (NullReferenceException) { }
            };

            Label usersLabel = new Label();
            usersLabel.Text = "Users: ";
            usersLabel.Parent = tabPage;
            usersLabel.Location = new Point(20, 250);

            users.CellContentClick += (s, e) =>
            {
                DataGridView dgv = s as DataGridView;
                try
                {
                    if(dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
                    {
                        switch (dgv.Columns[e.ColumnIndex].HeaderText)
                        {
                            case "Delete Account":
                                if ((int)dgv.Rows[e.RowIndex].Cells[0].Value != user.id)
                                {
                                    MyConnection.updateQuery("DELETE FROM Accounts WHERE [ID]=" + dgv.Rows[e.RowIndex].Cells[0].Value.ToString() + "");
                                    MessageBox.Show("This account has been successfully deleted.");
                                    CTRL.refreshDGV(users, "Delete Account", "Delete", "Promote", "Promote", "Demote", "Demote", statement2);
                                }
                                else MessageBox.Show("You cannot delete your own account.");
                                break;
                            case "Promote":
                                if ((int)dgv.Rows[e.RowIndex].Cells[8].Value != 3)
                                {
                                    MyConnection.updateQuery("UPDATE Accounts SET [Account Type]=" + ((int)dgv.Rows[e.RowIndex].Cells[8].Value + 1).ToString() + " WHERE [ID]=" + dgv.Rows[e.RowIndex].Cells[0].Value.ToString() + "");
                                    MessageBox.Show("The user " + dgv.Rows[e.RowIndex].Cells[1].Value.ToString() + " has been successfully promoted to " + ((AccountType)((int)dgv.Rows[e.RowIndex].Cells[8].Value + 1)).ToString());
                                    CTRL.refreshDGV(users, "Delete Account", "Delete", "Promote", "Promote", "Demote", "Demote", statement2);
                                }
                                else MessageBox.Show("You cannot promote this user anymore.");
                                break;
                            case "Demote":
                                if ((int)dgv.Rows[e.RowIndex].Cells[8].Value != 0)
                                {
                                    if ((int)dgv.Rows[e.RowIndex].Cells[0].Value != user.id)
                                    {
                                        MyConnection.updateQuery("UPDATE Accounts SET [Account Type]=" + ((int)dgv.Rows[e.RowIndex].Cells[8].Value - 1).ToString() + " WHERE [ID]=" + dgv.Rows[e.RowIndex].Cells[0].Value.ToString() + "");
                                        MessageBox.Show("The user " + dgv.Rows[e.RowIndex].Cells[1].Value.ToString() + " has been successfully demoted to " + ((AccountType)((int)dgv.Rows[e.RowIndex].Cells[8].Value - 1)).ToString());
                                        CTRL.refreshDGV(users, "Delete Account", "Delete", "Promote", "Promote", "Demote", "Demote", statement2);
                                    }
                                    else MessageBox.Show("You cannot demote yourself.");
                                }
                                else MessageBox.Show("You cannot demote this user anymore.");
                                break;
                            default: break;
                        }
                    }
                }
                catch (ArgumentOutOfRangeException) { }
                catch (NullReferenceException) { }
            };

            tabControl.SelectedIndexChanged += (s, e) =>
            {
                CTRL.refreshDGV(pendingUsers, "Accept", "Accept", "Decline", "Decline", statement);
                CTRL.refreshDGV(users, "Delete Account", "Delete", "Promote", "Promote", "Demote", "Demote", statement2);
            };
            tabPage.Parent = tabControl;
        }
        public static void addMyBookingsTab(TabControl tabControl, User user)
        {
            Room room = new Room();
            TabPage tabPage = new TabPage();
            tabPage.Text = "Bookings";
            
            Label myBookingsLabel = new Label();
            myBookingsLabel.Location = new Point(20, 10);
            myBookingsLabel.Text = "My Bookings:";
            myBookingsLabel.Parent = tabPage;

            string statement = "SELECT [Booking ID],[Room ID],[Room Name],[Booked At],[Meeting Start],[Meeting End],Status FROM BookingHistory WHERE [Booked By]='" + user.username.ToString() + "' ORDER BY [Booking ID] ASC";
            DataGridView myBookings = new DataGridView();
            myBookings.Parent = tabPage;
            myBookings.ReadOnly = true;
            myBookings.Size = new Size(650, 200);
            myBookings.Location = new Point(20, 35);
            CTRL.refreshDGV(myBookings, "Options", "Cancel", statement);

            DataGridView allBookings = new DataGridView();
            Label usersBookingsLabel = new Label();

            string statement2;
            if (user.account_type==AccountType.CEO) statement2 = "SELECT * FROM BookingHistory ORDER BY [Booking ID] ASC";
            else statement2 = "SELECT * FROM BookingHistory WHERE [Account Type]=0 OR [Account Type]=1 ORDER BY [Booking ID] ASC";
            switch (user.account_type)
            {
                case AccountType.Manager:
                case AccountType.CEO:
                    usersBookingsLabel.Text = "Users Bookings:";
                    usersBookingsLabel.Location = new Point(20, 250);
                    usersBookingsLabel.Parent = tabPage;

                    allBookings.Parent = tabPage;
                    allBookings.ReadOnly = true;
                    allBookings.Size = new Size(650, 200);
                    allBookings.Location = new Point(20, 275);
                    CTRL.refreshDGV(allBookings, "Options", "Cancel", statement2);
                    allBookings.CellContentClick += (s, e) =>
                    {
                        DataGridView dgv = s as DataGridView;
                        if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
                        {
                            try
                            {
                                if (dgv.Rows[e.RowIndex].Cells[8].Value.ToString() == "Booked" || dgv.Rows[e.RowIndex].Cells[8].Value.ToString() == "Ongoing")
                                {
                                    User toCancel = new BasicUser();
                                    switch (dgv.Rows[e.RowIndex].Cells[4].Value.ToString())
                                    {
                                        case "0": toCancel = new BasicUser(); break;
                                        case "1": toCancel = new PremiumUser(); break;
                                        case "2": toCancel = new Manager(); break;
                                        case "3": toCancel = new CEO(); break;
                                        default: break;
                                    }
                                    object[] user_info = MyConnection.getResultSet("SELECT [Username],[Booked Rooms] FROM Accounts WHERE [Username]='" + dgv.Rows[e.RowIndex].Cells[3].Value.ToString() + "'");
                                    toCancel.username = (string)user_info[0];
                                    if (!(user_info[1] is DBNull))
                                        toCancel.booked_rooms = (string)user_info[1];
                                    else toCancel.booked_rooms = String.Empty;
                                    toCancel.booked_rooms = CTRL.removeSubstring(toCancel.booked_rooms, dgv.Rows[e.RowIndex].Cells[2].Value.ToString() + "/");
                                    MyConnection.updateQuery("UPDATE Accounts SET [Booked Rooms]='" + toCancel.booked_rooms + "' WHERE [Username]='" + toCancel.username + "'");
                                    MyConnection.updateQuery("UPDATE BookingHistory SET Status ='Cancelled' WHERE [Booking ID]=" + dgv.Rows[e.RowIndex].Cells[0].Value.ToString() + "");
                                    MyConnection.updateWithParameterNull("UPDATE Rooms SET Booked=FALSE,[Booked By]='',[Booked At]=@DBNull,[Meeting Start]=@DBNull,[Meeting End]=@DBNull WHERE [Room Name]='" + dgv.Rows[e.RowIndex].Cells[2].Value.ToString() + "'");
                                    MessageBox.Show("This booking has been successfully cancelled.");
                                    CTRL.refreshDGV(allBookings, "Select", "Cancel", statement2);
                                    CTRL.refreshDGV(myBookings, "Select", "Cancel", statement);
                                }
                                else MessageBox.Show("This booking has already finished or has been cancelled.");
                            }
                            catch (ArgumentOutOfRangeException) { }
                            catch (NullReferenceException) { }
                        }
                    };
                    break;
                default: break;
            }

            myBookings.CellContentClick += (s, e) =>
            {
                DataGridView dgv = s as DataGridView;
                try
                {
                    if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
                    {
                        if (dgv.Rows[e.RowIndex].Cells[6].Value.ToString() == "Booked" || dgv.Rows[e.RowIndex].Cells[6].Value.ToString() == "Ongoing")
                        {
                            user.booked_rooms = CTRL.removeSubstring(user.booked_rooms, dgv.Rows[e.RowIndex].Cells[2].Value.ToString() + "/");
                            MyConnection.updateQuery("UPDATE Accounts SET [Booked Rooms]='" + user.booked_rooms + "'");
                            MyConnection.updateQuery("UPDATE BookingHistory SET Status ='Cancelled' WHERE [Booking ID]=" + dgv.Rows[e.RowIndex].Cells[0].Value.ToString() + "");
                            MyConnection.updateWithParameterNull("UPDATE Rooms SET Booked=FALSE,[Booked By]='',[Booked At]=@DBNull,[Meeting Start]=@DBNull,[Meeting End]=@DBNull WHERE [Room Name]='" + dgv.Rows[e.RowIndex].Cells[2].Value.ToString() + "'");
                            MessageBox.Show("This booking has been successfully cancelled.");
                            CTRL.refreshDGV(myBookings, "Select", "Cancel", statement);
                            CTRL.refreshDGV(allBookings, "Select", "Cancel", statement2);
                        }
                        else MessageBox.Show("This booking has already finished or has been cancelled.");
                    }
                }
                catch (NullReferenceException) { }
                catch (ArgumentOutOfRangeException) { }
            };
            tabControl.SelectedIndexChanged += (s, e) =>
            {
                CTRL.refreshDGV(myBookings, "Select", "Cancel", statement);
                CTRL.refreshDGV(allBookings, "Select", "Cancel", statement2);
                MyConnection.updateWithParameterDateTime("UPDATE BookingHistory SET Status='Ongoing' WHERE [Meeting Start]<@DateTime AND [Meeting End]>@DateTime AND Status<>'Cancelled'", DateTime.Now);
                MyConnection.updateWithParameterDateTime("UPDATE BookingHistory SET Status='Finished' WHERE [Meeting End]<@DateTime AND Status<>'Cancelled'", DateTime.Now);
            };
            tabPage.Parent = tabControl;

        }
        public static void addRoomsTab(TabControl tabControl, User user)
        {
            Room room = new Room();
            TabPage tabPage = new TabPage();
            tabPage.Text = "Meeting Rooms";

            PictureBox roomPicture = new PictureBox();
            roomPicture.Size = new Size(350, 200);
            roomPicture.Location = new Point(20, 250);
            roomPicture.BorderStyle = BorderStyle.FixedSingle;
            roomPicture.SizeMode = PictureBoxSizeMode.StretchImage;
            roomPicture.BackColor = Color.White;
            roomPicture.Parent = tabPage;

            GroupBox infoGroup = new GroupBox();
            infoGroup.Parent = tabPage;
            infoGroup.Size = new Size(250, 200);
            infoGroup.Location = new Point(400, 20);
            infoGroup.Text = "Information";

            Label infoLabel = new Label();
            infoLabel.Location = new Point(20, 20);
            infoLabel.Parent = infoGroup;
            infoLabel.AutoSize = false;
            infoLabel.Dock = DockStyle.Fill;

            Label meetingStart = new Label();
            meetingStart.Parent = tabPage;
            meetingStart.Text = "Meeting Start:";
            meetingStart.Location = new Point(400, 250);

            DateTimePicker dtpStart = new DateTimePicker();
            dtpStart.Location = new Point(400, 275);
            dtpStart.Parent = tabPage;
            dtpStart.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpStart.Format = DateTimePickerFormat.Custom;

            Label meetingEnd = new Label();
            meetingEnd.Parent = tabPage;
            meetingEnd.Text = "Meeting End:";
            meetingEnd.Location = new Point(400, 300);

            DateTimePicker dtpEnd = new DateTimePicker();
            dtpEnd.Location = new Point(400, 325);
            dtpEnd.Parent = tabPage;
            dtpEnd.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpEnd.Format = DateTimePickerFormat.Custom;
            
            string statement = "SELECT [Room ID],[Room Name] FROM Rooms where [Room Type]=0";
            DataGridView roomsDGV = new DataGridView();
            roomsDGV.Parent = tabPage;
            roomsDGV.ReadOnly = true;
            roomsDGV.Size = new Size(350, 200);
            roomsDGV.Location = new Point(20, 20);
            switch (user.account_type)
            {
                case AccountType.BasicUser:
                    CTRL.refreshDGV(roomsDGV, "Select", "Book", statement); break;
                case AccountType.PremiumUser:
                    CTRL.refreshDGV(roomsDGV, "Select", "Book", statement + " OR [Room Type]=1"); break;
                case AccountType.Manager:
                case AccountType.CEO:
                    CTRL.refreshDGV(roomsDGV, "Select", "Book", statement + " OR [Room Type]=1 or [Room Type]=2"); break;
                default: break;
            }
            roomsDGV.CellContentClick += (s, e) =>
            {
                try
                {
                    object[] room_data = MyConnection.getResultSet("SELECT * FROM Rooms WHERE [Room ID]=" + roomsDGV.Rows[e.RowIndex].Cells[0].Value.ToString());
                    if (room_data != null)
                    {
                        room.id = (short)(int)room_data[0];
                        room.name = (string)room_data[1];
                        room.picture = CTRL.imageFromByteArray((byte[])(room_data[2]));
                        room.equipment = (string)room_data[3];
                        room.type = (RoomType)(int)room_data[4];
                        room.max_capacity = (short)(int)room_data[5];
                        room.floor = (short)(int)room_data[6];
                        room.booked = (bool)room_data[7];
                        if (room_data[8] is System.DBNull)
                            room.booked_by = String.Empty;
                        else room.booked_by = (string)room_data[8];
                        if (!(room_data[9] is DBNull)) room.booked_at = (DateTime)room_data[9];
                        else room.booked_at = new DateTime();
                        if (!(room_data[9] is DBNull)) room.meeting_start = (DateTime)room_data[10];
                        else room.meeting_start = new DateTime();
                        if (!(room_data[9] is DBNull)) room.meeting_end = (DateTime)room_data[11];
                        else room.meeting_end = new DateTime();

                        roomPicture.Image = room.picture;

                        infoLabel.Text = CTRL.getInfo(user, room);
                    }
                DataGridView dgv = s as DataGridView;
                if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex<dgv.Rows.Count)
                {
                    if (dtpEnd.Value > dtpStart.Value)
                    {
                        string invalid_message = String.Empty;
                        string invalid = String.Empty;
                        if (Check.timespan_valid(dtpStart.Value, dtpEnd.Value, user, ref invalid_message))
                        {
                            if (dtpStart.Value > DateTime.Now && dtpEnd.Value > DateTime.Now)
                            {
                                if (!room.booked)
                                {
                                    if (Check.rooms_booked_valid(user, ref invalid))
                                    {
                                        MyConnection.updateWithParameterDateTime("UPDATE Rooms SET Booked=TRUE, [Booked At]=@DateTime, [Booked By]='"+user.username+"' WHERE [Room ID]=" + room.id.ToString() + "",DateTime.Now);
                                        MyConnection.updateWithParameterDateTime("UPDATE Rooms SET  [Meeting Start]=@DateTime WHERE [Room ID]=" + room.id.ToString() + "", dtpStart.Value);
                                        MyConnection.updateWithParameterDateTime("UPDATE Rooms SET  [Meeting End]=@DateTime WHERE [Room ID]=" + room.id.ToString() + "", dtpEnd.Value);
                                        MyConnection.updateQuery("UPDATE Accounts SET [Booked Rooms]='" + user.booked_rooms + room.name + "/" + "' WHERE [ID]=" + user.id + "");
                                        MyConnection.updateQuery("INSERT INTO BookingHistory ([Room ID],[Room Name],[Booked By],[Account Type],[Booked At],[Meeting Start],[Meeting End],[Status]) " +
                                            "VALUES (" +
                                            "'" + room.id.ToString() + "'," +
                                            "'" + room.name.ToString() + "'," +
                                            "'" + user.username.ToString() + "'," +
                                            "'" + user.account_type.GetHashCode().ToString() + "'," +
                                            "'" + DateTime.Now.ToString() + "'," +
                                            "'" + dtpStart.Value.ToString() + "'," +
                                            "'" + dtpEnd.Value.ToString() + "'," +
                                            "'Booked')");
                                        object[] o = MyConnection.getResultSet("SELECT [Booked Rooms] FROM Accounts WHERE [ID]=" + user.id.ToString() + "");
                                        user.booked_rooms = (string)o[0];
                                        MessageBox.Show("This room has been successfully booked.");
                                    }
                                    else MessageBox.Show(invalid);
                                }
                                else MessageBox.Show("This room is already booked by someone.");
                            }
                            else MessageBox.Show("The dates you selected are invalid. Please select a date in the future for your booking.");
                        }
                        else MessageBox.Show(invalid_message);
                    }
                    else MessageBox.Show("The ending date should be greater that the starting date.");
                    }
                }
                catch (ArgumentOutOfRangeException) { }
                catch (NullReferenceException) { }
            };
            roomsDGV.CellClick += (s, e) =>
             {
                 try
                 {
                     object[] room_data = MyConnection.getResultSet("SELECT * FROM Rooms WHERE [Room ID]=" + roomsDGV.Rows[e.RowIndex].Cells[0].Value.ToString());
                     if (room_data != null)
                     {
                         room.id = (short)(int)room_data[0];
                         room.name = (string)room_data[1];
                         room.picture = CTRL.imageFromByteArray((byte[])(room_data[2]));
                         room.equipment = (string)room_data[3];
                         room.type = (RoomType)(int)room_data[4];
                         room.max_capacity = (short)(int)room_data[5];
                         room.floor = (short)(int)room_data[6];
                         room.booked = (bool)room_data[7];
                         if (room_data[8] is System.DBNull)
                             room.booked_by = String.Empty;
                         else room.booked_by = (string)room_data[8];
                         if (!(room_data[9] is DBNull)) room.booked_at = (DateTime)room_data[9];
                         else room.booked_at = new DateTime();
                         if (!(room_data[9] is DBNull)) room.meeting_start = (DateTime)room_data[10];
                         else room.meeting_start = new DateTime();
                         if (!(room_data[9] is DBNull)) room.meeting_end = (DateTime)room_data[11];
                         else room.meeting_end = new DateTime();

                         roomPicture.Image = room.picture;

                         infoLabel.Text = CTRL.getInfo(user, room);
                     }
                 }
                 catch (ArgumentOutOfRangeException) { }
                 catch (NullReferenceException) { }
             };

            tabPage.Parent = tabControl;
            tabControl.SelectedIndexChanged += (s, e) =>
            { 
                switch (user.account_type)
                {
                    case AccountType.BasicUser:
                        CTRL.refreshDGV(roomsDGV, "Book", "Book", statement); break;
                    case AccountType.PremiumUser:
                        CTRL.refreshDGV(roomsDGV, "Book", "Book", statement + " or [Room Type]=1"); break;
                    case AccountType.Manager:
                        CTRL.refreshDGV(roomsDGV, "Book", "Book", statement + " or [Room Type]=1 or [Room Type]=2"); break;
                    case AccountType.CEO:
                        CTRL.refreshDGV(roomsDGV, "Book", "Book", statement + " or [Room Type]=1 or [Room Type]=2"); break;
                    default: break;
                }
            };

            Label sortLabel = new Label();
            sortLabel.Text = "Sort rooms by: ";
            sortLabel.Location = new Point(400, 400);
            sortLabel.Parent = tabPage;

            ComboBox sortComboBox = new ComboBox();
            sortComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            sortComboBox.Location = new Point(400 + sortLabel.Size.Width, 400);
            sortComboBox.Parent = tabPage;
            string[] items = { "Room ID", "Room Name", "Room Type", "Maximum Capacity", "Floor", "Booked" };
            sortComboBox.Items.AddRange(items);

            Label sortOrderLabel = new Label();
            sortOrderLabel.Text = "Sorting order: ";
            sortOrderLabel.Location = new Point(400, 425);
            sortOrderLabel.Parent = tabPage;

            ComboBox sortOrderComboBox = new ComboBox();
            sortOrderComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            sortOrderComboBox.Location = new Point(400 + sortOrderLabel.Size.Width, 425);
            sortOrderComboBox.Parent = tabPage;
            sortOrderComboBox.Items.Add("ASC");
            sortOrderComboBox.Items.Add("DESC");
            sortOrderComboBox.SelectedIndex = 0;

            switch (user.account_type)
            {
                case AccountType.PremiumUser:
                case AccountType.Manager:
                case AccountType.CEO: sortComboBox.Items.Add("Available at"); break;
                default:break;
            }
            sortComboBox.SelectedValueChanged += (s, e) =>
            {
                if (sortComboBox.Text != "Available at")
                    switch (user.account_type)
                    {
                        case AccountType.BasicUser:
                            CTRL.refreshDGV(roomsDGV, "Select", "Book", statement+" ORDER BY ["+sortComboBox.Text+"] "+sortOrderComboBox.Text+""); break;
                        case AccountType.PremiumUser:
                            CTRL.refreshDGV(roomsDGV, "Select", "Book", statement + " OR [Room Type]=1" + " ORDER BY [" + sortComboBox.Text + "] " + sortOrderComboBox.Text + ""); break;
                        case AccountType.Manager:
                        case AccountType.CEO:
                            CTRL.refreshDGV(roomsDGV, "Select", "Book", statement + " OR [Room Type]=1 or [Room Type]=2" + " ORDER BY [" + sortComboBox.Text + "] " + sortOrderComboBox.Text + ""); break;
                        default: break;
                    }
                else switch(user.account_type)
                    {
                        case AccountType.PremiumUser:
                            CTRL.refreshDGV(roomsDGV, "Select", "Book", statement + " OR [Room Type]=1" + " ORDER BY [Meeting End] " + sortOrderComboBox.Text + ""); break;
                        case AccountType.Manager:
                        case AccountType.CEO:
                            CTRL.refreshDGV(roomsDGV, "Select", "Book", statement + " OR [Room Type]=1 or [Room Type]=2" + " ORDER BY [Meeting End] " + sortOrderComboBox.Text + ""); break;
                        default: break;
                    }
            };
        }
    }
}
