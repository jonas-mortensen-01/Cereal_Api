using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cereal_Api.Models.DTO;
using Cereal_Api.Data;
using Cereal_Api.Helpers;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Cereal_Api.Models;

namespace Cereal_Api.Repositories
{
    // Contains all functionality related to communication with the database
    public class ProductRepository : IProductRepository
    {
        // The context holds the tables containing data from our database
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Gets a product by id if no id is passed it gets all products
        // Request is filters wrapped for easier understanding when creating the json to pass in the request
        // Filters work dynamically meaning they are defined by the database and what fields are available not the code
        public async Task<IEnumerable<ProductDTO>> GetAsync(RequestContext request, Guid? id = null)
        {
            if (id != null)
            {
                var result = _context.ProductTable.Where(c => c.Id == id);
                return result;
            }
            else
            {
                var query = _context.ProductTable.AsQueryable();

                foreach (var filter in request.Filters)
                {
                    query = ProductHelper.ApplyFilter(query, filter);
                }

                // Apply sort
                var sortedQuery = SortHelper.ApplySort(query, request.SortOrders);

                return await sortedQuery.ToListAsync();
            }
        }

        // Fetches an product image by id returns and empty object upon no image found
        public ProductImageDTO GetProductImageAsync(Guid id)
        {
            var result = _context.ProductImageTable.Where(c => c.ProductReference == id).FirstOrDefault();
            return result ?? new ProductImageDTO();
        }


        // Creates new row based of a ProductDTO instance
        public async Task<OperationResult> CreateAsync(IEnumerable<ProductUpdateDTO> toCreate)
        {
            // Maps updateDTO model to productDTO
            // This is required to remove fields related to creation of new products
            var mappedEntities = ProductHelper.MapDTOForCRUDList(toCreate).ToList();

            // Filters the mapped entities to only add populated objects to the database
            _context.ProductTable.AddRange(mappedEntities.Where(x => x != null)!);

            // Saves the changes in the context
            var affected = await _context.SaveChangesAsync();

            return new OperationResult
            {
                Success = affected > 0,
                AffectedRows = affected,
                Message = affected > 0 ? "Created successfully" : "No rows created"
            };
        }

        // Updates multiple rows based on id's
        public async Task<OperationResult> UpdateAsync(IEnumerable<ProductUpdateDTO> toUpdate)
        {
            foreach (var dto in toUpdate)
            {
                // Skip each item with no id passed
                if (dto.Id == null) continue;

                // Find existing item to update
                var existing = await _context.ProductTable.FindAsync(dto.Id.Value);
                if (existing != null)
                {
                    // Update only fields passed in the toUpdate object
                    // This will prevent fields with no new values from being set to an empty value if none is passed
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

            // Saves the changes in the context
            var affected = await _context.SaveChangesAsync();

            return new OperationResult
            {
                Success = affected > 0,
                AffectedRows = affected,
                Message = affected > 0 ? "Created successfully" : "No rows created"
            };
        }

        // Deletes rows based on list of id's
        public async Task<OperationResult> DeleteAsync(IEnumerable<Guid> toDelete)
        {
            // Filters away all items to delete where the id isn't set
            var items = _context.ProductTable
                .Where(c => c.Id.HasValue && toDelete.Contains(c.Id.Value))
                .ToList();

            // Removes the found items from the database context
            _context.ProductTable.RemoveRange(items);

            // Saves the changes in the context
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