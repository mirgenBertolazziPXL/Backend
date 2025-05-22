using ClassLib13.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebAppTeam13
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetRoles()
        {
            var result = Roles.GetRoles();
            if (result.Succeeded)
            {
                var role = result.DataTable;
                string JSONresult = JsonConvert.SerializeObject(role);
                return Ok(JSONresult);
            }
            return NotFound();
        }
    }
}

    

