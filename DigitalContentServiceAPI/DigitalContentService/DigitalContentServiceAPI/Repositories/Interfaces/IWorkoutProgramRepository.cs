using DigitalContentServiceAPI.Models;

namespace DigitalContentServiceAPI.Repositories.Interfaces
{
	public interface IWorkoutProgramRepository
	{
		
		// POST
		
		Task InsertWorkoutProgramAsync(WorkoutProgram workoutProgramToInsert);
		
		// GET
		
		Task<WorkoutProgram?> GetWorkoutProgramAsync(string id);

		Task<List<WorkoutProgram>> GetWorkoutProgramsAsync();
		
		// PUT

		Task<bool> UpdateWorkoutProgramAsync(string id, WorkoutProgram workoutProgramToChange);
		
		// DELETE
		
		Task<bool> DeleteWorkoutProgramAsync(string id);
	}
}
