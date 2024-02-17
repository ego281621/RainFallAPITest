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
            if (stationId < 1)
            {
                // Return a bad request response if the station ID is invalid
                return BadRequest(new ErrorResponse
                {
                    Message = ErrorTypeExtensions.GetErrorMessage(ErrorType.InvalidRequest),
                    Detail = new List<ErrorDetail>
                    {
                        new ErrorDetail { PropertyName = nameof(stationId), Message = $"The {nameof(stationId)} is not valid." }
                    }
                });
            }

            if (count is < 1 or > 100)
            {
                // Return a bad request response if the count is not within the valid range (1-100)
                return BadRequest(new ErrorResponse
                {
                    Message = ErrorTypeExtensions.GetErrorMessage(ErrorType.InvalidRequest),
                    Detail = new List<ErrorDetail>
                    {
                        new ErrorDetail { PropertyName = nameof(count), Message = "Must be a positive integer between 1 to 100 only." }
                    }
                });
            }

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
                // If an unexpected error occurs, return an Internal Server Error response with error details
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