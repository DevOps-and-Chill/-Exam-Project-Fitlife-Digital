using DigitalContentServiceAPI.Models;
using DigitalContentServiceAPI.Repositories.Interfaces;

namespace DigitalContentServiceAPI.Repositories
{
	public class WorkoutProgramRepositoryMoq : IWorkoutProgramRepository
	{
		public Task InsertWorkoutProgram(WorkoutProgram workoutProgramToInsert)
		{
			throw new NotImplementedException();
		}

		public Task GetWorkoutProgram(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}
