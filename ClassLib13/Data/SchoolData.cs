using ClassLib13.Business.Entities;
using ClassLib13.Data.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib13.Data
{
    public class SchoolData : SqlServer
    {
        public SchoolData()
        {
            TableName = "Schools";
        }

        public string TableName { get; set; }

        public SelectResult Select()
        {
            return base.Select(TableName);
        }

        public InsertResult Insert(School school)
        {
            var result = new InsertResult();
            try
            {
                StringBuilder insertQuery = new StringBuilder();
                insertQuery.Append($"INSERT INTO {TableName} ");
                insertQuery.Append("(schoolid, name, subscription, userid) VALUES ");
                insertQuery.Append("(@schoolid, @name, @subscription, @userid);");

                using (SqlCommand insertCommand = new SqlCommand(insertQuery.ToString()))
                {
            
                        insertCommand.Parameters.Add("@schoolid", SqlDbType.VarChar).Value = school.SchoolId;
                        insertCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = school.Name;
                        insertCommand.Parameters.Add("@subscription", SqlDbType.Int).Value = school.Subscription;
                        insertCommand.Parameters.Add("@userid", SqlDbType.Int).Value = school.UserId;

                        insertCommand.ExecuteNonQuery();
                        
                    
                }
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }
    }
}