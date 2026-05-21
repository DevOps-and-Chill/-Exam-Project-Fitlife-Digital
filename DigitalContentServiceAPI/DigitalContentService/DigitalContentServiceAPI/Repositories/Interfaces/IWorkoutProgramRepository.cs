using DigitalContentServiceAPI.Models;

namespace DigitalContentServiceAPI.Repositories.Interfaces
{
	public interface IWorkoutProgramRepository
	{
		public Task InsertWorkoutProgram(WorkoutProgram workoutProgramToInsert);
		
		public Task GetWorkoutProgram(Guid id);
	}
}
