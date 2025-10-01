using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cereal_Api.Data;
using Cereal_Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cereal_Api.Models.DTO;
using Cereal_Api.Repositories;
using System.Text.Json;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductApiController(IProductRepository repository)
        {
            _repository = repository;
        }

        // Get endpoint has been set to post for the reasons
        // body json parameters can't be used on get requests
        // post requests allow this and in turn lets us make dynamic filtering using the function displayed below
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts([FromQuery] string? request, Guid? id = null)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var deserializedRequest = new FilterRequest();
            if (request != null)
                deserializedRequest = JsonSerializer.Deserialize<FilterRequest>(request, options)
                                     ?? new FilterRequest();

            var products = await _repository.GetAsync(deserializedRequest, id);
            return Ok(products);
        }

        // Is used to create or update rows
        // Used by passing collection objects of type ProductUpdate 
        // If an Guid is passed in the object it will attempt to update the associated object 
        // on the fields that are passed with it / ex guid and name are passed it will attempt to update name on the row with that guid 
        // if no guid is passed it will take the object passed as a new row and attempt to add it to the dataset
        [HttpPost("update")]
        public async Task<ActionResult<string>> UpdateRows([FromBody] IEnumerable<ProductUpdateDTO> items)
        {
            if (items == null || !items.Any())
                return BadRequest("No items provided.");

            var resultList = new List<ProductDTO>();

            // Split items into batches
            var toDelete = items.Where(i => i.Id.HasValue && i.Delete == true).ToList();
            var toUpdate = items.Where(i => i.Id.HasValue && i.Delete != true).ToList();
            var toCreate = items.Where(i => !i.Id.HasValue).ToList();

            // Batch delete
            if (toDelete.Count > 0)
            {
                var deleted = await _repository.DeleteAsync(
                    toDelete.Select(i => i.Id.GetValueOrDefault()).Where(v => v != Guid.Empty).ToList());
            }

            // Batch update
            if (toUpdate.Count > 0)
            {
                var updated = await _repository.UpdateAsync(toUpdate);
            }

            // Batch create
            if (toCreate.Count > 0)
            {
                var created = await _repository.CreateAsync(toCreate);
            }

            return Ok(resultList);
        }
    }
}