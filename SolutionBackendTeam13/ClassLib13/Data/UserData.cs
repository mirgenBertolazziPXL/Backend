using ClassLib13.Business.Entities;
using ClassLib13.Data.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace ClassLib13.Data
{
    public class UserData : SqlServer
    {
        public UserData()
        {
            TableName = "Users";
        }

        public string TableName { get; set; }

        public SelectResult Select()
        {
            return base.Select(TableName);
        }
        public SelectResult GetUser(string email)
        {
            string query = "SELECT * FROM Users WHERE Email = @Value";
            using (SqlCommand command = new SqlCommand(query))
            {
                command.Parameters.AddWithValue("@Value", email);
                return Select(command);
            }
        }

        public InsertResult Insert(User user)
        {
            var result = new InsertResult();
            try
            {
         
                StringBuilder insertQuery = new StringBuilder();
                insertQuery.Append($"INSERT INTO {TableName} ");
                insertQuery.Append("(email, password, roleId, firstname, lastname, schoolId, parentId) VALUES ");
                insertQuery.Append("(@email, @password, @roleId, @firstname, @lastname, @schoolId, @parentId);");

                using (SqlCommand insertCommand = new SqlCommand(insertQuery.ToString()))
                {
                    insertCommand.Parameters.AddWithValue("@Email", user.Email);
                    insertCommand.Parameters.AddWithValue("@Password", user.Password);
                    insertCommand.Parameters.AddWithValue("@RoleId", user.RoleId);
                    insertCommand.Parameters.AddWithValue("@Firstname", user.Firstname);
                    insertCommand.Parameters.AddWithValue("@Lastname", user.LastName);
                    insertCommand.Parameters.AddWithValue("@SchoolId", (object)user.SchoolId ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@ParentId", (object)user.ParentId ?? DBNull.Value);

                    result = InsertRecord(insertCommand);
                }
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }
        public bool RegisterAdmin(string email)
        {
            string query = "UPDATE Users SET Admin =  1, RoleId = 1 WHERE Email = @Email";

            using (SqlConnection connection = new SqlConnection(Settings.GetConnectionString()))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }

        }

        public bool UpdateUser(int userId, string email, string firstName, string lastName, int roleId)
        {
            string query = "UPDATE Users SET Email = @email, Firstname = @firstName, Lastname = @lastName, RoleId = @roleId WHERE Id = @userId";

            using (SqlConnection connection = new SqlConnection(Settings.GetConnectionString()))
            using (SqlCommand updateCommand = new SqlCommand(query, connection))
            {
                updateCommand.Parameters.AddWithValue("@email", email);
                updateCommand.Parameters.AddWithValue("@firstName", firstName);
                updateCommand.Parameters.AddWithValue("@lastName", lastName);
                updateCommand.Parameters.AddWithValue("@roleId", roleId);
                updateCommand.Parameters.AddWithValue("@userId", userId);

                connection.Open();
                int rowsAffected = updateCommand.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        public bool AdminUpdateUser(int userId, int parentId, string email, string firstName, string lastName, int roleId)
        {
            string query = "UPDATE Users SET Email = @email, ParentId = @parentId, Firstname = @firstName, Lastname = @lastName, RoleId = @roleId WHERE Id = @userId";
            using (SqlConnection connection = new SqlConnection(Settings.GetConnectionString()))
            using (SqlCommand updateCommand = new SqlCommand(query, connection))
            {
                updateCommand.Parameters.AddWithValue("@email", email);
                updateCommand.Parameters.AddWithValue("@firstName", firstName);
                updateCommand.Parameters.AddWithValue("@lastName", lastName);
                updateCommand.Parameters.AddWithValue("@roleId", roleId);
                updateCommand.Parameters.AddWithValue("@userId", userId);
                if (parentId < 1 || string.IsNullOrEmpty(parentId.ToString()))
                {
                    updateCommand.Parameters.AddWithValue("@parentId", DBNull.Value);
                }
                else
                {
                    updateCommand.Parameters.AddWithValue("@parentId", (object)parentId);
                }

                    connection.Open();
                int rowsAffected = updateCommand.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool UpdatePassword(string email, string password)
        {
            string query = "UPDATE Users SET Password = @Password WHERE Email = @Email";

            using (SqlConnection connection = new SqlConnection(Settings.GetConnectionString()))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Email", email);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        public SelectResult FilterUser(string table, string value)
        {
            using (SqlCommand command = new SqlCommand($"SELECT * FROM USERS WHERE {table} = @Value"))
            {
                command.Parameters.AddWithValue("@Value", value);
                return Select(command);
            }
        }
        public SelectResult OwnUser(string value)
        {
            using (SqlCommand command = new SqlCommand($"SELECT * FROM USERS WHERE Email = @Value"))
            {
                command.Parameters.AddWithValue("@Value", value);
                return Select(command);
            }
        }
        public bool LogUserMethod(string emailuser)
        {
            using (SqlConnection connection = new SqlConnection(Settings.GetConnectionString()))
            {
                connection.Open(); 

                using (SqlCommand selectCommand = new SqlCommand("SELECT Id FROM Users WHERE Email = @Email", connection))
                {
                    selectCommand.Parameters.Add("@Email", SqlDbType.VarChar).Value = emailuser;
                    object result = selectCommand.ExecuteScalar();

                    if (result == null)
                    {
                        return false; 
                    }

                    int userId = Convert.ToInt32(result);

                    
                    using (SqlCommand selectCommandlogs = new SqlCommand(
                        "SELECT TOP 1 LogDate FROM UserLogs WHERE UserId = @UserId ORDER BY LogDate DESC",
                        connection))
                    {
                        selectCommandlogs.Parameters.Add("@UserId", SqlDbType.Int).Value = userId; 
                        object resultlog = selectCommandlogs.ExecuteScalar();

                        if (resultlog != null && resultlog != DBNull.Value)
                        {
                            DateTime logDate;
                            if (DateTime.TryParse(resultlog.ToString(), out logDate)) 
                            {
                                if (logDate.Date == DateTime.Today)
                                {
                                    return false; 
                                }
                            }
                        }
                    }

                    
                    using (SqlCommand insertCommand = new SqlCommand(
                        "INSERT INTO UserLogs (LogDate, UserId) VALUES (@LogDate, @UserId)",
                        connection))
                    {
                        insertCommand.Parameters.Add("@LogDate", SqlDbType.DateTime).Value = DateTime.Now;
                        insertCommand.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                        return insertCommand.ExecuteNonQuery() > 0;
                    }
                }
            }
        }

        public SelectResult FilterUser(string value)
        {
            using (SqlCommand command = new SqlCommand($"SELECT * FROM USERS WHERE {value}"))
            {
                return Select(command);
            }
        }

    }}

