using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cereal_Api.Models
{
    public class Product
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MFR { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Calories { get; set; }
        public int Protein { get; set; }
        public int Fat { get; set; }
        public int Sodium { get; set; }
        public double Fiber { get; set; }
        public double Carbo { get; set; }
        public int Sugars { get; set; }
        public int Potass { get; set; }
        public int Vitamins { get; set; }
        public int Shelf { get; set; }
        public double Weight { get; set; }
        public double Cups { get; set; }
        public double Rating { get; set; }
    }
}