using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cereal_Api.Models
{
    public class ProductImage
    {
        public Guid? Id { get; set; }
        public Guid? ProductReference { get; set; }
        public string ImagePath { get; set; } = string.Empty;
    }
}