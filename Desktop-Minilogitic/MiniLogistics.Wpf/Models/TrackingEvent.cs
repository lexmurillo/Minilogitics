
namespace MiniLogistics.Wpf.Models;

public class TrackingEvent
{
    public int Id { get; set; }

    public int PackageId { get; set; }

    public string Status { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}

