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
    public class RoleData : SqlServer
    {
        public RoleData()
        {
            TableName = "Roles";
        }

        public string TableName { get; set; }

        public SelectResult Select()
        {
            return base.Select(TableName);
        }

        public InsertResult Insert(Role role)
        {
            var result = new InsertResult();
            try
            {
                StringBuilder insertQuery = new StringBuilder();
                insertQuery.Append($"INSERT INTO {TableName} ");
                insertQuery.Append("(Id, name) VALUES ");
                insertQuery.Append("(@Id, @name);");

                using (SqlCommand insertCommand = new SqlCommand(insertQuery.ToString()))
                {

                    insertCommand.Parameters.Add("@Id", SqlDbType.VarChar).Value = role.RoleId;
                    insertCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = role.Name;

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