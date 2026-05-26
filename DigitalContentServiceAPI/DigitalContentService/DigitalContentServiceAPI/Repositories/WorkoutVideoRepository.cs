using DigitalContentServiceAPI.Data;
using DigitalContentServiceAPI.Models;
using DigitalContentServiceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalContentServiceAPI.Repositories;

public class WorkoutVideoRepository : IWorkoutVideoRepository
{
    
    private readonly DigitalContentDbContext _context;
    private readonly ILogger<WorkoutVideoRepository> _logger;
    
    public WorkoutVideoRepository(DigitalContentDbContext context, ILogger<WorkoutVideoRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task InsertWorkoutVideoAsync(WorkoutVideo workoutVideoToInsert)
    {
        _context.Set<WorkoutVideo>().Add(workoutVideoToInsert);
        await _context.SaveChangesAsync();
    }
    public async Task<WorkoutVideo?> GetWorkoutVideoAsync(Guid id)
    {
        var workoutVideo = await _context.Set<WorkoutVideo?>()
            .FirstOrDefaultAsync(c => c.Id == id);
        
        if (workoutVideo == null)
            _logger.LogInformation("Workout video not found");
        
        return workoutVideo;
    }
    
    public async Task<List<WorkoutVideo>> GetWorkoutVideosAsync()
    {
        return await _context.Set<WorkoutVideo>().ToListAsync();
    }

    public async Task<bool> DeleteWorkoutVideoAsync(Guid id)
    {
        var workoutVideo = _context.Set<WorkoutVideo>().
            FirstOrDefault(w => w.Id == id);
        
        if (workoutVideo == null)
        return false;
        
        _context.DigitalContent.Remove(workoutVideo);
        await _context.SaveChangesAsync();
        return true;
    }
}