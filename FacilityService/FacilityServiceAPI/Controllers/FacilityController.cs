using FacilityServiceAPI.Models;
using FacilityServiceAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FacilityServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacilityController : ControllerBase
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly ILogger<FacilityController> _logger;

        public FacilityController(ILogger<FacilityController> logger , IFacilityRepository facilityRepository)
        {
            _logger = logger;
            _facilityRepository = facilityRepository;
        }

		

		/// <summary>
		/// Gets all facilities of type  Swimming pool
		/// </summary>
		/// <returns></returns>
		[HttpGet("getswimmingpools")]
		public async Task<IActionResult> GetSwimmingPools()
		{
			_logger.LogDebug("Starting getswimmingpools");

			try
			{
				return Ok(await _facilityRepository.GetAllSwimmingPools());
			}
			catch (Exception ex)
			{

				return BadRequest(ex);
			}
		}

		/// <summary>
		/// controller for getting a single Swimming pool facility based on given id 
		/// </summary>
		/// <param name="swimmingPoolId"></param>
		/// <returns></returns>
		[HttpGet("getswimmingpoolbyid")]
		public async Task<IActionResult> GetSwimmingPool([FromBody] string swimmingPoolId)
		{
			_logger.LogDebug("Starting getswimmingpoolbyid for Swimming Pool" + swimmingPoolId);
			try
			{
				var result = await _facilityRepository.GetAllSwimmingPools();
				return Ok(result.Single(sp => sp.Id == swimmingPoolId));
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		/// <summary>
		/// Gets all facilities of type  Swimming pool
		/// </summary>
		/// <returns></returns>
		[HttpGet("getexercisegyms")]
		public async Task<IActionResult> GetExerciseGyms()
		{
			_logger.LogDebug("Starting getexercisegyms");

			try
			{
				return Ok(await _facilityRepository.GetAllExerciseGyms());
			}
			catch (Exception ex)
			{

				return BadRequest(ex);
			}
		}
		/// <summary>
		/// controller for getting a single exercise gym by id 
		/// </summary>
		/// <param name="exerciseGymId"></param>
		/// <returns></returns>
		[HttpGet("getexercisegymbyid")]
		public async Task<IActionResult> GetExerciseGym([FromBody] string exerciseGymId)
		{
			_logger.LogDebug("Starting getexercisegymbyid for ExerciseGym" + exerciseGymId);
			try
			{
                var result = await _facilityRepository.GetAllExerciseGyms();
				return Ok(result.Single(ex => ex.Id == exerciseGymId));
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		/// <summary>
		/// controller for getting a single facility
		/// </summary>
		/// <param name="facilityId"></param>
		/// <returns></returns>
		[HttpGet("getfacilitybyid")]
        public async Task<IActionResult> GetFacility([FromBody] string facilityId)
        {
            _logger.LogDebug("Starting getfacilitybyid for facility" + facilityId);
            try
            {
                return Ok(await _facilityRepository.GetFacility(facilityId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// Gets all facilities of both types (Exercise Gym and Swimming pool)
        /// </summary>
        /// <returns></returns>
        [HttpGet("getfacilities")]
        public async Task<IActionResult> GetFacilities()
        {
            _logger.LogDebug("Starting getfacilities");

            try
            {
                return Ok(await _facilityRepository.GetFacilities());
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Inserts a single facility
        /// </summary>
        /// <param name="facility"></param>
        /// <returns></returns>
        [HttpPost("insertfacility")]
        public async Task<IActionResult> InsertFacility([FromBody] Facility facility)
        {
            _logger.LogDebug("Starting insertfacility for facility" + facility.Name);

            try
            {
                await _facilityRepository.InsertFacility(facility);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Updates a single facility
        /// </summary>
        /// <param name="facility"></param>
        /// <returns></returns>
        [HttpPut("updatefacility")]
        public async Task<IActionResult> UpdateFacility([FromBody] Facility facility)
        {
            _logger.LogDebug("starting updatefacility for facility" + facility.Id);

            try
            {
                await _facilityRepository.UpdateFacility(facility);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Deletes a single facility
        /// </summary>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        [HttpDelete("deletefacility")]
        public async Task<IActionResult> DeleteFacility([FromBody] string facilityId)
        {
            _logger.LogDebug("starting deletefacility for facility" + facilityId);

            try
            {
                await _facilityRepository.DeleteFacility(facilityId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
