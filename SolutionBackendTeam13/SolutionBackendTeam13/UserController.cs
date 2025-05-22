using ClassLib13;
using ClassLib13.Business;
using ClassLib13.Business.Entities;
using ClassLib13.Data.Framework;
using ClassLib13.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using BCrypt;
using ClassLib13.Business.Entities.UserController;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace WebAppTeam13
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        /// <summary>
        /// Get all users out of database
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("GetUsers")]
        public ActionResult GetUsers()
        {
            var result = Users.GetUsers();
            if (result.Succeeded)
            {
                var users = result.DataTable;
                string JSONresult = JsonConvert.SerializeObject(users);
                return Ok(JSONresult);
            }
            return NotFound();
        }
     
        /// <summary>
        /// Login Api 
        /// </summary>
        /// <param name="request">Has the request body from the frontend</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.email) || string.IsNullOrWhiteSpace(request.password))
            {
                return BadRequest("Invalid login request.");
            }

            SelectResult result = Users.GetUsers();
            if (result == null || result.DataTable == null || result.DataTable.Rows.Count == 0)
            {
                return NotFound("No student records available.");
            }

            DataRow userRow = result.DataTable.AsEnumerable()
                .FirstOrDefault(row => row["Email"]?.ToString().Equals(request.email, StringComparison.OrdinalIgnoreCase) == true);

            if (userRow == null)
            {
                return Unauthorized("User Not Found");
            }

            if (!(userRow["RoleId"] is int roleId))
            {
                return Unauthorized("Invalid role.");
            }

            if (roleId != 1)
            {
                return Unauthorized("No Admin access.");
            }

            var stored = userRow["Password"].ToString();
            if (BCrypt.Net.BCrypt.Verify(request.password, stored))
            {
                LogUsers logUsers = new LogUsers();
                logUsers.UpdateLogs(request.email);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"] ?? "your_default_secret_key");
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, request.email) };

                if (userRow["RoleId"] != DBNull.Value)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRow["RoleId"].ToString()));
                    claims.Add(new Claim(ClaimTypes.GivenName, userRow["Firstname"].ToString()));
                    claims.Add(new Claim(ClaimTypes.Surname, userRow["Lastname"].ToString()));
                    claims.Add(new Claim("EmployeeId", userRow["Id"].ToString()));
                }

                var claimsIdentity = new ClaimsIdentity(claims);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claimsIdentity,
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);

                return Ok(new { Token = jwtToken, Name = request.email, Message = "Login successful." });
            }
            return Unauthorized("No valid ");
        }
        /// <summary>
        /// Create user api
        /// </summary>
        /// <param name="createUser">Class that has create user body</param>
        /// <returns></returns>

        [HttpPost]
        [Route("CreateUser")]
        public IActionResult CreateUser([FromBody] CreateUser createUser)
        {
            if (createUser == null)
            {
                return BadRequest("User data is required.");
            }

            Console.WriteLine($"Received: {JsonConvert.SerializeObject(createUser)}");

            if (string.IsNullOrWhiteSpace(createUser.Email) || string.IsNullOrWhiteSpace(createUser.Password))
            {
                return BadRequest("Email and password are required.");
            }

            string hashpass = BCrypt.Net.BCrypt.HashPassword(createUser.Password);
            UserData userData = new UserData();
            InsertResult result = Users.AddAdmin(createUser.Email.ToLower(), hashpass, createUser.FirstName, createUser.LastName, userData);

            if (result != null && result.NewId > 0)
            {
                EmailSender emailSender = new();
                emailSender.SentMail(createUser.Email);
                return Ok(new { message = "User created successfully!", userId = result.NewId });
            }
            return StatusCode(500, new { message = "An error occurred while creating the user.", errors = result?.Errors ?? new List<string> { "Unknown error" } });
        }
        /// <summary>
        /// Updates user data
        /// </summary>
        /// <param name="updateUser"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateUsers")]
        public IActionResult UpdateUser([FromBody] UpdateUser updateUser)
        {
            if (updateUser == null ||
                       updateUser.UserId <= 0 ||
                       string.IsNullOrWhiteSpace(updateUser.Email) ||
                       string.IsNullOrWhiteSpace(updateUser.FirstName) ||
                       string.IsNullOrWhiteSpace(updateUser.LastName) ||
                       updateUser.Role <= 0)
            {
                return BadRequest(new { message = "Invalid input. UserId, Email, First Name, Last Name, and Role are required." });
            }

            SelectResult result = Users.GetUsers();

            if (result == null || result.DataTable == null || result.DataTable.Rows.Count == 0)
            {
                return NotFound(new { message = "User not found." });
            }

            DataRow userRow = result.DataTable.AsEnumerable()
                .FirstOrDefault(row => Convert.ToInt32(row["Id"]) == updateUser.UserId);

            if (userRow == null)
            {
                return NotFound(new { message = "User not found." });
            }

            UserData userData = new UserData();

            bool isUpdated = userData.UpdateUser(updateUser.UserId, updateUser.Email, updateUser.FirstName, updateUser.LastName, updateUser.Role);

            if (!isUpdated)
            {
                return StatusCode(500, new { message = "Error updating user information." });
            }

            return Ok(new { message = "User information updated successfully." });

        }
        /// <summary>
        /// Updates user data
        /// </summary>
        /// <param name="adminUpdateUser"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("AdminUpdateUsers")]
        public IActionResult AdminUpdateUser([FromBody] AdminUpdateUser adminUpdateUser)
        {
            if (adminUpdateUser.UserId <= 0 || string.IsNullOrWhiteSpace(adminUpdateUser.Email) || string.IsNullOrWhiteSpace(adminUpdateUser.FirstName) || string.IsNullOrWhiteSpace(adminUpdateUser.LastName) || adminUpdateUser.Role <= 0)
            {
                return BadRequest(new { message = "Invalid input. ParentId are required." });
            }

            SelectResult result = Users.GetUsers();

            if (result == null || result.DataTable == null || result.DataTable.Rows.Count == 0)
            {
                return NotFound(new { message = "User not found." });
            }

            DataRow userRow = result.DataTable.AsEnumerable()
                .FirstOrDefault(row => Convert.ToInt32(row["Id"]) == adminUpdateUser.UserId);

            if (userRow == null)
            {
                return NotFound(new { message = "User not found." });
            }

            UserData userData = new UserData();

            bool isUpdated = userData.AdminUpdateUser(adminUpdateUser.UserId, adminUpdateUser.ParentId, adminUpdateUser.Email, adminUpdateUser.FirstName, adminUpdateUser.LastName, adminUpdateUser.Role);

            if (!isUpdated)
            {
                return StatusCode(500, new { message = "Error updating user information." });
            }

            return Ok(new { message = "User information updated successfully." });
        }
        [HttpPost]
        [Route("LoginUser")]
        public IActionResult LoginUser([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.email) || string.IsNullOrWhiteSpace(request.password))
            {
                return BadRequest("Invalid login request.");
            }

            SelectResult result = Users.GetUsers();
            if (result == null || result.DataTable == null || result.DataTable.Rows.Count == 0)
            {
                return NotFound("No student records available.");
            }

            DataRow userRow = result.DataTable.AsEnumerable()
                .FirstOrDefault(row => row["Email"]?.ToString().Equals(request.email, StringComparison.OrdinalIgnoreCase) == true);

            if (userRow == null)
            {
                return Unauthorized("User Not Found");
            }

            if (!(userRow["RoleId"] is int roleId))
            {
                return Unauthorized("Invalid role.");
            }

            var stored = userRow["Password"].ToString();
            if (BCrypt.Net.BCrypt.Verify(request.password, stored))
            {
                LogUsers logUsers = new LogUsers();
                logUsers.UpdateLogs(request.email);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"] ?? "your_default_secret_key");
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, request.email) };

                if (userRow["RoleId"] != DBNull.Value)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRow["RoleId"].ToString()));
                    claims.Add(new Claim(ClaimTypes.GivenName, userRow["Firstname"].ToString()));
                    claims.Add(new Claim(ClaimTypes.Surname, userRow["Lastname"].ToString()));
                    claims.Add(new Claim("EmployeeId", userRow["Id"].ToString()));
                }

                var claimsIdentity = new ClaimsIdentity(claims);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claimsIdentity,
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);

                return Ok(new { Token = jwtToken, Name = request.email, Message = "Login successful." });
            }
            return Unauthorized("No valid ");
        }

        [HttpPut]
        [Route("ChangePass")]
        public IActionResult ChangePasswordFromemail([FromBody] UpdateUserPass updateUserPass)
        {
            if (updateUserPass == null || string.IsNullOrWhiteSpace(updateUserPass.Email))
            {
                return BadRequest("Invalid input. Email");
            }

            SelectResult result = Users.GetUsers();
            if (result == null || result.DataTable == null || result.DataTable.Rows.Count == 0)
            {
                return NotFound("User not found.");
            }

            DataRow userRow = result.DataTable.AsEnumerable()
                .FirstOrDefault(row => row["Email"]?.ToString().Trim().ToLower() == updateUserPass.Email.Trim().ToLower());

            if (userRow == null)
            {
                return NotFound("User not found.");
            }

            EmailSender emailSender = new();
            emailSender.SentMailPassword(updateUserPass.Email);
            return Ok(new { message = "Email Sent" });
        }


    }
}

