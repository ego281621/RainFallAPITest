using RainfallAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Add MVC controllers to the services container.
builder.Services.AddEndpointsApiExplorer(); // Add API explorer services.
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Rainfall Api",
        Description = "An API which provides rainfall reading data ",
        Contact = new OpenApiContact
        {
            Name = "Sorted",
            Url = new Uri("https://www.sorted.com")
        },
    });
}); // Add Swagger services for API documentation.

builder.Services.AddScoped<IRainfallService, RainfallService>(); // Register the RainfallService for dependency injection.
builder.Services.AddHttpClient(); // Add HttpClient services for making HTTP requests.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger middleware for generating Swagger JSON
   // app.UseSwaggerUI(); // Enable Swagger UI middleware for interactive API documentation
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rainfall API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection(); // Enable HTTPS redirection middleware.

app.UseAuthorization(); // Enable authorization middleware.

app.MapControllers(); // Map controllers to endpoints.

app.UseRouting(); // Enable routing middleware.

app.Run(); // Run the application.
