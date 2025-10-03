using System.Collections.Generic;
using System.Threading.Tasks;
using Cereal_Api.Models;
using Cereal_Api.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Cereal_Api.Repositories
{
    public interface IProductRepository
    {
        // Retrieves the entire dataset or a single row if an id is passed
        Task<IEnumerable<ProductDTO>> GetAsync(RequestContext request, Guid? id = null);

        // Retrieves the image data for a single product
        ProductImageDTO GetProductImageAsync(Guid id);

        // Creates new row based of a CerealDTO instance
        Task<OperationResult> CreateAsync(IEnumerable<ProductUpdateDTO> toCreate);

        // Updates rows based on id
        Task<OperationResult> UpdateAsync(IEnumerable<ProductUpdateDTO> toUpdate);

        // Deletes rows based on list of id's or names
        Task<OperationResult> DeleteAsync(IEnumerable<Guid> toDelete);
    }
}