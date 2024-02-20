using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Text;

public static class SwaggerGenerator
{
    public static void GenerateSwaggerJson(string outputPath)
    {
        // Set up the OpenAPI document
        var openApiDocument = new OpenApiDocument
        {
            Info = new OpenApiInfo
            {
                Title = "My APITESTTT",
                Version = "v1"
            },
            Paths = new OpenApiPaths()
        };

        // Add your API endpoints and documentation here
        // ...

        // Save the OpenAPI document to a file
        var filePath = Path.Combine(outputPath, "swagger.json");
        File.WriteAllText(filePath, openApiDocument.SerializeAsJson(OpenApiSpecVersion.OpenApi3_0), Encoding.UTF8);
    }
}