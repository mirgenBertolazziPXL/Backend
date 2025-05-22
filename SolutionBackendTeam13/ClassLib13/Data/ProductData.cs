using ClassLib13.Data.Framework;
using ClassLib13.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib13.Data
{
    public class ProductData : SqlServer
    {
        private readonly List<string> tableNames = DataValidator.AllowedTables;

        private readonly List<string> queriesArray = new List<string>() { "(Sport, Name, Grade, Description, Video) VALUES (@sport, @name, @grade, @description, @video); ",
                                                                 "(Duration, Price, Description) VALUES (@duration, @price, @description); ",
                                                                 "(Title, Description, Points) VALUES (@title, @description, @points); "};
        public InsertResult Insert(Object currentProduct, string objectType)
        {
            InsertResult result = new InsertResult();
            
            int index = DataValidator.AllowedTypes.FindIndex(name => name.Equals(objectType));
            
            string query = $"INSERT INTO {tableNames[index]} {queriesArray[index]}";
            using (SqlCommand insertCommand = new SqlCommand(query))
            {
                foreach (PropertyInfo prop in currentProduct.GetType().GetProperties())
                {
                    insertCommand.Parameters.AddWithValue($"@{prop.Name.ToLower()}", prop.GetValue(currentProduct) ?? DBNull.Value);
                }
                result = InsertRecord(insertCommand);
            }
            return result;
        }

        public SelectResult SelectSingle(Dictionary<string, string> filters, string objectType)
        {
            int index = tableNames.FindIndex(name => name.Equals(objectType.ToLower()));
            SqlCommand selectCommand = new SqlCommand();

            List<KeyValuePair<string, string>> nonEmptyFilters = filters.Where(f => !string.IsNullOrEmpty(f.Value)).ToList();
            if (nonEmptyFilters.Count > 0)
            {
                string sqlQuery = $"SELECT * FROM {tableNames[index]}";
                List<string> conditionsList = new List<string>();

                foreach (KeyValuePair<string, string> filter in nonEmptyFilters)
                {
                    string paramName = $"@{filter.Key}";
                    conditionsList.Add($"{filter.Key} = {paramName}");
                    selectCommand.Parameters.AddWithValue(paramName, filter.Value);
                }

                if (conditionsList.Any())
                {
                    sqlQuery += " WHERE " + string.Join(" AND ", conditionsList);
                }

                selectCommand.CommandText = sqlQuery;
            }
            else
            {
                selectCommand.CommandText = $"SELECT * FROM {tableNames[index]};";
            }
            return base.Select(selectCommand);
        }

        public SelectResult SelectMultiple(Dictionary<string, string> filters, string objectType)
        {
            List<KeyValuePair<string, string>> nonEmptyFiltersList = filters.Where(filter => !string.IsNullOrWhiteSpace(filter.Value)).ToList();
            SqlCommand selectCommand = new SqlCommand();
            List<string> allQueriesList = new List<string>();
            List<string> includedTableNamesList = new List<string>();

            foreach (string tableName in tableNames)
            {
                List<string> allowedFilters = DataValidator.GetFiltersForTable(tableName);
                List<KeyValuePair<string, string>> applicableFilters = nonEmptyFiltersList.Where(filter => allowedFilters.Contains(filter.Key)).ToList();

                string sql = $"SELECT * FROM {tableName}";
                if (applicableFilters.Any())
                {
                    List<string> conditionsList = new List<string>();
                    foreach (KeyValuePair<string, string> filter in applicableFilters)
                    {
                        string paramName = $"@{filter.Key}_{tableName}";
                        conditionsList.Add($"{filter.Key} = {paramName}");
                        selectCommand.Parameters.AddWithValue(paramName, filter.Value);
                    }
                    sql += " WHERE " + string.Join(" AND ", conditionsList);
                }

                allQueriesList.Add(sql);
                includedTableNamesList.Add(tableName);
            }

            selectCommand.CommandText = string.Join("; ", allQueriesList);
            return base.SelectMultiple(selectCommand, includedTableNamesList);
        }

        public bool DeleteProduct(Dictionary<string, string> productsToDelete)
        {
            SqlCommand deleteCommand = new SqlCommand();
            List<string> allQueries = new List<string>();
            int parameterNamingIndex = 0;
            foreach (KeyValuePair<string, string> productTable in productsToDelete)
            { 
                string tableName = productTable.Key;
                List<int> productIDs = productTable.Value.Split(',').Select(id => int.Parse(id)).ToList();
                List<string> parameterNames = new List<string>();

                foreach (int id in productIDs)
                {
                    string paramName = "@productId" + parameterNamingIndex;
                    deleteCommand.Parameters.AddWithValue(paramName, id);
                    parameterNames.Add(paramName);
                    parameterNamingIndex++;
                }
                string query = $"DELETE FROM {tableName} WHERE Id in ({string.Join(",", parameterNames)})";
                allQueries.Add(query);
            }
            deleteCommand.CommandText = string.Join("; ", allQueries);
            return base.DeleteProduct(deleteCommand) > 0;
        }

        public bool UpdateProduct(string tableName, int productId, Dictionary<string, string> values)
        {
            SqlCommand updateCommand = new SqlCommand();
            string query = $"UPDATE {tableName} SET ";

            List<string> updateValuesList = new List<string>();
            foreach (KeyValuePair<string, string> valuePair in values)
            {
                string paramName = $"@{valuePair.Key.ToLower()}";
                updateValuesList.Add($"{valuePair.Key} = {paramName}");
                updateCommand.Parameters.AddWithValue(paramName, valuePair.Value);
            }
            query += string.Join(",", updateValuesList);
            query += $" WHERE Id = @productID";
            updateCommand.Parameters.AddWithValue("@productID", productId);
            updateCommand.CommandText = query;

            return base.UpdateProduct(updateCommand) > 0;
        }

        public static List<Dictionary<string, object>> DataTableToList(DataTable dataTable)
        {
            List<Dictionary<string, object>> dictionaryList = new List<Dictionary<string, object>>();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                Dictionary<string, object> rowDictionary = new Dictionary<string, object>();

                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    rowDictionary[dataColumn.ColumnName] = dataRow[dataColumn];
                }

                dictionaryList.Add(rowDictionary);
            }

            return dictionaryList;
        }

        public static string ReturnSuccesMessage(int rowInput, string productType)
        {
            return rowInput > 0 
                ? $"Succesfully gathered {(productType == "All" ? productType : productType + "s")} data (rows affected: {rowInput})" 
                : $"Succes: but no records exist (rows affected: {rowInput})";
        }
    }
}
