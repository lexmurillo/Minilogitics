namespace Packages.Api.Models
{
    public class Package
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal weight { get; set; }

        public string origin { get; set; }

        public string destination { get; set; }

        public DateTime createdAt { get; set; }
    }
}
