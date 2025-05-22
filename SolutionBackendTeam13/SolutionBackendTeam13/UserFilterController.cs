using ClassLib13.Data;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebAppTeam13
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFilterController : ControllerBase
    {
        [HttpPost]
        public IActionResult Filter([FromBody] Filtervalues filtervalues)
        {
            if (filtervalues == null)
            {
                return BadRequest("Data is required.");
            }

            Console.WriteLine($"Received: {JsonConvert.SerializeObject(filtervalues)}"); // Log received data

            if (string.IsNullOrWhiteSpace(filtervalues.Value))
            {
                return BadRequest("Invalid filter parameters.");
            }

            UserData userData = new UserData();
            var result = userData.FilterUser(filtervalues.Value);

            if (result != null && result.DataTable.Rows.Count > 0)
            {
                var data = ConvertDataTableToList(result.DataTable);
                return Ok(new
                {
                    message = "Filter successful",
                    data = data
                });
            }
            else
            {
                return NotFound(new
                {
                    message = "No users found for the given filter."
                });
            }
        }

        /// <summary>
        /// Converts a DataTable to a List of Dictionaries for JSON serialization.
        /// </summary>
        private List<Dictionary<string, object>> ConvertDataTableToList(DataTable dataTable)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in dataTable.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    dict[column.ColumnName] = row[column];
                }
                list.Add(dict);
            }
            return list;
        }


        public class Filtervalues
        {
            public string Value { get; set; }
        }
    }
}
