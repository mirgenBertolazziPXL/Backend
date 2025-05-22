using ClassLib13.Business;
using ClassLib13.Data.Framework;
using ClassLib13.Data;
using ClassLib13.Utils;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using WebAppTeam13.Services;
using ClassLib13.Business.Entities;
using Newtonsoft.Json;

namespace WebAppTeam13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLogController : ControllerBase
    {

        [HttpGet]
        [Route("GetUserLogs")]
        public IActionResult GetUserLogs([FromQuery] List<int>filterIds, [FromQuery] string groupBy)
        {
            var result = UserLogs.GetUserLogs(filterIds, new UserLogData(), groupBy);
            if (result.Succeeded)
            {
                var userLogs = result.DataTable;
                string JSONresult = JsonConvert.SerializeObject(userLogs);
                return Ok(JSONresult);
            }
            return NotFound();
        }
        [HttpGet]
        [Route("GetLastUserLogs")]
        public IActionResult GetLastUserLogss()
        {
            var result = UserLogs.GetLastUserLogs(new UserLogData());
            if (result.Succeeded)
            {
                var userLogs = result.DataTable;
                string JSONresult = JsonConvert.SerializeObject(userLogs);
                return Ok(JSONresult);
            }
            return NotFound();
        }
    }
}
