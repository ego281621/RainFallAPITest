﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RainfallAPI.Models;
using RainfallAPI.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RainfallAPI.Services
{
    // Implementation of rainfall data service
    public class RainfallService : IRainfallService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private const string baseUrlKeyValue = "ApiSettings:BaseUrlKeyValue";

        public RainfallService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        // Method implementation to get rainfall readings
        public async Task<List<RainfallReading>> GetRainfallReadingsAsync(int stationId, int count)
        {
            // Get the base URL from appsettings.json using IConfiguration
            string baseUrl = _configuration.GetValue<string>(baseUrlKeyValue);

            // Construct the URL for the API endpoint
            string url = $"{baseUrl}/flood-monitoring/id/stations/{stationId}/readings?_sorted&_limit={count}";

            // Call external API 
            var response = await _httpClient.GetAsync(url);

            // Check if the response is successful
            response.EnsureSuccessStatusCode();

            // Read response content
            string json = await response.Content.ReadAsStringAsync();

            // Parse response JSON and map to models
            return ParseReadings(json);
        }

        // Method to parse JSON response and map to RainfallReading models
        public static List<RainfallReading> ParseReadings(string json)
        {
            List<RainfallReading> readings = new List<RainfallReading>();

            // Parse JSON string to JObject
            JObject obj = JObject.Parse(json);

            // Extract items array from JSON object
            JArray items = (JArray)obj["items"];

            // Loop through each item in the items array
            foreach (JObject item in items)
            {
                // Extract dateTime and value properties from item
                string dateTimeStr = (string)item["dateTime"];
                decimal value = (decimal)item["value"];

                // Parse dateTime string to DateTime object using CultureInfo.InvariantCulture
                DateTime dateTime = DateTime.Parse(dateTimeStr, CultureInfo.InvariantCulture);

                // Create new RainfallReading object and add to readings list
                readings.Add(new RainfallReading
                {
                    DateMeasured = dateTime,
                    AmountMeasured = value
                });
            }

            return readings;
        }
    }
}