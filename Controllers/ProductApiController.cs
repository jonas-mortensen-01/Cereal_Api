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
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace MyApi.Controllers
{
    [ApiController]

    [Route("api/product")]
    public class ProductApiController : ControllerBase
    {
        // Initializes the product repository for access to relevant functions
        private readonly IProductRepository _repository;

        // Basic api controller
        // These fetch their relevant data in the form of ProductDTO and ProductImageDTO models
        // then maps them to models like Product or ProductImage
        // This is done to seperate the data needed for serving to a frontend application
        // from the data needed to communicate with a database.
        // It will also allow easier configuration of the fields and mapping for data sent to the frontend
        // Ex a lightweight model or a Product that does not initially contain an image but an image reference
        // where we then map that image to a Product

        // Additionally we use a seperate ProductUpdateDTO to further seperate models
        // this is to allow some later mapping of the products to add additional data 
        // not otherwise needed initially for the frontend
        public ProductApiController(IProductRepository repository)
        {
            _repository = repository;
        }

        // Get endpoint has been set to post for the reasons
        // body json parameters can't be used on get requests
        // post requests allow this and in turn lets us make dynamic filtering using the function displayed below
        [EndpointSummary("Get Product")]
        [EndpointDescription(@"Retrieves one or multiple products<br><br>
            Parameters:<br><br>
            - id: The id of the product to fetch<br>
            If none is specified it will fetch all products instead<br><br>
            - request: Holds the filters and sort orders<br>
            for the request multiple of each may be passed<br>
            It takes an object containing a list of filters like this<br>
            {<br>
                ""filters"": [<br>
                    { ""field"": ""Sodium"", ""operator"": "">"", ""value"": ""250"" }<br>
                ],<br>
                ""sortOrders"": <br>
                [<br>
                    { ""field"": ""Sodium"", ""direction"": ""asc"" },<br>
                    { ""field"": ""Calories"", ""direction"": ""asc"" }<br>
                ]<br>
            }
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

                var deserializedRequest = new RequestContext();
                if (request != null)
                    deserializedRequest = JsonSerializer.Deserialize<RequestContext>(request, options)
                                         ?? new RequestContext();

                var products = await _repository.GetAsync(deserializedRequest, id);
                var mappedProducts = ProductHelper.MapProductsFromDTOList(products);

                return Ok(mappedProducts);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching products.", ex);
            }
        }

        // Gets an product image by product id
        // Gets an ProductImageDTO to map to a ProductImage to return
        [EndpointSummary("Get Product Image")]
        [EndpointDescription(@"Retrieves the image for a product<br><br>
            Parameters:<br><br>
            - id: The id of the product that the image is attached to
            ")]
        [ProducesResponseType<ProductImage>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("get/image")]
        public ActionResult<ProductImage> GetProductImageAsync(Guid id)
        {
            try
            {
                // Retrieves an image DTO model
                var image = _repository.GetProductImageAsync(id);

                // Maps the retrieved image to a full model to serve for the frontend
                var mappedImage = ImageHelper.MapImageFromDTO(image);

                return Ok(mappedImage);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching product image.", ex);
            }
        }

        // Is used to create or update rows
        // Used by passing collection objects of type ProductUpdate 
        // If an Guid is passed in the object it will attempt to update the associated object 
        // on the fields that are passed with it / ex guid and name are passed it will attempt to update name on the row with that guid 
        // if no guid is passed it will take the object passed as a new row and attempt to add it to the dataset
        [EndpointSummary("Update Product")]
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
                // Runs the update function on all items passed in the parameter
                await _repository.UpdateAsync(items);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating products.", ex);
            }
        }

        // Creates one or more products from a list of ProductUpdateDTO objects
        [EndpointSummary("Create Product")]
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
                // Runs the create function on all items passed in the parameter
                await _repository.CreateAsync(items);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating products.", ex);
            }
        }

        // Deletes one or more products based of a list of guids
        [EndpointSummary("Delete Product")]
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
                // Runs the delete function on all non null ids passed in the parameter
                await _repository.DeleteAsync(
                    ids.Select(i => i).Where(v => v != Guid.Empty).ToList());
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting products.", ex);
            }
        }
    }
}