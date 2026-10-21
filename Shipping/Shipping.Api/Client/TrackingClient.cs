
using System.Net.Http.Json;

namespace Shipping.Api.Client;

public class TrackingClient
{
    private readonly HttpClient _httpClient;

    public TrackingClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(bool Success, string Message)> CreateTrackingEventAsync(
        int packageId,
        string status,
        string location)
    {
        var request = new
        {
            PackageId = packageId,
            Status = status,
            Location = location
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                "tracking",
                request);

            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return (
                    false,
                    $"Tracking respondió {(int)response.StatusCode} " +
                    $"{response.StatusCode}: {responseBody}");
            }

            return (true, "Evento de seguimiento registrado.");
        }
        catch (HttpRequestException ex)
        {
            return (false, $"No se pudo conectar con Tracking: {ex.Message}");
        }
    }
}


