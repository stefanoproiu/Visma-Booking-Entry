using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;

namespace VismaBooking.Connection
{
    static class MyConnection
    {
        private static OleDbConnection getConnection()
        {
            OleDbConnection con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=visma.accdb; Persist Security Info = False; ";
            return con;
        }
        public static object[] getResultSet (string statement)
        {
            object[] resultArray=null;
            OleDbConnection con = getConnection();
            OleDbCommand command = new OleDbCommand(statement, con);
            try
            {
                con.Open();
                OleDbDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    resultArray = new object[reader.FieldCount];
                    reader.GetValues(resultArray);
                }
                reader.Close();
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.ToString(),"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
            return resultArray;
        }
        public static void updateQuery(string statement)
        {
            OleDbConnection con = getConnection();
            OleDbCommand command = new OleDbCommand(statement, con);
            try
            {
                con.Open();
                command.ExecuteNonQuery();
            }
            catch(OleDbException ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }
        public static DataTable getDataTable(string statement)
        {   
            OleDbConnection con = getConnection();
            OleDbCommand command = new OleDbCommand(statement, con);
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(command);
            DataTable dataTable = new DataTable();
            try
            {
                con.Open();
                dataAdapter.Fill(dataTable);
            }
            catch(OleDbException ex)
            {
                dataTable = null;
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
            return dataTable;
        }
        public static short countApparitions(string statement)
        {
            OleDbConnection con = getConnection();
            OleDbCommand command = new OleDbCommand(statement, con);
            short count = 0;
            try
            {
                con.Open();
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read()) count++;
            }
            catch(OleDbException ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
            return count;
        }
        public static void updateWithParameter(string statement, byte[] bytearray)
        {
            OleDbConnection con = getConnection();
            OleDbCommand command = new OleDbCommand(statement, con);
            try
            {
                con.Open();
                command.Parameters.AddWithValue("@picture", bytearray);
                command.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }
        public static void updateWithParameterNull(string statement)
        {
            OleDbConnection con = getConnection();
            OleDbCommand command = new OleDbCommand(statement, con);
            try
            {
                con.Open();
                command.Parameters.AddWithValue("@DBNull", DBNull.Value);
                command.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }
        public static void updateWithParameterDateTime(string statement,DateTime dateTime)
        {
            OleDbConnection con = getConnection();
            OleDbCommand command = new OleDbCommand(statement, con);
            try
            {
                con.Open();
                command.Parameters.AddWithValue("@DateTime", dateTime.ToString());
                command.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }
    }
}
