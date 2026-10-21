using System;
using System.Collections.Generic;
using System.Text;

namespace MiniLogistics.Wpf.Models
{
    public class Shipment
    {
        public int Id { get; set; }

        public int PackageId { get; set; }

        public string Status { get; set; } = "Created";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
