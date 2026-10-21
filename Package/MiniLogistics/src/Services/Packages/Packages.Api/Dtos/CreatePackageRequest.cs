namespace Packages.Api.Dtos
{
    public class CreatePackageRequest
    {
        public string Description { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
    }
    
    
}


