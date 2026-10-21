namespace Shipping.Api.DTO
{
    public class UpdateShipmentStatusRequest
    {
        public string Status {get; set;} = string.Empty;

        public string Location { get; set; } = string.Empty;
    }
}
