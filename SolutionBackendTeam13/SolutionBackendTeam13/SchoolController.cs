using ClassLib13.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebAppTeam13
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
            [HttpGet]
            public ActionResult GetSchools()
            {
                var result = Schools.GetSchools();
                if (result.Succeeded)
                {
                    var users = result.DataTable;
                    string JSONresult = JsonConvert.SerializeObject(users);
                    return Ok(JSONresult);
                }
                return NotFound();
            }
    }
}
