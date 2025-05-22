using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace WebAppTeam13
{

    [Route("api/protected")]
    [ApiController]
    public class ProtectedController : ControllerBase
    {
        [HttpGet]
        [Authorize] // 🔹 Enforce authentication
        public IActionResult GetProtectedData()
        {
            return Ok(new { Message = "This is a protected route." });
        }
    }
}
