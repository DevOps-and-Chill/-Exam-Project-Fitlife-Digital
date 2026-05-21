using FacilityServiceAPI.Models;

namespace FacilityServiceAPI.Repositories.Interfaces
{
    public interface IExerciseGymRepository
    {
		/// <summary>
		/// Gets all the facilities of facility type ExerciseGym
		/// </summary>
		/// <returns>returns a list of all the exercise gyms </returns>
		public Task<List<ExerciseGym>> GetAllExerciseGyms();

        /// <summary>
        /// Insert method for adding an exercisegym, that is parsed as a parameter
        /// </summary>
        /// <param name="exerciseGym"></param>
        /// <returns>Task.CompletedTask</returns>
        public Task InsertExerciseGym(ExerciseGym exerciseGym);

        /// <summary>
        /// Updates the added exercisegym.  
        /// </summary>
        /// <param name="exerciseGym"></param>
        /// <returns>Task.CompletedTask</returns>
        public Task UpdateExerciseGym(ExerciseGym exerciseGym);

	}
}
