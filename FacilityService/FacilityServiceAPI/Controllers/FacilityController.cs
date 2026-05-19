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
