using DigitalContentServiceAPI.Models;
using DigitalContentServiceAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalContentServiceAPI.Controllers
{
    [ApiController]
    [Route("workoutprogram")]
    public class WorkoutProgramController : ControllerBase
    {
        private readonly ILogger<WorkoutProgramController> _logger;
        private readonly IWorkoutProgramRepository _programRepo;

        public WorkoutProgramController(ILogger<WorkoutProgramController> logger, IWorkoutProgramRepository programRepo)
        {
            _logger = logger;
            _programRepo = programRepo;
        }

        // POST
        [Authorize]
        [HttpPost("insert")]
        public async Task<IActionResult> InsertWorkoutProgramAsync(WorkoutProgram workoutProgram)
        {
            try
            {
                await _programRepo.InsertWorkoutProgramAsync(workoutProgram);
                _logger.LogInformation("Workout program successfully inserted: {Name}", workoutProgram.Name);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed inserting: {Name}", workoutProgram.Name);
                return BadRequest(ex.Message);
            }
        }

        // GET
        [Authorize]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetWorkoutProgramAsync(Guid id)
        {
            try
            {
                await _programRepo.GetWorkoutProgramAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get workoutprogram with id: {id}", id);
                return BadRequest(ex.Message);
            }
        }

        // PUT
        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateWorkoutProgramAsync(Guid id, WorkoutProgram workoutProgram)
        {
            try
            {
                await _programRepo.UpdateWorkoutProgramAsync(id, workoutProgram);
                _logger.LogInformation("Workout program {id} updated", id);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Workout program with id {id} not found", id);
                return NotFound(ex.Message);
            }
        }

        // DELETE 
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteWorkoutProgramAsync(Guid id)
        {
            try
            {
                await _programRepo.DeleteWorkoutProgramAsync(id);
                _logger.LogInformation("Workout program {id} deleted", id);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Workout program with id {id} not found", id);
                return NotFound(ex.Message);
            }
        }
    }
}