using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Adultus.Models;
using Microsoft.Ajax.Utilities;

namespace Adultus.Helpers
{
    public class SqlHelper
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        private static SqlConnection Connection;

        public static DataSet DbContext()
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            DataTable tables = Connection.GetSchema("Tables");
            DataSet dbSet = tables.DataSet;
            return dbSet;
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static Users LoginQuery(string userName, string password, bool encrypt)
        {
            if (encrypt)
            {
                string queryString = "SELECT * FROM Users WHERE UserName ='" + userName + "' AND Password='" + Base64Decode(password) + "'";
                SqlCommand command = new SqlCommand(queryString, Connection);
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                da.Fill(ds);
                Users u = new Users();
                foreach (DataRow dr in ds.Tables["Table"].Rows)
                {
                    u.Id = dr["Id"].ToString();
                    u.ProfileId = dr["ProfileId"].ToString();
                    u.UserName = dr["UserName"].ToString();

                    return u;
                }

                return u;
            }
            else
            {
                string queryString = "SELECT * FROM Users WHERE UserName ='" + userName + "' AND Password='" + password + "'";
                SqlCommand command = new SqlCommand(queryString, Connection);
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                da.Fill(ds);
                Users u = new Users();
                foreach (DataRow dr in ds.Tables["Table"].Rows)
                {
                    u.Id = dr["Id"].ToString();
                    u.ProfileId = dr["ProfileId"].ToString();
                    u.UserName = dr["UserName"].ToString();

                    return u;
                }

                return u;
            }
        }

        public static void AddProfilePic(string fileName, string userId)
        {
            string queryString = "UPDATE Users SET ProfilePic = '" + fileName + "' WHERE Id = '" + userId + "'";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.InsertCommand = command;
            da.InsertCommand.ExecuteNonQuery();
        }

        public static void AddUser(string newGuid, string userName, string email, DateTime DateOfBirth)
        {
            string queryString = "INSERT INTO Users Values ('" + newGuid + "','" + email + "'," + 0 + ",'" + userName + "',null,null,null,null,null,null,null,null," + DateOfBirth.ToShortDateString() + "," + DateTime.Now.ToShortDateString() + ",null,null,null,null,null,null,null,null,null,null,null)";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.InsertCommand = command;
            da.InsertCommand.ExecuteNonQuery();
        }

        public static void EmailConfirmation(string userId)
        {
            string queryString = "UPDATE Users SET EmailConfirmed = 1 WHERE Id = '" + userId + "'";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.InsertCommand = command;
            da.InsertCommand.ExecuteNonQuery();
        }

        public static void ProfileSetUp(string userId, string profileId, string gender, string genderPreference)
        {
            string queryString = "UPDATE Users SET ProfileId = '" + profileId + "', Gender = '" + gender + "', GenderPreference = '" + genderPreference + "' WHERE Id = '" + userId + "'";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.InsertCommand = command;
            da.InsertCommand.ExecuteNonQuery();
        }

        public static void SetPassword(string password, string userId, bool setHash)
        {
            string encrypted = Base64Encode(password);
            string queryString = "UPDATE Users SET Password = '" + password + "' WHERE Id = '" + userId + "'";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.InsertCommand = command;
            da.InsertCommand.ExecuteNonQuery();

            if (setHash)
            {
                string hashQuery = "UPDATE Users SET PasswordHash = '" + encrypted + "' WHERE Id = '" + userId + "'";
                SqlCommand hashCommand = new SqlCommand(hashQuery, Connection);
                SqlDataAdapter hashDa = new SqlDataAdapter(hashCommand);
                hashDa.InsertCommand = hashCommand;
                hashDa.InsertCommand.ExecuteNonQuery();
            }
        }

        public static void AddSession(string userId)
        {
            string queryString = "INSERT INTO LoginSessions Values ('" + Guid.NewGuid() + "','" + userId + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.InsertCommand = command;
            da.InsertCommand.ExecuteNonQuery();
        }

        public static void SetOnlineStatus(string userId)
        {
            string queryString = "UPDATE Users SET OnlineStatus = 1 WHERE Id = '" + userId + "'";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.InsertCommand = command;
            da.InsertCommand.ExecuteNonQuery();
        }

        public static void CreateRole(string id, string name)
        {
            string queryString = "INSERT INTO Roles Values ('" + id + "','" + name + "')";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.InsertCommand = command;
            da.InsertCommand.ExecuteNonQuery();
        }

        public static void EditRole(string id, string name)
        {
            string queryString = "UPDATE Roles SET name = '" + name + "' WHERE Id = '" + id + "'";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.UpdateCommand = command;
            da.InsertCommand.ExecuteNonQuery();
        }

        public static void CreateProfile(string id, string name)
        {
            string queryString = "INSERT INTO Profiles Values ('" + id + "','" + name + "')";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.InsertCommand = command;
            da.InsertCommand.ExecuteNonQuery();
        }

        public static void EditProfile(string id, string name)
        {
            string queryString = "UPDATE Profiles SET name = '" + name + "' WHERE Id = '" + id + "'";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.UpdateCommand = command;
            da.InsertCommand.ExecuteNonQuery();
        }

