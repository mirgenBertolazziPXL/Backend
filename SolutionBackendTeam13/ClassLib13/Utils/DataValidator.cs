using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ClassLib13.Business;

namespace ClassLib13.Utils
{
    public class DataValidator
    {        
        public static List<string> AllowedTypes => LoadAllData("Types");
        public static List<string> AllowedTables => LoadAllData("Tables");

        private static List<string> LoadAllData(string input)
        {
            return input == "Tables" ? new List<string>() { "excercises", "subscriptions", "challenges" } : new List<string>() { "Exercise", "Subscription", "Challenge", "all" };
        }

        public static bool TypeExists(string typeName) => AllowedTypes.Contains(typeName);
        public static bool TableExists(string tableName) => AllowedTables.Contains(tableName);

        private static readonly Dictionary<string, List<string>> TypeFilters = new Dictionary<string, List<string>>()
        {
            { "Exercise", new List<string>{ "Id","Sport", "Grade" } },
            { "Subscription", new List<string>{ "Price", "Duration" } },
            { "Challenge", new List<string>{ "Points", "Title" } },
            { "All", new List<string>{ "Sport", "Grade", "Points", "Price" } }
        };

        private static readonly Dictionary<string, string> TableToType = new Dictionary<string, string>()
        {
            { "excercises", "Exercise" },
            { "subscriptions", "Subscription" },
            { "challenges", "Challenge" },
            { "all", "All" }
        };

        public static List<string> GetFilters(string typeName) => TypeFilters.TryGetValue(typeName, out var filters) ? filters : new List<string>();
        public static List<string> GetFiltersForTable(string typeName) => TableToType.TryGetValue(typeName, out var type) ? GetFilters(type) : new List<string>();


        public static string ValidateData(string productType, ref Dictionary<string, string> productFilters)
        {
            if (productFilters.ContainsKey("productType"))
            { productFilters.Remove("productType"); }

            if (string.IsNullOrEmpty(productType))
            { return $"Missing required parameter: {productType}"; }

            if (AllowedTables.Contains(productType))
            {
                if (!TableExists(productType))
                { return $"Table: {productType} does not exist"; }
            }
            else
            {
                if (!TypeExists(productType))
                { return $"Type: {productType} does not exist"; }
            }

                var wrongFiltersList = productFilters.Keys.Where(key => !GetFiltersForTable(productType).Contains(key)).ToList();
            if (wrongFiltersList.Count > 0) 
            { return $"Error: ({string.Join(", ", wrongFiltersList)}) these filters don't exist for '{productType}'... I cancelled the operation"; }

            if (productFilters.ContainsKey("Grade") && !int.TryParse(productFilters["Grade"], out _)) 
            { return $"Error: '{productFilters["Grade"]}' is not a valid number"; }

            return null;
        }

        public static string ValidateRemoveData(Dictionary<string, string> productsToDelete)
        {
            foreach(KeyValuePair<string, string> productsTable in productsToDelete)
            {
                string tableName = productsTable.Key;
                List<int> productIDs;
                try
                {
                    productIDs = productsTable.Value.Split(',').Select(id => int.Parse(id)).ToList();
                }
                catch
                {
                    return $"You sent non numbers ({productsTable.Value})";
                }

                if (string.IsNullOrWhiteSpace(tableName))
                { return $"Tablename: '{tableName}' can't be empty"; }

                if (!TableExists(tableName))
                { return $"Tablename: '{tableName}' Does not exist in the database"; }

                if (productIDs.Any(id => id <= 0))
                { return $"ProductIDs: '{string.Join(", ", productIDs.Where(id => id <= 0))}' are not valid numbers"; }
            }
            return null;
        }

        public static string ValidateUpdateData(string tableName, int productId, Dictionary<string, string> values)
        {
            if (!values.Any())
            { return $"No update values found ({values.Values})"; }

            if (!TableExists(tableName))
            { return $"Tablename: '{tableName}' Does not exist in the database"; }

            if (productId <= 0)
            { return $"ProductID: '{productId}' is not valid (minimum 1 or higher)"; }

            return null;
        } 
    }
}
