using DigitalContentServiceAPI.Models;

namespace DigitalContentServiceAPI.Repositories.Interfaces;

public interface IWorkoutVideoRepository
{
    Task InsertWorkoutVideo(WorkoutVideo workoutVideoToInsert);
    
    Task<WorkoutVideo> GetWorkoutVideo(Guid id);
}