        public static void CreateProfileRole(string id, string profileId, string roleId)
        {
            string queryString = "INSERT INTO ProfileRoles Values ('" + id + "','" + profileId + "','" + roleId + "')";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.InsertCommand = command;
            da.InsertCommand.ExecuteNonQuery();
        }

        public static void EditProfileRoles(string id, string profileId, string roleId)
        {
            string queryString = "UPDATE Profiles SET ProfileId = '" + profileId + "' AND RoleId ='" + roleId + "' WHERE Id = '" + id + "'";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.UpdateCommand = command;
            da.InsertCommand.ExecuteNonQuery();
        }

        public static Users GetUser(string userId)
        {
            string queryString = "SELECT * FROM Users where Id = '" + userId + "'";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);

            Users u = new Users();
            foreach (DataRow dr in ds.Tables["Table"].Rows)
            {
                
                u.Id = dr["Id"].ToString();
                u.ProfileId = dr["ProfileId"].ToString();
                u.UserName = dr["UserName"].ToString();
                u.Email = dr["Email"].ToString();
                u.Password = dr["Password"].ToString();

                return u;
            }

            return u;
        }

        public static List<Users> GetAllUsers()
        {
            string queryString = "SELECT * FROM Users";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);

            List<Users> users = new List<Users>();
            foreach (DataRow dr in ds.Tables["Table"].Rows)
            {
                Users u = new Users();
                u.Id = dr["Id"].ToString();
                u.UserName = dr["UserName"].ToString();

                users.Add(u);
            }

            return users;
        }

        public static Roles GetRole(string roleId)
        {
            string queryString = "SELECT * FROM Roles where Id = '" + roleId + "'";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);

            Roles r = new Roles();
            foreach (DataRow dr in ds.Tables["Table"].Rows)
            {
                r.Id = dr["Id"].ToString();
                r.name = dr["Name"].ToString();

                return r;
            }

            return r;
        }

        public static Profiles GetProfile(string profileId)
        {
            string queryString = "SELECT * FROM Profiles where Id = '" + profileId + "'";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);

            Profiles p = new Profiles();
            foreach (DataRow dr in ds.Tables["Table"].Rows)
            {
                p.Id = dr["Id"].ToString();
                p.name = dr["Name"].ToString();

                return p;
            }

            return p;
        }

        public static List<Profiles> GetAllProfiles()
        {
            string queryString = "SELECT * FROM Profiles";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);

            List<Profiles> profiles = new List<Profiles>();
            foreach (DataRow dr in ds.Tables["Table"].Rows)
            {
                Profiles p = new Profiles();
                p.Id = dr["Id"].ToString();
                p.name = dr["Name"].ToString();

                profiles.Add(p);
            }

            return profiles;
        }

        public static List<Roles> GetAllRoles()
        {
            string queryString = "SELECT * FROM Roles";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);

            List<Roles> roles = new List<Roles>();
            foreach (DataRow dr in ds.Tables["Table"].Rows)
            {
                Roles r = new Roles();
                r.Id = dr["Id"].ToString();
                r.name = dr["Name"].ToString();

                roles.Add(r);
            }

            return roles;
        }

        public static string GetUserProfile(string userId)
        {
            string queryString = "SELECT * FROM Profiles where UserId = '" + userId + "'";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds.Tables[0].Columns["Id"].ToString();
        }

        public static List<ProfileRoles> GetProfileRoles(string profileId)
        {
            string queryString = "SELECT * FROM ProfileRoles where ProfileId = '" + profileId + "'";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);

            List<ProfileRoles> profiles = new List<ProfileRoles>();
            foreach (DataRow dr in ds.Tables["Table"].Rows)
            {
                ProfileRoles pr = new ProfileRoles();
                pr.Id = dr["Id"].ToString();
                pr.RoleId = dr["RoleId"].ToString();
                pr.ProfileId = dr["ProfileId"].ToString();
                profiles.Add(pr);
            }

            return profiles;
        }

        public static List<ProfileRoles> GetAllProfileRoles()
        {
            string queryString = "SELECT * FROM ProfileRoles";
            SqlCommand command = new SqlCommand(queryString, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);

            List<ProfileRoles> profiles = new List<ProfileRoles>();
            foreach (DataRow dr in ds.Tables["Table"].Rows)
            {
                ProfileRoles pr = new ProfileRoles();
                pr.Id = dr["Id"].ToString();
                pr.RoleId = dr["RoleId"].ToString();
                pr.ProfileId = dr["ProfileId"].ToString();
                Profiles p = new Profiles();
                p = SqlHelper.GetProfile(pr.ProfileId);
                pr.ProfileName = p.name;
                Roles r = new Roles();
                r = SqlHelper.GetRole(pr.RoleId);
                pr.RoleName = r.name;
                profiles.Add(pr);
            }

            return profiles;
        }

        public static void CloseConnection()
        {
            Connection.Close();
        }
    }
}