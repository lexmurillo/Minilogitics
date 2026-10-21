
using System.Net.Http;
using System.Net.Http.Json;
using MiniLogistics.Wpf.Models;

namespace MiniLogistics.Wpf.Services;

public class TrackingService
{
    private readonly HttpClient _httpClient;

    public TrackingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TrackingEvent>> GetTrackingByPackageIdAsync(
        int packageId)
    {
        var events = await _httpClient.GetFromJsonAsync<List<TrackingEvent>>(
            $"tracking/{packageId}");

        return events ?? new List<TrackingEvent>();
    }
}

