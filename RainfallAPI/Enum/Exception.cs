namespace RainfallAPI.Enum
{
    // Enum to represent error types
    public enum ErrorType
    {
        InvalidRequest,
        InternalServerError
    }

    // Extension method for ErrorType enum to get error message
    public static class ErrorTypeExtensions
    {
        public static string GetErrorMessage(this ErrorType errorType)
        {
            switch (errorType)
            {
                case ErrorType.InvalidRequest:
                    return "Invalid Request.";
                case ErrorType.InternalServerError:
                default:
                    return "Internal Server Error.";
            }
        }
    }
}

