using FacilityServiceAPI.Models;
using FacilityServiceAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FacilityServiceAPI.Controllers
{
    [ApiController]
    [Route("facility")]
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
		/// controller for getting a single facility
		/// </summary>
		/// <param name="facilityId"></param>
		/// <returns></returns>
		[HttpGet("getbyid")]
        public async Task<IActionResult> GetFacility([FromBody] string facilityId)
        {
            _logger.LogDebug("Starting getfacilitybyid for facility {facilityId}", facilityId);
            try
            {
                return Ok(await _facilityRepository.GetFacility(facilityId));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getfacilitybyid for {facilityId}: {message}", facilityId, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Gets all facilities of both types (Exercise Gym and Swimming pool)
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        public async Task<IActionResult> GetFacilities()
        {
            _logger.LogDebug("Starting getfacilities");

            try
            {
                return Ok(await _facilityRepository.GetFacilities());
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getfacilities: {message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a facility (doesnt care about type exercisegym or swimmingpool
        /// </summary>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteFacility([FromBody] string facilityId)
        {
            _logger.LogDebug("starting deletefacility for facility {facilityId}", facilityId);

            try
            {
                await _facilityRepository.DeleteFacility(facilityId);
                _logger.LogInformation("Facility {facilityId} deleted successfully", facilityId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting facility {facilityId}: {message}", facilityId, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("addtestdata")]
        public async Task<IActionResult> loadtestdata()
        {
            _logger.LogDebug("Starting loadtestdata");
            try
            {
                await _facilityRepository.AddTestData();
                _logger.LogInformation("Test data loaded successfully");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error loading test data: {message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
