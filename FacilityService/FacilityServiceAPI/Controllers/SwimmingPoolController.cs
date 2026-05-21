using FacilityServiceAPI.Models;
using FacilityServiceAPI.Repositories.Interfaces;
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

				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// controller for getting a single Swimming pool facility based on given id 
		/// </summary>
		/// <param name="swimmingPoolId"></param>
		/// <returns></returns>
		[HttpGet("getbyid")]
		public async Task<IActionResult> GetSwimmingPool([FromBody] string swimmingPoolId)
		{
			_logger.LogDebug("Starting getswimmingpoolbyid for Swimming Pool" + swimmingPoolId);
			try
			{
				var result = await _swimmingPoolRepository.GetAllSwimmingPools();
				return Ok(result.Single(sp => sp.Id == swimmingPoolId));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        /// <summary>
        /// Inserts a single swimmingpool
        /// </summary>
        /// <param name="swimmingPool"></param>
        /// <returns></returns>
        [HttpPost("insert")]
        public async Task<IActionResult> Insertswimmingpool([FromBody] SwimmingPool swimmingPool)
        {
            _logger.LogDebug("Starting insertswimmingppol for swimmingpool" + swimmingPool.Name);

            try
            {
                await _swimmingPoolRepository.InsertSwimmingpool(swimmingPool);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates a single facility
        /// </summary>
        /// <param name="swimmingpool"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> Updateswimmingpool([FromBody] SwimmingPool swimmingPool)
        {
            _logger.LogDebug("starting updateswimmingpool for facility" + swimmingPool.Id);

            try
            {
                await _swimmingPoolRepository.UpdateSwimmingPool(swimmingPool);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
