using DigitalContentServiceAPI.Models;
using DigitalContentServiceAPI.Repositories.Interfaces;

namespace DigitalContentServiceAPI.Repositories;

public class WorkoutVideoRepository : IWorkoutVideoRepository
{
    
    private readonly List<WorkoutVideo> _workoutVideos = new List<WorkoutVideo>();
    public Task InsertWorkoutVideo(WorkoutVideo workoutVideoToInsert)
    {
        _workoutVideos.Add(workoutVideoToInsert);
        return Task.CompletedTask;
    }
    public Task<WorkoutVideo> GetWorkoutVideo(Guid id)
    {
        var workoutVideo = _workoutVideos.FirstOrDefault(w => w.Id == id);
        return Task.FromResult(workoutVideo);
    }
}