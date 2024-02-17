
namespace RainfallAPI.Models
{
    // RainfallReadingResponse model to represent a response containing a list of rainfall readings
    public class RainfallReadingResponse
    {
        // Readings property to store the list of rainfall readings
        public List<RainfallReading> Readings { get; set; }
    }
}