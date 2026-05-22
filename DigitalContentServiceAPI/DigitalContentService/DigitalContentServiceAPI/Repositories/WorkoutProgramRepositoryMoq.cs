using DigitalContentServiceAPI.Models;
using DigitalContentServiceAPI.Repositories.Interfaces;

namespace DigitalContentServiceAPI.Repositories
{
	public class WorkoutProgramRepositoryMoq : IWorkoutProgramRepository
	{
		public Task InsertWorkoutProgramAsync(WorkoutProgram workoutProgramToInsert)
		{
			throw new NotImplementedException();
		}

		public Task<WorkoutProgram?> GetWorkoutProgramAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UpdateWorkoutProgramAsync(Guid id, WorkoutProgram workoutProgramToChange)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteWorkoutProgramAsync(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}
