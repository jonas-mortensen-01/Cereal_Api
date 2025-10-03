using System.Linq.Expressions;
using Cereal_Api.Models;
using Cereal_Api.Models.DTO;

namespace Cereal_Api.Helpers
{
    public static class ProductHelper
    {
        public static IQueryable<ProductDTO> ApplyFilter(IQueryable<ProductDTO> query, Filter filter)
        {
            var parameter = Expression.Parameter(typeof(ProductDTO), "x");
            var property = Expression.PropertyOrField(parameter, filter.Field);

            // Convert value to property type
            var propertyType = Nullable.GetUnderlyingType(property.Type) ?? property.Type;
            var typedValue = Convert.ChangeType(filter.Value, propertyType);
            var constant = Expression.Constant(typedValue, property.Type);

            Expression comparison;

            switch (filter.Operator.ToLower())
            {
                case "=":
                case "eq":
                case "==":
                    comparison = Expression.Equal(property, constant);
                    break;
                case ">":
                    comparison = Expression.GreaterThan(property, constant);
                    break;
                case "<":
                    comparison = Expression.LessThan(property, constant);
                    break;
                case ">=":
                    comparison = Expression.GreaterThanOrEqual(property, constant);
                    break;
                case "<=":
                    comparison = Expression.LessThanOrEqual(property, constant);
                    break;
                case "contains":
                    var methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    if (methodInfo == null)
                        throw new InvalidOperationException("Could not find 'string.Contains(string)' method.");

                    comparison = Expression.Call(property, methodInfo, constant);
                    break;
                default:
                    throw new NotSupportedException($"Operator {filter.Operator} not supported");
            }

            var lambda = Expression.Lambda<Func<ProductDTO, bool>>(comparison, parameter);
            return query.Where(lambda);
        }

        public static Product? MapProductFromDTO(this ProductDTO dto)
        {
            if (dto == null) return null;

            return new Product
            {
                Id = dto.Id.GetValueOrDefault(),
                Name = dto.Name ?? "",
                MFR = dto.MFR ?? "",
                Type = dto.Type ?? "",
                Calories = dto.Calories.GetValueOrDefault(),
                Protein = dto.Protein.GetValueOrDefault(),
                Fat = dto.Fat.GetValueOrDefault(),
                Sodium = dto.Sodium.GetValueOrDefault(),
                Fiber = dto.Fiber.GetValueOrDefault(),
                Carbo = dto.Carbo.GetValueOrDefault(),
                Sugars = dto.Sugars.GetValueOrDefault(),
                Potass = dto.Potass.GetValueOrDefault(),
                Vitamins = dto.Vitamins.GetValueOrDefault(),
                Shelf = dto.Shelf.GetValueOrDefault(),
                Weight = dto.Weight.GetValueOrDefault(),
                Cups = dto.Cups.GetValueOrDefault(),
                Rating = dto.Rating.GetValueOrDefault(),
            };
        }

        public static IEnumerable<Product> MapProductsFromDTOList(IEnumerable<ProductDTO> dtoList)
        {
            List<Product> result = new List<Product>();

            foreach (ProductDTO item in dtoList)
            {
                var resultItem = MapProductFromDTO(item);
                if (resultItem != null) result.Add(resultItem);
            }

            return result;
        }

        public static ProductDTO? MapDTOForCRUD(this ProductUpdateDTO dto)
        {
            if (dto == null) return null;

            return new ProductDTO
            {
                Id = dto.Id,
                Name = dto.Name,
                MFR = dto.MFR,
                Type = dto.Type,
                Calories = dto.Calories,
                Protein = dto.Protein,
                Fat = dto.Fat,
                Sodium = dto.Sodium,
                Fiber = dto.Fiber,
                Carbo = dto.Carbo,
                Sugars = dto.Sugars,
                Potass = dto.Potass,
                Vitamins = dto.Vitamins,
                Shelf = dto.Shelf,
                Weight = dto.Weight,
                Cups = dto.Cups,
                Rating = dto.Rating,
            };
        }

        // Maps a list of ProductDTO model from a list of ProductUpdateDTO
        public static IEnumerable<ProductDTO?> MapDTOForCRUDList(this IEnumerable<ProductUpdateDTO> dtoList)
        {
            List<ProductDTO> result = new List<ProductDTO>();

            foreach (ProductUpdateDTO item in dtoList)
            {
                var resultItem = MapDTOForCRUD(item);
                if (resultItem != null) result.Add(resultItem);
            }

            return result;
        }
    }
}