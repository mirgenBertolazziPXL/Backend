using ClassLib13.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebAppTeam13
{
[ApiController]
public class AddAdminController : ControllerBase
{
    // GET request to activate an admin account
    [HttpGet("AddAdmin")]
    public IActionResult ActivateAccount([FromQuery] string email)
    {
        if (string.IsNullOrEmpty(email))
            return BadRequest("Email is required.");

        UserData userData = new();
        userData.RegisterAdmin(email);
           
            return Content($@"
        <html>
            <body>
                <h3>Account activated for {email}!</h3>           
            </body>
        </html>", "text/html");
        }
    }
}




