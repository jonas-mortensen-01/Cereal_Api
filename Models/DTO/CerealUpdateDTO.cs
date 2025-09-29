using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cereal_Api.Models.DTO {
    // Seperate model used for updating rows in dataset
    // Needed if the updates should contain less or more fields based on the requirements for the creation or update of the row    
    public class CerealUpdateDTO
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? MFR { get; set; }
        public string? Type { get; set; }
        public int? Calories { get; set; }
        public int? Protein { get; set; }
        public int? Fat { get; set; }
        public int? Sodium { get; set; }
        public double? Fiber { get; set; }
        public double? Carbo { get; set; }
        public int? Sugars { get; set; }
        public int? Potass { get; set; }
        public int? Vitamins { get; set; }
        public int? Shelf { get; set; }
        public double? Weight { get; set; }
        public double? Cups { get; set; }
        public double? Rating { get; set; }
        public bool? Delete { get; set; }
    }
}