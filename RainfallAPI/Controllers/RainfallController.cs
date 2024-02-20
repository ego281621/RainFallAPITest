using Microsoft.AspNetCore.Mvc;
using RainfallAPI.Enum;
using RainfallAPI.Models;
using RainfallAPI.Services;
using System.Net;


namespace RainfallAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
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
        //This is used for swagger OpenAPI documentation
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(RainfallReadingResponse), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> GetRainfallReadingsAsync([FromRoute] string stationId, [FromQuery] int count = 10)
        {
            // Validate input parameters

            // Initialize a error response to store error details
            var error = new Error();

            // Validate the stationId parameter
            if (!int.TryParse(stationId, out int value))
            {
                error.Detail.Add(new ErrorDetail { PropertyName = nameof(stationId), Message = $"The {nameof(stationId)} is not valid." });
            }

            // Validate the count parameter using pattern matching
            if (count is < 1 or > 100)
                error.Detail.Add(new ErrorDetail { PropertyName = nameof(count), Message = "Must be a positive integer between 1 to 100 only." });

            // If any error details were added, return a bad request response
            if (error.Detail.Any())
            {
                error.Message = ErrorTypeExtensions.GetErrorMessage(ErrorType.InvalidRequest);
                return BadRequest(new ErrorResponse
                {
                    Error = error,
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
                // If an unexpected error occurs, add error details for logging
                error.Message = ErrorTypeExtensions.GetErrorMessage(ErrorType.InternalServerError);
                error.Detail.Add(new ErrorDetail { PropertyName = ex.Source, Message = ex.Message });

                // Return an Internal Server Error response with error details
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse
                {
                    Error = error,
                });
            }
        }
    }
}
