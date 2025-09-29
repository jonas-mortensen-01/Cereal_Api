using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cereal_Api.Models.DTO;
using Cereal_Api.Data;
using Cereal_Api.Helpers;
using System.Linq;

namespace Cereal_Api.Repositories
{
    public class CerealRepository : ICerealRepository
    {
        private readonly ApplicationDbContext _context;

        public CerealRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CerealDTO>> GetAllAsync()
        {
            var result = await _context.CerealTable.ToListAsync<CerealDTO>();
            return result;
        }

        // Creates new row based of a CerealDTO instance
        public async Task<OperationResult> CreateAsync(IEnumerable<CerealUpdateDTO> toCreate)
        {
            var mappedEntities = CerealMapper.MapDTOForCRUDList(toCreate).ToList();

            _context.CerealTable.AddRange(mappedEntities.Where(x => x != null)!);
            var affected = await _context.SaveChangesAsync();

            return new OperationResult
            {
                Success = affected > 0,
                AffectedRows = affected,
                Message = affected > 0 ? "Created successfully" : "No rows created"
            };
        }

        // Updates rows based on id
        public async Task<OperationResult> UpdateAsync(IEnumerable<CerealUpdateDTO> toUpdate)
        {
            foreach (var dto in toUpdate)
            {
                if (dto.Id == null) continue; // skip if no Id

                var existing = await _context.CerealTable.FindAsync(dto.Id.Value);
                if (existing != null)
                {
                    // Map only non-null values from DTO
                    existing.Name = dto.Name ?? existing.Name;
                    existing.MFR = dto.MFR ?? existing.MFR;
                    existing.Type = dto.Type ?? existing.Type;
                    existing.Calories = dto.Calories ?? existing.Calories;
                    existing.Protein = dto.Protein ?? existing.Protein;
                    existing.Fat = dto.Fat ?? existing.Fat;
                    existing.Sodium = dto.Sodium ?? existing.Sodium;
                    existing.Fiber = dto.Fiber ?? existing.Fiber;
                    existing.Carbo = dto.Carbo ?? existing.Carbo;
                    existing.Sugars = dto.Sugars ?? existing.Sugars;
                    existing.Potass = dto.Potass ?? existing.Potass;
                    existing.Vitamins = dto.Vitamins ?? existing.Vitamins;
                    existing.Shelf = dto.Shelf ?? existing.Shelf;
                    existing.Weight = dto.Weight ?? existing.Weight;
                    existing.Cups = dto.Cups ?? existing.Cups;
                    existing.Rating = dto.Rating ?? existing.Rating;
                }
            }

            var affected = await _context.SaveChangesAsync();

            return new OperationResult
            {
                Success = affected > 0,
                AffectedRows = affected,
                Message = affected > 0 ? "Created successfully" : "No rows created"
            };
        }

        // Deletes rows based on list of id's or names
        public async Task<OperationResult> DeleteAsync(IEnumerable<Guid> toDelete)
        {
            var items = _context.CerealTable
                .Where(c => c.Id.HasValue && toDelete.Contains(c.Id.Value))
                .ToList();

            _context.CerealTable.RemoveRange(items);
            await _context.SaveChangesAsync();

            var affected = await _context.SaveChangesAsync();

            return new OperationResult
            {
                Success = affected > 0,
                AffectedRows = affected,
                Message = affected > 0 ? "Created successfully" : "No rows created"
            };
        }
    }
}