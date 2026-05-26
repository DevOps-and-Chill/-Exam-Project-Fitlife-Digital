using DigitalContentServiceAPI.Models;
using DigitalContentServiceAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalContentServiceAPI.Controllers
{
    [ApiController]
    [Route("workoutvideo")]
    public class WorkoutVideoController : ControllerBase
    {
        private readonly ILogger<WorkoutVideoController> _logger;
        private readonly IWorkoutVideoRepository _videoRepo;

        public WorkoutVideoController(ILogger<WorkoutVideoController> logger, IWorkoutVideoRepository videoRepo)
        {
            _logger = logger;
            _videoRepo = videoRepo;
        }

        // POST

        [HttpPost("insert")]
        public async Task<IActionResult> InsertWorkoutVideoAsync(WorkoutVideo workoutVideo)
        {
            try
            {
                await _videoRepo.InsertWorkoutVideoAsync(workoutVideo);
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

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetWorkoutVideoAsync(Guid id)
        {
            try
            {
                var video = await _videoRepo.GetWorkoutVideoAsync(id);
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

        [HttpGet("getall")]
        public async Task<IActionResult> GetWorkoutVideosAsync()
        {
            try
            {
                return Ok(await _videoRepo.GetWorkoutVideosAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured when trying to fetch all workout videos", ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        // DELETE

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteWorkoutVideoAsync(Guid id)
        {
            try
            {
                await _videoRepo.DeleteWorkoutVideoAsync(id);
                _logger.LogInformation("Workout video {id} deleted", id);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Workout video with id {id} not found", id);
                return NotFound(ex.Message);
            }

        }
    }
}