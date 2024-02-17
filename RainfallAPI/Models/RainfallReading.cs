namespace RainfallAPI.Models
{
    // RainfallReading model to represent a single reading
    public class RainfallReading
    {
        // DateMeasured property to store the date and time of the measurement
        public DateTime DateMeasured { get; set; }

        // AmountMeasured property to store the amount of rainfall measured
        public decimal AmountMeasured { get; set; }
    }
}