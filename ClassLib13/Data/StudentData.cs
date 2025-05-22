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
    internal class StudentData : SqlServer
    {
        public StudentData()
        {
            TableName = "students";
        }
        public string TableName { get; set; }
        public SelectResult Select()
        {
            return base.Select(TableName);
        }
        public InsertResult Insert(Student student)
        {
            var result = new InsertResult();
            try
            {
                //SQL Command
                StringBuilder insertQuery = new StringBuilder();
                insertQuery.Append($"Insert INTO {TableName} ");
                insertQuery.Append($"(firstname, lastname) VALUES ");
                insertQuery.Append($"(@firstname, @lastname); ");

                using (SqlCommand insertCommand = new SqlCommand(insertQuery.ToString()))
                {
                    insertCommand.Parameters.Add("@firstname", SqlDbType.VarChar).Value =
                    student.FirstName;
                    insertCommand.Parameters.Add("@lastname", SqlDbType.VarChar).Value =
                    student.LastName;
                    result = InsertRecord(insertCommand);
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