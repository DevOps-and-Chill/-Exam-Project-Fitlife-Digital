using FacilityServiceAPI.Models;
using FacilityServiceAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FacilityServiceAPI.Controllers
{
    [ApiController]
    [Route("exercisegym")]
    public class ExerciseGymController : ControllerBase
    {
        private readonly IExerciseGymRepository _exerciseGymRepository;
        private readonly ILogger<ExerciseGymController> _logger;

        public ExerciseGymController(ILogger<ExerciseGymController> logger , IExerciseGymRepository exerciseGymRepository)
        {
            _logger = logger;
            _exerciseGymRepository = exerciseGymRepository;
        }

		/// <summary>
		/// Gets all facilities of type 
		/// </summary>
		/// <returns></returns>
		[HttpGet("getall")]
		public async Task<IActionResult> GetExerciseGyms()
		{
			_logger.LogDebug("Starting getexercisegyms");

			try
			{
				return Ok(await _exerciseGymRepository.GetAllExerciseGyms());
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}
		}
		/// <summary>
		/// controller for getting a single exercise gym by id 
		/// </summary>
		/// <param name="exerciseGymId"></param>
		/// <returns></returns>
		[HttpGet("getbyid")]
		public async Task<IActionResult> GetExerciseGym([FromBody] string exerciseGymId)
		{
			_logger.LogDebug("Starting getexercisegymbyid for ExerciseGym" + exerciseGymId);
			try
			{
                var result = await _exerciseGymRepository.GetAllExerciseGyms();
				return Ok(result.Single(ex => ex.Id == exerciseGymId));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


        /// <summary>
        /// Inserts a single exercisegym
        /// </summary>
        /// <param name="exercisegym"></param>
        /// <returns></returns>
        [HttpPost("insert")]
        public async Task<IActionResult> InsertExerciseGym([FromBody] ExerciseGym exerciseGym)
        {
            _logger.LogDebug("Starting InsertExerciseGym for exerciseGym" + exerciseGym.Name);

            try
            {
                await _exerciseGymRepository.InsertExerciseGym(exerciseGym);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates a single exercisegym
        /// </summary>
        /// <param name="exercisegym"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateExerciseGym([FromBody] ExerciseGym exerciseGym)
        {
            _logger.LogDebug("starting updateexercisegym for exercisegym" + exerciseGym.Id);

            try
            {
                await _exerciseGymRepository.UpdateExerciseGym(exerciseGym);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
