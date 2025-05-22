using ClassLib13.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebAppTeam13
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentSqlController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetAllStudents()
        {
            var result = Students.GetStudents();
            if (result.Succeeded)
            {
                var students = result.DataTable;
                string JSONresult = JsonConvert.SerializeObject(students);
                return Ok(JSONresult);
            }
            return NotFound();
        }
    }
}