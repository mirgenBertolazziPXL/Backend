using ClassLib13.Business;
using ClassLib13.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace WebAppTeam13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangePassEmail : ControllerBase
    {
        [HttpGet]
        public IActionResult ChangePass(string email)
        {
            return Content($@"
            <html>
                <body style='display:flex;align-items:center'>
                    <div style='display:flex;flex-direction: column;margin:auto;justify-content:center;width:25%; height:50%;border-radius:5px;box-shadow:0 10 black; align-self:center;'>
                    <h3>Set a password for {email}</h3>
                    <form method='post'>
                        <input style='border-radius:5px;' name='password' type='password' placeholder='New password' required />
                        <input style='border-radius:5px;' name='password' type='password' placeholder='Confirm new password' required />
                        <input style='border-radius:5px;' type='hidden' name='email' value='{email}' />
                        <button style='border-radius:5px; background-color:#F0743E; border:none;padding:5px;' type='submit'>Set Password</button>
                    </form>
                    </div>

                <script>
                    
                </script>
                </body>
            </html>", "text/html");
                }

           [HttpPost]
    public IActionResult ChangePass([FromForm] string email, [FromForm] string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return BadRequest("Email and password are required.");

        // Hash the new password
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        // Use your existing method
        UserData userData = new UserData();
        bool result = userData.UpdatePassword(email, hashedPassword);

        if (result)
            return Content("<h3>Password successfully updated!</h3><p>You may now log in with your new password.</p>", "text/html");

        return StatusCode(500, "Error updating password.");
    }

    }
}
