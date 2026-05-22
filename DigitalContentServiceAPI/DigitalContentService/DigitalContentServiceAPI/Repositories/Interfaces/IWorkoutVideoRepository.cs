using DigitalContentServiceAPI.Models;

namespace DigitalContentServiceAPI.Repositories.Interfaces;

public interface IWorkoutVideoRepository
{
    
    // POST
    
    Task InsertWorkoutVideoAsync(WorkoutVideo workoutVideoToInsert);
   
    // GET
    
    Task<WorkoutVideo?> GetWorkoutVideoAsync(Guid id);
    
    // DELETE
    
    Task<bool> DeleteWorkoutVideoAsync(Guid id);
}