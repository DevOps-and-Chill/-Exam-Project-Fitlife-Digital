using DigitalContentServiceAPI.Models;
using DigitalContentServiceAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DigitalContentServiceAPI.Repositories
{
	public class WorkoutProgramRepository : IWorkoutProgramRepository
	{
		private readonly List<WorkoutProgram> _workoutPrograms = new List<WorkoutProgram>();
		public Task InsertWorkoutProgram(WorkoutProgram workoutProgramToInsert)
		{
			_workoutPrograms.Add(workoutProgramToInsert);
			return Task.CompletedTask;
		}

		public Task GetWorkoutProgram(Guid id)
		{
			var programs = _workoutPrograms.Where(p => p.Id == id).ToList();
			return Task.FromResult(programs);
		}
	}
}
