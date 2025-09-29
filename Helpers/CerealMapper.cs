using Cereal_Api.Models;
using Cereal_Api.Models.DTO;

namespace Cereal_Api.Helpers
{
    public static class CerealMapper
    {
        public static Cereal? MapCerealFromDTO(this CerealDTO dto)
        {
            if (dto == null) return null;

            return new Cereal
            {
                Id = dto.Id.Value,
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

        public static CerealDTO? MapDTOForCRUD(this CerealUpdateDTO dto)
        {
            if (dto == null) return null;

            return new CerealDTO
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

        public static IEnumerable<CerealDTO?> MapDTOForCRUDList(this IEnumerable<CerealUpdateDTO> dtoList)
        {
            List<CerealDTO> result = new List<CerealDTO>();

            foreach (CerealUpdateDTO item in dtoList)
            {
                var resultItem = MapDTOForCRUD(item);
                if (resultItem != null) result.Add(resultItem);
            }

            return result;
        }
    }
}