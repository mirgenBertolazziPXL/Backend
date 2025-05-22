using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using ClassLib13.Data;
using Newtonsoft.Json;

namespace WebAppTeam13
{
    // [Authorize] // Ensures only authenticated users can access
    [ApiController]
    [Route("api/[controller]")]
    public class GetOwnUserController : ControllerBase
    {
        [HttpGet("email")]
        public IActionResult GetEmailFromToken([FromQuery] string email)
        {
            if (email == null)
            {
                return BadRequest("Data is required.");
            }

            Console.WriteLine($"Received: {JsonConvert.SerializeObject(email)}"); // Log received data

            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Invalid filter parameters.");
            }

            UserData userData = new UserData();
            var result = userData.GetUser(email);

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


    }
}