using DigitalContentServiceAPI.Models;

namespace DigitalContentServiceAPI.Repositories.Interfaces;

public interface IWorkoutVideoRepository
{
    
    // POST
    
    Task InsertWorkoutVideoAsync(WorkoutVideo workoutVideoToInsert);
   
    // GET
    
    Task<WorkoutVideo?> GetWorkoutVideoAsync(Guid id);

    public Task<List<WorkoutVideo>> GetWorkoutVideosAsync();

	// DELETE

	Task<bool> DeleteWorkoutVideoAsync(Guid id);
}