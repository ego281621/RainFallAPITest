using Microsoft.AspNetCore.Mvc;
using RainfallAPI.Enum;
using RainfallAPI.Models;
using RainfallAPI.Services;
using System.Net;

namespace RainfallAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RainfallController : ControllerBase
    {
        private readonly IRainfallService _rainfallService;

        // Constructor to inject the rainfall service dependency
        public RainfallController(IRainfallService rainfallService)
        {
            _rainfallService = rainfallService;
        }

        [HttpGet("id/{stationId}/readings")]
        public async Task<IActionResult> GetRainfallReadingsAsync([FromRoute] int stationId, [FromQuery] int count = 10)
        {
            // Validate input parameters

            // Initialize a list to store error details
            var errorDetails = new List<ErrorDetail>();

            // Validate the stationId parameter
            if (stationId < 1)
                errorDetails.Add(new ErrorDetail { PropertyName = nameof(stationId), Message = $"The {nameof(stationId)} is not valid." });

            // Validate the count parameter using pattern matching
            if (count is < 1 or > 100)
                errorDetails.Add(new ErrorDetail { PropertyName = nameof(count), Message = "Must be a positive integer between 1 to 100 only." });

            // If any error details were added, return a bad request response
            if (errorDetails.Any())
                return BadRequest(new ErrorResponse
                {
                    Message = ErrorTypeExtensions.GetErrorMessage(ErrorType.InvalidRequest),
                    Detail = errorDetails
                });

            try
            {
                // Call the service to get rainfall readings asynchronously
                var readings = await _rainfallService.GetRainfallReadingsAsync(stationId, count);

                // If there are readings, return an OK response with the RainfallReadingResponse;
                // otherwise, return a NoContent response
                return readings.Any() ? Ok(new RainfallReadingResponse { Readings = readings }) : NoContent();
            }
            catch (Exception ex)
            {
                // If an unexpected error occurs, add error details for logging
                errorDetails.Add(new ErrorDetail { PropertyName = ex.Source, Message = ex.Message });

                // Return an Internal Server Error response with error details
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse
                {
                    Message = ErrorTypeExtensions.GetErrorMessage(ErrorType.InternalServerError),
                    Detail = errorDetails
                });
            }
        }
    }
}
