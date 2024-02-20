using Microsoft.AspNetCore.Mvc;
using RainfallAPI.Enum;
using RainfallAPI.Models;
using RainfallAPI.Services;
using System.Net;


namespace RainfallAPI.Controllers
{
    public class RainfallController : ControllerBase
    {
        private readonly IRainfallService _rainfallService;

        // Constructor to inject the rainfall service dependency
        public RainfallController(IRainfallService rainfallService)
        {
            _rainfallService = rainfallService;
        }

        /// <summary>
        /// Gets rainfall readings for a given station.
        /// </summary>
        /// <param name="stationId">The ID of the station.</param>
        /// <param name="count">The number of readings to retrieve (default is 10, max is 100).</param>
        /// <returns>The rainfall readings for the specified station.</returns>
        [HttpGet("id/{stationId}/readings")]
        public async Task<IActionResult> GetRainfallReadingsAsync([FromRoute] string stationId, [FromQuery] int count = 10)
        {
            // Validate input parameters

            // Initialize a error response to store error details
            var errorResponse = new ErrorResponse();

            // Validate the stationId parameter
            if (!int.TryParse(stationId, out int value))
            {
                errorResponse.Detail.Add(new ErrorDetail { PropertyName = nameof(stationId), Message = $"The {nameof(stationId)} is not valid." });
            }

            // Validate the count parameter using pattern matching
            if (count is < 1 or > 100)
                errorResponse.Detail.Add(new ErrorDetail { PropertyName = nameof(count), Message = "Must be a positive integer between 1 to 100 only." });

            // If any error details were added, return a bad request response
            if (errorResponse.Detail.Any())
            {
                errorResponse.Message = ErrorTypeExtensions.GetErrorMessage(ErrorType.InvalidRequest);
                return BadRequest(errorResponse);
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
                // If an unexpected error occurs, add error details for logging
                errorResponse.Message = ErrorTypeExtensions.GetErrorMessage(ErrorType.InternalServerError);
                errorResponse.Detail.Add(new ErrorDetail { PropertyName = ex.Source, Message = ex.Message });

                // Return an Internal Server Error response with error details
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }
    }
}
