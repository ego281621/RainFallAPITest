# Rainfall API

The Rainfall API is a RESTful web service that provides rainfall reading data for various rainfall stations.

## Description

The Rainfall API allows users to retrieve a list of rainfall readings for a specific station ID. The API follows a Clean Architecture approach and provides error handling, validation, and documentation using Swagger.

## Dependencies

- .NET 7.0 SDK
- Microsoft.Extensions.Logging.Abstractions
- Moq (for unit testing)
- NUnit (for unit testing)

## How to run the project

1. Clone the repository or download the project files.
2. Open the project in Visual Studio or Visual Studio Code.
3. Restore the NuGet packages by running the following command in the terminal:
   - dotnet restore
4. Build the project by running the following command:
   - dotnet build
5. Run the project using the following command:
   - dotnet run
   - The API will be hosted at `https://localhost:3001` (or any other available port if 3001 is already in use).
6. To access the Swagger UI and interact with the API, open a web browser and navigate to `https://localhost:3001/swagger`.
   
## Testing

The project includes unit tests for the `RainfallController` using NUnit and Moq. To run the tests, execute the following command in the terminal:
- dotnet test

This will run the unit tests and display the test results in the terminal.

## API Endpoints

The Rainfall API provides the following endpoint:

### GET /rainfall/id/stations/{stationId}/readings

Retrieves a list of rainfall readings for a given station ID.

**Parameters:**

- `stationId` (required, integer): The ID of the rainfall station.
- `count` (optional, integer, mininmum=100, default=10, max=100): The number of rainfall readings to retrieve.

**Responses:**

- `200 OK`: Successful response with a `RainfallReadingResponse` object containing the list of rainfall readings.
- `204 No Content`: No rainfall readings found for the given station ID.
- `400 Bad Request`: Invalid input parameters (invalid station ID or count).
- `500 Internal Server Error`: An error occurred while processing the request.

For more details on the API responses and data models, refer to the Swagger documentation at `https://localhost:5001/swagger`.
