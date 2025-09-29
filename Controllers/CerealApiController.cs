using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cereal_Api.Data;
using Cereal_Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cereal_Api.Models.DTO;
using Cereal_Api.Repositories;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/cereal")]
    public class CerealApiController : ControllerBase
    {
        private readonly ICerealRepository _repository;

        public CerealApiController(ICerealRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<CerealDTO>>> GetAllCereals()
        {
            var cereals = await _repository.GetAllAsync();
            return Ok(cereals);
        }

        // Is used to create or update rows
        // Used by passing collection objects of type CerealUpdate 
        // If an Guid is passed in the object it will attempt to update the associated object 
        // on the fields that are passed with it / ex guid and name are passed it will attempt to update name on the row with that guid 
        // if no guid is passed it will take the object passed as a new row and attempt to add it to the dataset
        [HttpPost("update")]
        public async Task<ActionResult<string>> UpdateRows([FromBody] IEnumerable<CerealUpdateDTO> items)
        {
            if (items == null || !items.Any())
                return BadRequest("No items provided.");

            var resultList = new List<CerealDTO>();

            // Split items into batches
            var toDelete = items.Where(i => i.Id.HasValue && i.Delete == true).ToList();
            var toUpdate = items.Where(i => i.Id.HasValue && i.Delete != true).ToList();
            var toCreate = items.Where(i => !i.Id.HasValue).ToList();

            // Batch delete
            if (toDelete.Any())
            {
                var deleted = await _repository.DeleteAsync(toDelete.Select(i => i.Id.Value).ToList());
            }

            // Batch update
            if (toUpdate.Any())
            {
                var updated = await _repository.UpdateAsync(toUpdate);
            }

            // Batch create
            if (toCreate.Any())
            {
                var created = await _repository.CreateAsync(toCreate);
            }

            return Ok(resultList);
        }
    }
}