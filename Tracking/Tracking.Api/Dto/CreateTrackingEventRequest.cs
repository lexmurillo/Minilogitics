namespace Tracking.Api.Dto
{
    public class CreateTrackingEventRequest
    {
        public int PackageId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}
