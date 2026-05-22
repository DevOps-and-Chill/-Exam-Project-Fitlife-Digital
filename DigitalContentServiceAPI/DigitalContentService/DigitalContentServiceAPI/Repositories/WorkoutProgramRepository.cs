using System.Globalization;
using DigitalContentServiceAPI.Data;
using DigitalContentServiceAPI.Models;
using DigitalContentServiceAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace DigitalContentServiceAPI.Repositories
{
	public class WorkoutProgramRepository : IWorkoutProgramRepository
	{
		    private readonly DigitalContentDbContext _context;
			private readonly ILogger<WorkoutProgramRepository> _logger;
    
    public WorkoutProgramRepository(DigitalContentDbContext context, ILogger<WorkoutProgramRepository> logger)
    {
	    _context = context;
	    _logger = logger;
    }
		
		// POST
		
		public async Task InsertWorkoutProgramAsync(WorkoutProgram workoutProgramToInsert)
		{
			_context.DigitalContent.Add(workoutProgramToInsert);
			await _context.SaveChangesAsync();
		}

		// GET
	
		public async Task<WorkoutProgram?> GetWorkoutProgramAsync(Guid id)
		{
			return await _context.Set<WorkoutProgram>()
				.FirstOrDefaultAsync(c => c.Id == id);
		}
		
		public async Task<bool> UpdateWorkoutProgramAsync(Guid id, WorkoutProgram workoutProgramToChange)
		{
			var program = await _context.Set<WorkoutProgram>().FirstOrDefaultAsync( c => c.Id == id);
		   
			if (program == null)
				return false;
			
			program.Name = workoutProgramToChange.Name;
			program.Description = workoutProgramToChange.Description;
			program.ActiveContent = workoutProgramToChange.ActiveContent;
			program.ProgramGoal = workoutProgramToChange.ProgramGoal;
			program.Workouts = workoutProgramToChange.Workouts;
			program.GeneralEducationLevel = workoutProgramToChange.GeneralEducationLevel;
			program.TimeRequired = workoutProgramToChange.TimeRequired;
			program.DateModified = DateTime.Now;
			
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> DeleteWorkoutProgramAsync(Guid id)
		{
			var program = _context.DigitalContent.FirstOrDefault( c => c.Id == id);
		
			if (program == null)
				return false;

			_context.DigitalContent.Remove(program);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
