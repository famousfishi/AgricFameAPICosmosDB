using AgricFameAPICosmosDB.Models;
using AgricFameAPICosmosDB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AgricFameAPICosmosDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmDataController : ControllerBase
    {
        private readonly IFarmCosmosDbService _farmCosmosDbService;

        public FarmDataController(IFarmCosmosDbService farmCosmosDbService)
        {
            _farmCosmosDbService = farmCosmosDbService;
        }

        // GET: api/<FarmDataController>
        [HttpGet]
        public async Task<IActionResult> ListOfFarmData()
        {
            return Ok(await _farmCosmosDbService.GetFarmsAsync("select * from AgricDataDatabase"));
        }

        // GET api/<FarmDataController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFarm(string id)
        {
            return Ok(await _farmCosmosDbService.GetFarmAsync(id));
        }

        // POST api/<FarmDataController>
        [HttpPost]
        public async Task<IActionResult> CreateFarmData([FromBody] Farm farm)
        {
            if (farm == null)
            {
                return BadRequest("Payload doesnt correspond, Check your payload");
            }

            farm.Id = Guid.NewGuid().ToString();
            await _farmCosmosDbService.AddFarmAsync(farm);
            return CreatedAtAction(nameof(GetFarm), new { id = farm.Id }, farm);
        }

        // PUT api/<FarmDataController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFarmData(string id, [FromBody] Farm farm)
        {
            if (id != farm.Id)
            {
                return NotFound();
            }

            await _farmCosmosDbService.UpdateFarmAsync(farm.Id, farm);
            return NoContent();
        }

        // DELETE api/<FarmDataController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFarmData(string id)
        {
            await _farmCosmosDbService.DeleteAsync(id);
            return NoContent();
        }
    }
}