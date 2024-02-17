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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rainfall API", Version = "v1" });
}); // Add Swagger services for API documentation.

builder.Services.AddScoped<IRainfallService, RainfallService>(); // Register the RainfallService for dependency injection.
builder.Services.AddHttpClient(); // Add HttpClient services for making HTTP requests.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger middleware for generating Swagger JSON
    app.UseSwaggerUI(); // Enable Swagger UI middleware for interactive API documentation
}

app.UseHttpsRedirection(); // Enable HTTPS redirection middleware.

app.UseAuthorization(); // Enable authorization middleware.

app.MapControllers(); // Map controllers to endpoints.

app.UseRouting(); // Enable routing middleware.

app.Run(); // Run the application.
