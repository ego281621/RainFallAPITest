namespace RainfallAPI.Models
{
    // Error response model to represent API errors
    public class Error
    {

        // Message property for high-level error message
        public string Message { get; set; }

        // Details property to hold list of error details
        public List<ErrorDetail> Detail { get; set; } = new List<ErrorDetail>();

    }

    // Error detail model to represent details of an API error
    public class ErrorDetail
    {

        // Name of the property that caused the error
        public string PropertyName { get; set; }

        // Detailed error message for property
        public string Message { get; set; }

    }

}

