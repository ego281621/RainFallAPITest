// Import necessary namespaces for ASP.NET Core MVC, exceptions, and services
using Microsoft.AspNetCore.Mvc;
using RainfallAPI.Enum;
using RainfallAPI.Models;
using RainfallAPI.Services;
using System.Net;

// Namespace for the RainfallController class
namespace RainfallAPI.Controllers
{
    // Attribute routing for the RainfallController, specifying base route and controller type
    [Route("api/[controller]")]
    [ApiController]
    // RainfallController class inheriting from ControllerBase
    public class RainfallController : ControllerBase
    {
        // Private field to hold the rainfall service dependency
        private IRainfallService _rainfallService;

        // Constructor to inject the rainfall service dependency
        public RainfallController(IRainfallService rainfallService)
        {
            _rainfallService = rainfallService;
        }

        // HTTP GET method to retrieve rainfall readings for a specific station
        [HttpGet("id/{stationId}/readings")]
        // Action method to handle GET requests for rainfall readings
        public async Task<IActionResult> GetRainfallReadingsAsync([FromRoute] int stationId, [FromQuery] int count = 10)
        {
            try
            {
                // Check if the station ID is valid
                if (stationId < 1)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Message = ErrorTypeExtensions.GetErrorMessage(ErrorType.InvalidRequest),
                        Detail = new List<ErrorDetail>
                        {
                            new ErrorDetail { PropertyName = nameof(stationId), Message = $"The {nameof(stationId)} is not valid." }
                        }
                    });
                }

                // Check if count is within the valid range (1-100)
                if (count < 1 || count > 100)
                {
                    // Return a BadRequest response with an error message
                    return BadRequest(new ErrorResponse
                    {
                        Message = ErrorTypeExtensions.GetErrorMessage(ErrorType.InvalidRequest),
                        Detail = new List<ErrorDetail>
                        {
                            new ErrorDetail { PropertyName = nameof(count), Message = "Must be a positive integer between 1 to 100 only." }
                        }
                    });
                }

                // Call service to get readings asynchronously
                var readings = await _rainfallService.GetRainfallReadingsAsync(stationId, count);

                // If no readings are found, return no content
                if (!readings.Any())
                {
                    return NoContent();
                }

                // Return OK status code with the retrieved readings
                return Ok(new RainfallReadingResponse { Readings = readings });
            }
            catch (Exception ex)
            {
                // If an unexpected error occurs, return Internal Server Error with error details
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse
                {
                    Message = ErrorTypeExtensions.GetErrorMessage(ErrorType.InternalServerError),
                    Detail = new List<ErrorDetail>
                    {
                        new ErrorDetail { PropertyName = ex.Source, Message = ex.Message }
                    }
                });
            }

        }

    }
}
