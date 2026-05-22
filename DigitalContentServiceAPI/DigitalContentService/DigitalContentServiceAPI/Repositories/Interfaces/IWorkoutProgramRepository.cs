using DigitalContentServiceAPI.Models;

namespace DigitalContentServiceAPI.Repositories.Interfaces
{
	public interface IWorkoutProgramRepository
	{
		
		// POST
		
		Task InsertWorkoutProgramAsync(WorkoutProgram workoutProgramToInsert);
		
		// GET
		
		Task<WorkoutProgram?> GetWorkoutProgramAsync(Guid id);
		
		// PUT

		Task<bool> UpdateWorkoutProgramAsync(Guid id, WorkoutProgram workoutProgramToChange);
		
		// DELETE
		
		Task<bool> DeleteWorkoutProgramAsync(Guid id);
	}
}
