using System;

namespace Shipping.Api.Models;

public class Shipment
{
    public int Id { get; set; }
    public int PackageId { get; set; }
    public string Status { get; set; } = "Created";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}







