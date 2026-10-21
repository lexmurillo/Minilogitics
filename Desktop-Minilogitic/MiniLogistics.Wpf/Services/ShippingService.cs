using System;
using MiniLogistics.Wpf.Models;
using System.Net.Http;
using System.Net.Http.Json;


namespace MiniLogistics.Wpf.Services;

public class ShippingService
{
    private readonly HttpClient _httpClient;

    public ShippingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Consultar todos los envíos
    public async Task<List<Shipment>> GetShipmentsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Shipment>>("shipments")
               ?? new List<Shipment>();
    }

    // Consultar un envío por su ID
    public async Task<Shipment?> GetShipmentByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"shipments/{id}");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<Shipment>();
    }

    // Crear un envío
    public async Task<Shipment?> CreateShipmentAsync(Shipment shipment)
    {
        var response = await _httpClient.PostAsJsonAsync("shipments", shipment);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Shipment>();
    }

    // Actualizar el estado de un envío
    public async Task<Shipment?> UpdateShipmentStatusAsync(
        int id, 
        string status,
        string location)
    {
        var response = await _httpClient.PutAsJsonAsync(
            $"shipments/{id}/status",
            new { Status = status, Location = location });

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Shipment>();
    }

    // Eliminar un envío
    public async Task<bool> DeleteShipmentAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"shipments/{id}");

        return response.IsSuccessStatusCode;
    }
}
