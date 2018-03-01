using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adultus.Helpers;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using Adultus.Models;

namespace LogOut
{
    class Program
    {
        static void Main(string[] args)
        {
            //Schedules schedules = new Schedules();
            //DbContext();
            //List<String> usernames =  schedules.GetContext(GetAllActiveUser());
            //foreach (string username in usernames)
            //{
            //    SetOnlineStatusToOffline(username);
            //}
        }

        private static readonly string ConnectionString = "Data Source=SQLHA;Initial Catalog=Adultus;Integrated Security=True";

        private static SqlConnection Connection;

        public static DataSet DbContext()
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            DataTable tables = Connection.GetSchema("Tables");
            DataSet dbSet = tables.DataSet;
            return dbSet;
        }

        public static List<Users> GetAllActiveUser()
        {
            DataSet userDataset = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Users where OnlineStatus = 1", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.Fill(userDataset);
            }

            List<Users> users = new List<Users>();
            Users u = new Users();
            foreach (DataRow dr in userDataset.Tables["Table"].Rows)
            {

                u.Id = dr["Id"].ToString();
                u.UserName = dr["UserName"].ToString();
                u.Email = dr["Email"].ToString();
                u.Password = dr["Password"].ToString();
                if (dr["ProfileId"].ToString() != "")
                {
                    u.ProfileId = dr["ProfileId"].ToString();
                    u.Gender = dr["Gender"].ToString();
                    u.GenderPreference = dr["GenderPreference"].ToString();
                    u.ProfilePic = dr["ProfilePic"].ToString();
                }
                if (dr["AccountBalance"] != System.DBNull.Value)
                {
                    u.AccountBalance = dr.Field<int>("AccountBalance");
                }
                else
                {
                    u.AccountBalance = 0;
                }
                if (dr["DateOfBirth"] != System.DBNull.Value)
                {
                    u.DateOfBirth = DateTime.Parse(dr["DateOfBirth"].ToString());
                }
                if (dr["OnlineStatus"] != System.DBNull.Value)
                {
                    u.OnlineStatus = bool.Parse(dr["OnlineStatus"].ToString());
                }
                if (dr["PostCode"] != System.DBNull.Value)
                {
                    u.City = dr["City"].ToString();
                    u.Country = dr["Country"].ToString();
                    u.PostCode = dr["PostCode"].ToString();
                }

                users.Add(u);
            }

            return users;
        }

        public static void SetOnlineStatusToOffline(string userName)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE Users SET OnlineStatus = 0 WHERE UserName = @UserName", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@UserName"].Value = userName;
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.UpdateCommand.ExecuteNonQuery();
            }
        }
    }
}
