using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using VismaBooking.Connection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using VismaBooking.Users.General;
using VismaBooking.Rooms;

namespace VismaBooking.GUI
{
    public static class CTRL
    {
        //populates or refreshes a DataGridView with data from the Database
        public static void refreshDGV(DataGridView dgv, string statement)
        {
            dgv.Columns.Clear();
            DataTable dt = MyConnection.getDataTable(statement);
            foreach (DataColumn dc in dt.Columns)
            {
                DataGridViewTextBoxColumn dgtc = new DataGridViewTextBoxColumn();
                dgtc.HeaderText = dc.ColumnName;
                dgv.Columns.Add(dgtc);
            }
            foreach (DataRow dr in dt.Rows)
                dgv.Rows.Add(dr.ItemArray);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        //populates or refreshes a datagridview that contains a buttoncolumn aswell
        public static void refreshDGV(DataGridView dgv, string headerText, string buttonText, string statement)
        {
            refreshDGV(dgv, statement);
            DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn();
            btnCol.HeaderText = headerText;
            btnCol.Text = buttonText;
            btnCol.UseColumnTextForButtonValue = true;
            dgv.Columns.Add(btnCol);
        }
        //populates or refreshes a datagridview that contains two buttoncolumns
        public static void refreshDGV(DataGridView dgv, string headerText1, string buttonText1,string headerText2, string buttonText2, string statement)
        {
            refreshDGV(dgv,headerText1,buttonText1, statement);
            DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn();
            btnCol.HeaderText = headerText2;
            btnCol.Text = buttonText2;
            btnCol.UseColumnTextForButtonValue = true;
            dgv.Columns.Add(btnCol);
        }
        public static void refreshDGV(DataGridView dgv, string headerText1, string buttonText1, string headerText2, string buttonText2, string headerText3, string buttonText3, string statement)
        {
            refreshDGV(dgv, headerText1, buttonText1, headerText2, buttonText2, statement);
            DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn();
            btnCol.HeaderText = headerText3;
            btnCol.Text = buttonText3;
            btnCol.UseColumnTextForButtonValue = true;
            dgv.Columns.Add(btnCol);
        }
        public static Image imageFromByteArray(byte[] bytearray)
        {
            ImageConverter imgCon = new ImageConverter();
            Image img= (Image)imgCon.ConvertFrom(bytearray);
            return img;
        }
        public static byte[] imageToByteArray(Image image)
        {
            ImageConverter imageConverter = new ImageConverter();
            byte[] bytearray= (byte[])imageConverter.ConvertTo(image, typeof(byte[]));
            return bytearray;
        }
        public static string getInfo(User user, Room room)
        {
            string info = "Room ID: " + room.id.ToString() + Environment.NewLine +
                        "Room Name: " + room.name.ToString() + Environment.NewLine +
                        "Equipment: " + room.equipment.ToString() + Environment.NewLine +
                        "Room Type: " + room.type.ToString() + Environment.NewLine +
                        "Maximum capacity: " + room.max_capacity.ToString() + Environment.NewLine +
                        "Floor: " + room.floor.ToString() + Environment.NewLine;
            if (room.booked) info += "Booked: YES" + Environment.NewLine;
            else info += "Booked: NO" + Environment.NewLine;
            switch (user.account_type)
            {
                case AccountType.BasicUser: break;
                case AccountType.PremiumUser:
                    if (room.meeting_end != DateTime.MinValue)
                    { info += "Available at: " + room.meeting_end.ToString(); break; }
                    else { info += "Available at: Available"; break; }
                case AccountType.Manager:
                case AccountType.CEO:
                    info += "Booked by: " + room.booked_by.ToString() + Environment.NewLine;
                    info += "Booked at: ";
                    if (room.booked_at != DateTime.MinValue)
                        info += room.booked_at.ToString();
                    info += Environment.NewLine + "Meeting start: ";
                    if (room.meeting_start != DateTime.MinValue)
                        info += room.meeting_start.ToString();
                    info += Environment.NewLine + "Available at: ";
                    if (room.meeting_end != DateTime.MinValue)
                        info += room.meeting_end.ToString();
                    else info += "Available"; break;
                default: break;
            }
            return info;
        }
        public static string removeSubstring(string text, string substring)
        {
            int i = text.IndexOf(substring);
            if (i < 0)
                return text;
            else return text.Remove(i, substring.Length);
        }
    }
}
