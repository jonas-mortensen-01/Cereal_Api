using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cereal_Api.Models.DTO;
using Cereal_Api.Data;
using Cereal_Api.Helpers.CerealMapper

using Cereal_Api.Helpers;

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
            return await _context.CerealTable.ToListAsync<CerealDTO>();
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
            return null;
        }

        // Deletes rows based on list of id's or names
        public async Task<OperationResult> DeleteAsync(IEnumerable<Guid> toDelete)
        {
            return null;
        }
    }
}