using DigitalContentServiceAPI.Models;
using DigitalContentServiceAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalContentServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DigitalContentController : ControllerBase
    {
        private readonly ILogger<DigitalContentController> _logger;
        private readonly IWorkoutProgramRepository _programRepo;
        private readonly IWorkoutVideoRepository _videoRepo;

        public DigitalContentController(ILogger<DigitalContentController> logger, IWorkoutProgramRepository programRepo, IWorkoutVideoRepository videoRepo)
        {
            _logger = logger;
            _programRepo = programRepo;
            _videoRepo = videoRepo;
        }

        // POST
        
        [HttpPost("programs")]
        public async Task<IActionResult> InsertWorkoutProgramAsync(WorkoutProgram workoutProgram)
        {
            try
            {
                await _programRepo.InsertWorkoutProgram(workoutProgram);
                _logger.LogInformation("Workout program successfully inserted: {Name}", workoutProgram.Name);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed inserting: {Name}", workoutProgram.Name);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("videos")]
        public async Task<IActionResult> InsertWorkoutVideoAsync(WorkoutVideo workoutVideo)
        {
            try
            {
                await _videoRepo.InsertWorkoutVideo(workoutVideo);
                _logger.LogInformation("Workout video successfully inserted: {Name}", workoutVideo.Name);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to insert: {Name}", workoutVideo.Name);
                return BadRequest(ex.Message);
            }
        }

        // GET
        
        [HttpGet("programs/{id}")]
        public async Task<IActionResult> GetWorkoutProgramAsync(Guid id)
        {
            try
            {
                await _programRepo.GetWorkoutProgram(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get workoutprogram with id: {id}", id);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("videos/{id}")]
        public async Task<IActionResult> GetWorkoutVideoAsync(Guid id)
        {
            try
            {
                var video = await _videoRepo.GetWorkoutVideo(id);
                if (video is null)
                {
                    _logger.LogWarning("Workout video with {id} not found", id);
                }
                return Ok(video);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fejl ved hentning af workout video med id: {id}", id);
                return BadRequest(ex.Message);
            }
        }

        // PUT
        
        [HttpPut("programs/{id}")]
        public async Task<IActionResult> ChangeWorkoutProgramAsync(Guid id, WorkoutProgram workoutProgram)
        {
            try
            {
                var updated = await _programRepo.UpdateWorkoutProgram(id, workoutProgram);
                _logger.LogInformation("Workout program {id} updated", id);
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Workout program with id {id} not found", id);
                return NotFound(ex.Message);
            }
        }
        
        [HttpPut("videos/{id}")]
        public async Task<IActionResult> UpdateWorkoutVideoAsync(Guid id, WorkoutVideo workoutVideo)
        {
            try
            {
                var updated = await _videoRepo.UpdateWorkoutVideo(id, workoutVideo);
                _logger.LogInformation("Workout video {id} updated", id);
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Workout video with {id} not found", id);
                return NotFound(ex.Message);
            }
        }

        // DELETE 
        
        [HttpDelete("programs/{id}")]
        public async Task<IActionResult> DeleteWorkoutProgramAsync(Guid id)
        {
            try
            {
                var deleted = await _programRepo.DeleteWorkoutProgram(id);
                _logger.LogInformation("Workout program {id} deleted", id);
                return Ok(deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Workout program with id {id} not found", id);
                return NotFound(ex.Message);
            }
        }
        
        [HttpDelete("videos/{id}")]
        public async Task<IActionResult> DeleteWorkoutVideoAsync(Guid id)
        {
            try
            {
                var deleted = await _videoRepo.DeleteWorkoutVideo(id);
                _logger.LogInformation("Workout video {id} deleted", id);
                return Ok(deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Workout video with id {id} not found", id);
                return NotFound(ex.Message);
            }
        }
    }
}