using Microsoft.AspNetCore.Mvc;
using ClassLib13.Business;  

namespace WebAppTeam13
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetStudents()  
        {
            var students = Students.List(); 
            return Ok(students);
        }
    }
}
