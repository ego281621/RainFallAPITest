using RainfallAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RainfallAPI.Services
{
    // Interface for rainfall data service
    public interface IRainfallService
    {
        // Method to get rainfall readings by station id
        Task<List<RainfallReading>> GetRainfallReadingsAsync(string stationId, int count);
    }
}