using System;
using System.Collections.Generic;
using System.Text;

namespace MiniLogistics.Wpf.Models
{
    public class Package
    {
        public int Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public decimal Weight { get; set; }

        public string Origin { get; set; } = string.Empty;

        public string Destination { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
