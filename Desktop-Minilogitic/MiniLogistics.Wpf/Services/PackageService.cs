using System;
using MiniLogistics.Wpf.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace MiniLogistics.Wpf.Services;

public class PackageService
{
    private readonly HttpClient _httpClient;

    public PackageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // GET: obtener todos los paquetes
    public async Task<List<Package>> GetPackagesAsync()
    {
        return await _httpClient
            .GetFromJsonAsync<List<Package>>("packages")
            ?? [];
    }

    // GET: obtener un paquete por ID
    public async Task<Package?> GetPackageByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"packages/{id}");

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Package>();
    }

    // POST: crear un paquete
    public async Task CreatePackageAsync(Package package)
    {
        var response = await _httpClient.PostAsJsonAsync("packages", package);

        response.EnsureSuccessStatusCode();
    }

    // DELETE: eliminar un paquete
    public async Task DeletePackageAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"packages/{id}");

        response.EnsureSuccessStatusCode();
    }
}
