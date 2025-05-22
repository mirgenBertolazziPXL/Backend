using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib13.Data.Framework
{
        public abstract class SqlServer
    {
        SqlConnection connection;
        SqlDataAdapter adapter;
        public SqlServer()
        {
            connection = new SqlConnection(Settings.GetConnectionString());
        }
        protected SelectResult Select(SqlCommand selectCommand)
        {
            SelectResult result = new SelectResult();
            try
            {
                using (connection)
                {
                    selectCommand.Connection = connection;
                    connection.Open();
                    adapter = new SqlDataAdapter(selectCommand);
                    result.DataTable = new DataTable();
                    adapter.Fill(result.DataTable);
                    connection.Close();
                }
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }
            return result;
        }
        protected SelectResult Select(string tableName)
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = $"SELECT * FROM {tableName}";
            return Select(command);
        }
        protected SelectResult SelectMultiple(SqlCommand selectCommand, List<string> includedTableNames)
        {
            SelectResult result = new SelectResult();
            result.DataTableList = new List<DataTable>();
            using (connection)
            {
                selectCommand.Connection = connection;
                connection.Open();
                using (SqlDataReader reader = selectCommand.ExecuteReader())
                {
                    int tableIndex = 0;

                    while (!reader.IsClosed)
                    {
                        DataTable table = new DataTable();
                        table.TableName = includedTableNames[tableIndex++];
                        table.Load(reader);
                        result.DataTableList.Add(table);
                    }
                }
            }

            result.Succeeded = result.DataTableList.Count > 0;
            return result;
        }

        protected int DeleteProduct(SqlCommand deleteCommand)
        {
            using (connection)
            {
                deleteCommand.Connection = connection;
                connection.Open();
                return deleteCommand.ExecuteNonQuery();
            }
        }

        protected int UpdateProduct(SqlCommand updateCommand)
        {
            using (connection)
            {
                updateCommand.Connection = connection;
                connection.Open();
                return updateCommand.ExecuteNonQuery();
            }
        }
        protected InsertResult InsertRecord(SqlCommand insertCommand)
        {
            InsertResult result = new InsertResult();
            try
            {
                using (connection)
                {
                    insertCommand.CommandText += "SET @new_id = SCOPE_IDENTITY();";
                    insertCommand.Parameters.Add("@new_id", SqlDbType.Int).Direction =
                    ParameterDirection.Output;
                    insertCommand.Connection = connection;
                    connection.Open();
                    insertCommand.ExecuteNonQuery();
                    int newId = Convert.ToInt32(insertCommand.Parameters["@new_id"].Value);
                    result.NewId = newId;
                    connection.Close();
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
