using System.Linq.Expressions;
using Cereal_Api.Models;
using Cereal_Api.Models.DTO;

namespace Cereal_Api.Helpers
{
    public static class ImageHelper
    {
        public static ProductImage? MapImageFromDTO(this ProductImageDTO dto)
        {
            if (dto == null) return null;

            return new ProductImage
            {
                Id = dto.Id.GetValueOrDefault(),
                ProductReference = dto.ProductReference.GetValueOrDefault(),
                ImagePath = dto.ImagePath ?? ""
            };
        }
    }
}