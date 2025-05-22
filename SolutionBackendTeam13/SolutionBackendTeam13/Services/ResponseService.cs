using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace WebAppTeam13.Services
{
    public class ResponseService
    {
        public static IActionResult GetResponse(int statusCode, string message)
        {
            return new ObjectResult(new { message })
            {
                StatusCode = statusCode switch
                {
                    400 => StatusCodes.Status400BadRequest,
                    404 => StatusCodes.Status404NotFound,
                    500 => StatusCodes.Status500InternalServerError,
                    502 => StatusCodes.Status502BadGateway,
                    201 => StatusCodes.Status201Created,
                    _ => StatusCodes.Status200OK
                }
            };
        }

        public static IActionResult IsException(Exception exception)
        {
            return exception switch
            {
                SqlException sqlException => 
                    ResponseService.GetResponse(500, $"Sql error: {sqlException.Message}"),

                ArgumentNullException argumentNullException => 
                    ResponseService.GetResponse(400, $"Argument error: {argumentNullException.Message}"),

                IndexOutOfRangeException indexOutOfRangeException => 
                    ResponseService.GetResponse(500, $"Index out of range error: {indexOutOfRangeException.Message}"),

                TimeoutException timeoutException => 
                    ResponseService.GetResponse(504, $"Timeout error: response took to long"),

                _ => ResponseService.GetResponse(500, $"Unexpected error: {exception.Message}")
            };
        }
    }
}
