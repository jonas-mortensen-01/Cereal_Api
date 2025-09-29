using System.Collections.Generic;
using System.Threading.Tasks;
using Cereal_Api.Models.DTO;

namespace Cereal_Api.Repositories
{
    public interface ICerealRepository
    {
        // Retrieves the entire dataset
        Task<IEnumerable<CerealDTO>> GetAllAsync();

        // Creates new row based of a CerealDTO instance
        Task<OperationResult> CreateAsync(IEnumerable<CerealUpdateDTO> toCreate);

        // Updates rows based on id
        Task<OperationResult> UpdateAsync(IEnumerable<CerealUpdateDTO> toUpdate);

        // Deletes rows based on list of id's or names
        Task<OperationResult> DeleteAsync(IEnumerable<Guid> toDelete);
    }
}