using FacilityServiceAPI.Models;
using FacilityServiceAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FacilityServiceAPI.Controllers
{
    [ApiController]
    [Route("swimmingpool")]
    public class SwimmingPoolController : ControllerBase
    {
        private readonly ISwimmingPoolRepository _swimmingPoolRepository;
        private readonly ILogger<SwimmingPoolController> _logger;

        public SwimmingPoolController(ILogger<SwimmingPoolController> logger , ISwimmingPoolRepository facilityRepository)
        {
            _logger = logger;
            _swimmingPoolRepository = facilityRepository;
        }

        /// <summary>
        /// Gets all facilities of type  Swimming pool
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("getall")]
		public async Task<IActionResult> GetSwimmingPools()
		{
			_logger.LogDebug("Starting getswimmingpools");

			try
			{
				return Ok(await _swimmingPoolRepository.GetAllSwimmingPools());
			}
			catch (Exception ex)
			{
                _logger.LogError("Error in getswimmingpools: {message}", ex.Message);
				return BadRequest(ex.Message);
			}
		}

        /// <summary>
        /// controller for getting a single Swimming pool facility based on given id 
        /// </summary>
        /// <param name="swimmingPoolId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("getbyid")]
		public async Task<IActionResult> GetSwimmingPool([FromBody] string swimmingPoolId)
		{
			_logger.LogDebug("Starting getswimmingpoolbyid for Swimming Pool {swimmingPoolId}", swimmingPoolId);
			try
			{
				var result = await _swimmingPoolRepository.GetAllSwimmingPools();
				return Ok(result.Single(sp => sp.Id == swimmingPoolId));
			}
			catch (Exception ex)
            {
				_logger.LogError("Error in getswimmingpoolbyid for {swimmingPoolId}: {message}", swimmingPoolId, ex.Message);
                return BadRequest(ex.Message);
			}
		}

        /// <summary>
        /// Inserts a single swimmingpool
        /// </summary>
        /// <param name="swimmingPool"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("insert")]
        public async Task<IActionResult> Insertswimmingpool([FromBody] SwimmingPool swimmingPool)
        {
            _logger.LogDebug("Starting insertswimmingppol for swimmingpool {name}", swimmingPool.Name);

            try
            {
                await _swimmingPoolRepository.InsertSwimmingpool(swimmingPool);
                _logger.LogInformation("SwimmingPool {name} inserted successfully", swimmingPool.Name);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error inserting swimmingpool {name}: {message}", swimmingPool.Name, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates a single facility
        /// </summary>
        /// <param name="swimmingpool"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Updateswimmingpool([FromBody] SwimmingPool swimmingPool)
        {
            _logger.LogDebug("starting updateswimmingpool for swimmingpool {id}", swimmingPool.Id);

            try
            {
                await _swimmingPoolRepository.UpdateSwimmingPool(swimmingPool);
                _logger.LogInformation("SwimmingPool {id} updated successfully", swimmingPool.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating swimmingpool {id}: {message}", swimmingPool.Id, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
