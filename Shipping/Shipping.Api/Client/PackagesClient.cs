using System.Net.Http.Json;
namespace Shipping.Api.Client
{
    public class PackagesClient
    {
        private readonly HttpClient _httpClient;

        public PackagesClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> PackageExistsAsync(int packageId)
        {
            var response = await _httpClient.GetAsync($"packages/{packageId}");

            return response.IsSuccessStatusCode;
        }
    }
}
