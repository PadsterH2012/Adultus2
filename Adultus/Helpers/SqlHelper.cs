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
                DataSet userDataset = new DataSet();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    Guid loginSessionId = Guid.NewGuid();
                    string e = Base64Decode(password);
                    SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE UserName = @userName AND Password= @Password", connection);
                    SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                    myDataAdapter.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar);
                    myDataAdapter.SelectCommand.Parameters["@UserName"].Value = userName;
                    myDataAdapter.SelectCommand.Parameters.Add("@Password", SqlDbType.VarChar);
                    myDataAdapter.SelectCommand.Parameters["@Password"].Value = e;
                    myDataAdapter.UpdateCommand = command;
                    myDataAdapter.Fill(userDataset);
                }
                //string queryString = "SELECT * FROM Users WHERE UserName ='" + userName + "' AND Password='" + Base64Decode(password) + "'";
                //SqlCommand command = new SqlCommand(queryString, Connection);
                //SqlDataAdapter da = new SqlDataAdapter(command);
                //DataSet ds = new DataSet();
                //da.Fill(ds);
                Users u = new Users();
                foreach (DataRow dr in userDataset.Tables["Table"].Rows)
                {
                    u.Id = dr["Id"].ToString();
                    u.ProfileId = dr["ProfileId"].ToString();
                    u.UserName = dr["UserName"].ToString();
                    if (dr["EmailConfirmed"] != System.DBNull.Value)
                    {
                        u.EmailConfirmed = bool.Parse(dr["EmailConfirmed"].ToString());
                    }

                    return u;
                }

                return u;
            }
            else
            {
                DataSet userDataset = new DataSet();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    Guid loginSessionId = Guid.NewGuid();
                    SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE UserName = @userName AND Password= @Password", connection);
                    SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                    myDataAdapter.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar);
                    myDataAdapter.SelectCommand.Parameters["@UserName"].Value = userName;
                    myDataAdapter.SelectCommand.Parameters.Add("@Password", SqlDbType.VarChar);
                    myDataAdapter.SelectCommand.Parameters["@Password"].Value = password;
                    myDataAdapter.UpdateCommand = command;
                    myDataAdapter.Fill(userDataset);
                }
                //string queryString = "SELECT * FROM Users WHERE UserName ='" + userName + "' AND Password='" + password + "'";
                //SqlCommand command = new SqlCommand(queryString, Connection);
                //SqlDataAdapter da = new SqlDataAdapter(command);
                //DataSet ds = new DataSet();
                //da.Fill(ds);
                Users u = new Users();
                foreach (DataRow dr in userDataset.Tables["Table"].Rows)
                {
                    u.Id = dr["Id"].ToString();
                    u.ProfileId = dr["ProfileId"].ToString();
                    u.UserName = dr["UserName"].ToString();
                    if (dr["EmailConfirmed"] != System.DBNull.Value)
                    {
                        u.EmailConfirmed = bool.Parse(dr["EmailConfirmed"].ToString());
                    }

                    return u;
                }

                return u;
            }
        }

        public static void AddProfilePic(string fileName, string userId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command =
                    new SqlCommand(
                        "UPDATE Users SET ProfilePic = @FileName WHERE Id = @Id",
                        connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@FileName", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@FileName"].Value = fileName;
                myDataAdapter.SelectCommand.Parameters.Add("@Id", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Id"].Value = userId;
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.InsertCommand.ExecuteNonQuery();
            }
            //string queryString = "UPDATE Users SET ProfilePic = '" + fileName + "' WHERE Id = '" + userId + "'";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //da.InsertCommand = command;
            //da.InsertCommand.ExecuteNonQuery();
        }

        public static void AddUser(string newGuid, string userName, string email, DateTime DateOfBirth)
        {
            string dob = "";
            dob = DateOfBirth.ToShortDateString();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command =
                    new SqlCommand(
                        "INSERT INTO Users Values (@newGuid,@Email," + 0 + ",@userName,null,null,null,null,null,null,null,null,@dob," + DateTime.Now.ToShortDateString() + ",null,null,null,null,null,null,null,null,null,null,null)", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@newGuid", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@newGuid"].Value = newGuid;
                myDataAdapter.SelectCommand.Parameters.Add("@Email", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Email"].Value = email;
                myDataAdapter.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@UserName"].Value = userName;
                myDataAdapter.SelectCommand.Parameters.Add("@DateOfBirth", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@DateOfBirth"].Value = DateOfBirth;
                myDataAdapter.InsertCommand = command;
                myDataAdapter.InsertCommand.ExecuteNonQuery();
            }
            //string queryString = "INSERT INTO Users Values ('" + newGuid + "','" + email + "'," + 0 + ",'" + userName + "',null,null,null,null,null,null,null,null," + DateOfBirth.ToShortDateString() + "," + DateTime.Now.ToShortDateString() + ",null,null,null,null,null,null,null,null,null,null,null)";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //da.InsertCommand = command;
            //da.InsertCommand.ExecuteNonQuery();
        }

        public static void EmailConfirmation(string userId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command =
                    new SqlCommand(
                        "UPDATE Users SET EmailConfirmed = 1 WHERE Id = @Id",
                        connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@Id", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Id"].Value = userId;
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.InsertCommand.ExecuteNonQuery();
            }
            //string queryString = "UPDATE Users SET EmailConfirmed = 1 WHERE Id = '" + userId + "'";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //da.InsertCommand = command;
            //da.InsertCommand.ExecuteNonQuery();
        }

        public static void ProfileSetUp(string userId, string profileId, string gender, string genderPreference)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command =
                    new SqlCommand(
                        "UPDATE Users SET ProfileId = @ProfileId, Gender = @Gender, GenderPreference = @GenderPreference WHERE Id = @Id",
                        connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@Gender", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Gender"].Value = gender;
                myDataAdapter.SelectCommand.Parameters.Add("@GenderPreference", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@GenderPreference"].Value = genderPreference;
                myDataAdapter.SelectCommand.Parameters.Add("@Id", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Id"].Value = userId;
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.InsertCommand.ExecuteNonQuery();
            }
            //string queryString = "UPDATE Users SET ProfileId = '" + profileId + "', Gender = '" + gender + "', GenderPreference = '" + genderPreference + "' WHERE Id = '" + userId + "'";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //da.InsertCommand = command;
            //da.InsertCommand.ExecuteNonQuery();
        }

        public static void SetPassword(string password, string userId, bool setHash)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string encrypted = Base64Encode(password);
                SqlCommand command = new SqlCommand("UPDATE Users SET Password = @Password WHERE Id = @Id", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@Password", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Password"].Value = userId;
                myDataAdapter.SelectCommand.Parameters.Add("@Id", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Id"].Value = userId;
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.InsertCommand.ExecuteNonQuery();
                if (setHash)
                {
                    SqlCommand hashCommand = new SqlCommand("UPDATE Users SET PasswordHash = @PasswordHash WHERE Id = @Id", connection);
                    SqlDataAdapter hashDataAdapter = new SqlDataAdapter(command);
                    hashDataAdapter.SelectCommand.Parameters.Add("@PasswordHash", SqlDbType.VarChar);
                    hashDataAdapter.SelectCommand.Parameters["@PasswordHash"].Value = userId;
                    hashDataAdapter.SelectCommand.Parameters.Add("@Id", SqlDbType.VarChar);
                    hashDataAdapter.SelectCommand.Parameters["@Id"].Value = userId;
                    hashDataAdapter.UpdateCommand = hashCommand;
                    hashDataAdapter.InsertCommand.ExecuteNonQuery();
                }
            }
            //string encrypted = Base64Encode(password);
            //string queryString = "UPDATE Users SET Password = '" + password + "' WHERE Id = '" + userId + "'";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //da.InsertCommand = command;
            //da.InsertCommand.ExecuteNonQuery();

            //if (setHash)
            //{
            //    string hashQuery = "UPDATE Users SET PasswordHash = '" + encrypted + "' WHERE Id = '" + userId + "'";
            //    SqlCommand hashCommand = new SqlCommand(hashQuery, Connection);
            //    SqlDataAdapter hashDa = new SqlDataAdapter(hashCommand);
            //    hashDa.InsertCommand = hashCommand;
            //    hashDa.InsertCommand.ExecuteNonQuery();
            //}
        }

        public static void AddSession(string userId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                Guid loginSessionId = Guid.NewGuid();
                string join = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SqlCommand command = new SqlCommand("INSERT INTO LoginSessions Values (@loginId,@Id,@join)", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@Id", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Id"].Value = userId;
                myDataAdapter.SelectCommand.Parameters.Add("@loginId", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@loginId"].Value = loginSessionId.ToString();
                myDataAdapter.SelectCommand.Parameters.Add("@join", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@join"].Value = join;
                myDataAdapter.InsertCommand = command;
                myDataAdapter.InsertCommand.ExecuteNonQuery();
            }
            //string queryString = "INSERT INTO LoginSessions Values ('" + Guid.NewGuid() + "','" + userId + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //da.InsertCommand = command;
            //da.InsertCommand.ExecuteNonQuery();
        }

        public static void SetOnlineStatus(string userId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE Users SET OnlineStatus = 1 WHERE Id = @Id", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@Id", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Id"].Value = userId;
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.UpdateCommand.ExecuteNonQuery();
            }
            //string queryString = "UPDATE Users SET OnlineStatus = 1 WHERE Id = '" + userId + "'";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //da.InsertCommand = command;
            //da.InsertCommand.ExecuteNonQuery();
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

        public static void CreateRole(string id, string name)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Roles Values ( @Id , @Name)", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@Name", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Name"].Value = name;
                myDataAdapter.SelectCommand.Parameters.Add("@Id", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Id"].Value = id;
                myDataAdapter.InsertCommand = command;
                myDataAdapter.InsertCommand.ExecuteNonQuery();
            }
            //string queryString = "INSERT INTO Roles Values ('" + id + "','" + name + "')";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //da.InsertCommand = command;
            //da.InsertCommand.ExecuteNonQuery();
        }

        public static void EditRole(string id, string name)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE Roles SET name = @Name WHERE Id = @Id", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@Name", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Name"].Value = name;
                myDataAdapter.SelectCommand.Parameters.Add("@Id", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Id"].Value = id;
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.UpdateCommand.ExecuteNonQuery();
            }
            //string queryString = "UPDATE Roles SET name = '" + name + "' WHERE Id = '" + id + "'";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //da.UpdateCommand = command;
            //da.InsertCommand.ExecuteNonQuery();
        }

        public static void CreateProfile(string id, string name)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Profiles Values ( @Id , @Name)", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@Name", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Name"].Value = name;
                myDataAdapter.SelectCommand.Parameters.Add("@Id", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Id"].Value = id;
                myDataAdapter.InsertCommand = command;
                myDataAdapter.InsertCommand.ExecuteNonQuery();
            }
            //string queryString = "INSERT INTO Profiles Values ('" + id + "','" + name + "')";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //da.InsertCommand = command;
            //da.InsertCommand.ExecuteNonQuery();
        }

        public static void EditProfile(string id, string name)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE Profiles SET name = @Name WHERE Id = @Id", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@Name", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Name"].Value = name;
                myDataAdapter.SelectCommand.Parameters.Add("@Id", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Id"].Value = id;
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.UpdateCommand.ExecuteNonQuery();
            }
            //string queryString = "UPDATE Profiles SET name = '" + name + "' WHERE Id = '" + id + "'";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //da.UpdateCommand = command;
            //da.InsertCommand.ExecuteNonQuery();
        }

        public static void CreateProfileRole(string id, string profileId, string profileName, string roleId, string roleName)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO ProfileRoles Values ( @Id , @ProfileId , @ProfileName , @RoleId , @RoleName)", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@ProfileName", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@ProfileName"].Value = profileName;
                myDataAdapter.SelectCommand.Parameters.Add("@RoleName", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@RoleName"].Value = roleName;
                myDataAdapter.SelectCommand.Parameters.Add("@ProfileId", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@ProfileId"].Value = profileId;
                myDataAdapter.SelectCommand.Parameters.Add("@RoleId", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@RoleId"].Value = roleId;
                myDataAdapter.SelectCommand.Parameters.Add("@Id", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Id"].Value = id;
                myDataAdapter.InsertCommand = command;
                myDataAdapter.InsertCommand.ExecuteNonQuery();
            }
            //string queryString = "INSERT INTO ProfileRoles Values ('" + id + "','" + profileId + "','" + roleId + "')";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //da.InsertCommand = command;
            //da.InsertCommand.ExecuteNonQuery();
        }

        public static void EditProfileRoles(string id, string profileId, string profileName, string roleId, string roleName)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE ProfileRoles SET ProfileId = @ProfileId , RoleId = @RoleId , ProfileName = @ProfileName , RoleName = @RoleName WHERE Id = @Id", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@ProfileName", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@ProfileName"].Value = profileName;
                myDataAdapter.SelectCommand.Parameters.Add("@RoleName", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@RoleName"].Value = roleName;
                myDataAdapter.SelectCommand.Parameters.Add("@ProfileId", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@ProfileId"].Value = profileId;
                myDataAdapter.SelectCommand.Parameters.Add("@RoleId", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@RoleId"].Value = roleId;
                myDataAdapter.SelectCommand.Parameters.Add("@Id", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Id"].Value = id;
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.UpdateCommand.ExecuteNonQuery();
            }
        }

        public static List<Users> UserSearch(string searchText)
        {
            DataSet userDataset = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(
                    "SELECT * FROM Users WHERE UserName like '%' + @UserName + '%'",
                    connection);
                myDataAdapter.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@UserName"].Value = searchText;
                myDataAdapter.Fill(userDataset);
            }

            List<Users> users = new List<Users>();
            foreach (DataRow dr in userDataset.Tables["Table"].Rows)
            {
                Users u = new Users();
                u.Id = dr["Id"].ToString();
                u.UserName = dr["UserName"].ToString();
                u.Email = dr["Email"].ToString();
                u.Password = dr["Password"].ToString();
                u.ProfilePic = dr["ProfilePic"].ToString();
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
                if (dr["EmailConfirmed"] != System.DBNull.Value)
                {
                    u.EmailConfirmed = bool.Parse(dr["EmailConfirmed"].ToString());
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

        public static Users GetUser(string userId)
        {
            DataSet userDataset = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Users where Id = @Id", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@Id", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Id"].Value = userId;
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.Fill(userDataset);
            }
            //string queryString = "SELECT * FROM Users where Id = '" + userId + "'";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //DataSet ds = new DataSet();
            //da.Fill(ds);

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

                return u;
            }

            return u;
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

        public static List<Users> GetAllUsers()
        {
            DataSet userDataset = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Users", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.Fill(userDataset);
            }
            //string queryString = "SELECT * FROM Users";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //DataSet ds = new DataSet();
            //da.Fill(ds);

            List<Users> users = new List<Users>();
            foreach (DataRow dr in userDataset.Tables["Table"].Rows)
            {
                Users u = new Users();
                u.Id = dr["Id"].ToString();
                u.UserName = dr["UserName"].ToString();
                u.Email = dr["Email"].ToString();
                u.Password = dr["Password"].ToString();
                u.ProfilePic = dr["ProfilePic"].ToString();
                if (dr["ProfileId"].ToString() != "")
                {
                    u.ProfileId = dr["ProfileId"].ToString();
                    u.Gender = dr["Gender"].ToString();
                    u.GenderPreference = dr["GenderPreference"].ToString();
                    
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

        public static Roles GetRole(string roleId)
        {
            DataSet roleDataset = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Roles where Id = @Id", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@Id", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Id"].Value = roleId;
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.Fill(roleDataset);
            }
            //string queryString = "SELECT * FROM Roles where Id = '" + roleId + "'";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //DataSet ds = new DataSet();
            //da.Fill(ds);

            Roles r = new Roles();
            foreach (DataRow dr in roleDataset.Tables["Table"].Rows)
            {
                r.Id = dr["Id"].ToString();
                r.name = dr["Name"].ToString();

                return r;
            }

            return r;
        }

        public static Profiles GetProfile(string profileId)
        {
            DataSet profileDataset = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Profiles where Id = @Id", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@Id", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Id"].Value = profileId;
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.Fill(profileDataset);
            }
            //string queryString = "SELECT * FROM Profiles where Id = '" + profileId + "'";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //DataSet ds = new DataSet();
            //da.Fill(ds);

            Profiles p = new Profiles();
            foreach (DataRow dr in profileDataset.Tables["Table"].Rows)
            {
                p.Id = dr["Id"].ToString();
                p.name = dr["Name"].ToString();

                return p;
            }

            return p;
        }

        public static List<Profiles> GetAllProfiles()
        {
            DataSet profileDataset = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Profiles", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.Fill(profileDataset);
            }
            //string queryString = "SELECT * FROM Profiles";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //DataSet ds = new DataSet();
            //da.Fill(ds);

            List<Profiles> profiles = new List<Profiles>();
            foreach (DataRow dr in profileDataset.Tables["Table"].Rows)
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
            DataSet roleDataset = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Roles", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.Fill(roleDataset);
            }
            //string queryString = "SELECT * FROM Roles";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //DataSet ds = new DataSet();
            //da.Fill(ds);

            List<Roles> roles = new List<Roles>();
            foreach (DataRow dr in roleDataset.Tables["Table"].Rows)
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
            DataSet profileDataset = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Profiles where Id = @Id", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@Id", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@Id"].Value = userId;
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.Fill(profileDataset);
            }
            //string queryString = "SELECT * FROM Profiles where UserId = '" + userId + "'";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //DataSet ds = new DataSet();
            //da.Fill(ds);

            return profileDataset.Tables[0].Columns["Id"].ToString();
        }

        public static Profiles GetProfileByName(string profileName)
        {
            DataSet profileDataset = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Profiles where name = @ProfileName", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@ProfileName", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@ProfileName"].Value = profileName;
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.Fill(profileDataset);
            }
            //string queryString = "SELECT * FROM ProfileRoles where ProfileId = '" + profileId + "'";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //DataSet ds = new DataSet();
            //da.Fill(ds);

            Profiles pr = new Profiles();
            foreach (DataRow dr in profileDataset.Tables["Table"].Rows)
            {
                pr.Id = dr["Id"].ToString();
                pr.name = dr["name"].ToString();
            }

            return pr;
        }

        public static Roles GetRoleByName(string roleName)
        {
            DataSet profileDataset = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Roles where name = @RoleName", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@RoleName", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@RoleName"].Value = roleName;
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.Fill(profileDataset);
            }
            //string queryString = "SELECT * FROM ProfileRoles where ProfileId = '" + profileId + "'";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //DataSet ds = new DataSet();
            //da.Fill(ds);

            Roles roles = new Roles();
            foreach (DataRow dr in profileDataset.Tables["Table"].Rows)
            {
                roles.Id = dr["Id"].ToString();
                roles.name = dr["name"].ToString();
            }

            return roles;
        }

        public static List<ProfileRoles> GetProfileRoles(string profileId)
        {
            DataSet profileDataset = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM ProfileRoles where ProfileId = @ProfileId", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.SelectCommand.Parameters.Add("@ProfileId", SqlDbType.VarChar);
                myDataAdapter.SelectCommand.Parameters["@ProfileId"].Value = profileId;
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.Fill(profileDataset);
            }
            //string queryString = "SELECT * FROM ProfileRoles where ProfileId = '" + profileId + "'";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //DataSet ds = new DataSet();
            //da.Fill(ds);

            List<ProfileRoles> profiles = new List<ProfileRoles>();
            foreach (DataRow dr in profileDataset.Tables["Table"].Rows)
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
            DataSet profileDataset = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM ProfileRoles", connection);
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command);
                myDataAdapter.UpdateCommand = command;
                myDataAdapter.Fill(profileDataset);
            }
            //string queryString = "SELECT * FROM ProfileRoles";
            //SqlCommand command = new SqlCommand(queryString, Connection);
            //SqlDataAdapter da = new SqlDataAdapter(command);
            //DataSet ds = new DataSet();
            //da.Fill(ds);

            List<ProfileRoles> profiles = new List<ProfileRoles>();
            foreach (DataRow dr in profileDataset.Tables["Table"].Rows)
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