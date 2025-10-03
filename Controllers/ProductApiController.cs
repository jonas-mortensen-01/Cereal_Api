using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cereal_Api.Data;
using Cereal_Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cereal_Api.Models.DTO;
using Cereal_Api.Repositories;
using System.Text.Json;
using Cereal_Api.Helpers;

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
        [EndpointSummary("Gets products")]
        [EndpointDescription(@"Retrieves one or multiple products<br><br>
            Parameters:<br><br>
            - id: The id of the product to fetch<br>
            If none is specified it will fetch all products instead<br><br>
            - request: Holds the filters for the request<br>
            It takes an object containing a list of filters like this<br>
            the object should be encrypted as a json string and passed in the query<br>
            {'filters': [{ 'field':'Sodium', 'operator':'==', 'value':'140' }]}
            ")]
        [ProducesResponseType<IEnumerable<Product>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] string? request, Guid? id = null)
        {
            try
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

                var mappedProducts = ProductHelper.MapProductsFromDTOList(products);

                return Ok(mappedProducts);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching products.", ex);
            }
        }



        // Is used to create or update rows
        // Used by passing collection objects of type ProductUpdate 
        // If an Guid is passed in the object it will attempt to update the associated object 
        // on the fields that are passed with it / ex guid and name are passed it will attempt to update name on the row with that guid 
        // if no guid is passed it will take the object passed as a new row and attempt to add it to the dataset
        [EndpointSummary("Updates products")]
        [EndpointDescription(@"Updates one or multiple products<br><br>
            Parameters:<br><br>
            - items: A list of mapped products<br><br>
            Is used for updating products in the database with new data
            the items parameter contains the id field for a related product for each item
            this will be the product it pushes the update to
            this is added in the body of the request
            ")]
        [ProducesResponseType<IEnumerable<Product>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("update")]
        public async Task<ActionResult> UpdateRows([FromBody] IEnumerable<ProductUpdateDTO> items)
        {
            try
            {
                await _repository.UpdateAsync(items);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching products.", ex);
            }
        }

        [EndpointSummary("Creates products")]
        [EndpointDescription(@"Creates one or more products<br><br>
            Parameters:<br><br>
            - items: A list of mapped products<br><br>
            Is used to enter new products into the database
            this is added in the body of the request
            ")]
        [ProducesResponseType<IEnumerable<Product>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("create")]
        public async Task<ActionResult> CreateRows([FromBody] IEnumerable<ProductUpdateDTO> items)
        {
            try
            {
                await _repository.CreateAsync(items);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching products.", ex);
            }
        }

        [EndpointSummary("Deletes products")]
        [EndpointDescription(@"Deletes one or more products<br><br>
            Parameters:<br><br>
            - ids: A list of ids<br><br>
            Is used to delete one or more products
            ")]
        [ProducesResponseType<IEnumerable<Product>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteRows([FromBody] IEnumerable<Guid> ids)
        {
            try
            {
                await _repository.DeleteAsync(
                    ids.Select(i => i).Where(v => v != Guid.Empty).ToList());
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching products.", ex);
            }
        }

        // [HttpPost("update")]
        // public async Task<ActionResult<string>> UpdateRows([FromBody] IEnumerable<ProductUpdateDTO> items)
        // {
        //     if (items == null || !items.Any())
        //         return BadRequest("No items provided.");

        //     var resultList = new List<ProductDTO>();

        //     // Split items into batches
        //     var toDelete = items.Where(i => i.Id.HasValue && i.Delete == true).ToList();
        //     var toUpdate = items.Where(i => i.Id.HasValue && i.Delete != true).ToList();
        //     var toCreate = items.Where(i => !i.Id.HasValue).ToList();

        //     // Batch delete
        //     if (toDelete.Count > 0)
        //     {
        //         var deleted = await _repository.DeleteAsync(
        //             toDelete.Select(i => i.Id.GetValueOrDefault()).Where(v => v != Guid.Empty).ToList());
        //     }

        //     // Batch update
        //     if (toUpdate.Count > 0)
        //     {
        //         var updated = await _repository.UpdateAsync(toUpdate);
        //     }

        //     // Batch create
        //     if (toCreate.Count > 0)
        //     {
        //         var created = await _repository.CreateAsync(toCreate);
        //     }

        //     return Ok(resultList);
        // }
    }
}