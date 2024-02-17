using RainfallAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RainfallAPI.Services
{
    // Interface for weather data service
    public interface IRainfallService
    {
        // Method to get rainfall readings by station id
        Task<List<RainfallReading>> GetRainfallReadingsAsync(int stationId, int count);
    }
}