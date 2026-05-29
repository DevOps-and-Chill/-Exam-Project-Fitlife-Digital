using FacilityServiceAPI.Contexts;
using FacilityServiceAPI.Models;
using FacilityServiceAPI.Repositories.Interfaces;
using FacilityServiceAPI.TestData;
using Microsoft.EntityFrameworkCore;

namespace FacilityServiceAPI.Repositories
{
	public class ExerciseGymRepository : IExerciseGymRepository
	{
		private readonly IDbContextFactory<FacilityContext> _contextFactory;

		public ExerciseGymRepository(IDbContextFactory<FacilityContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns></returns>
		public async Task<List<ExerciseGym>> GetAllExerciseGyms()
		{
			using (var facilityContext = await GetDbContextAsync())
			{
				var result = await facilityContext.Facilities.OfType<ExerciseGym>().ToListAsync();
				return result;
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="exerciseGym"></param>
		/// <returns></returns>
        public async Task InsertExerciseGym(ExerciseGym exerciseGym)
        {
            using (var facilityContext = await GetDbContextAsync())
            {
                await facilityContext.Facilities.AddAsync(exerciseGym);
                await facilityContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="exerciseGym"></param>
        /// <returns></returns>
        public async Task UpdateExerciseGym(ExerciseGym exerciseGym)
        {
            if (exerciseGym == null)
            {
                throw new Exception("Cannot update with a null-ified object");
            } 
            using (var facilityContext = await GetDbContextAsync())
            {
                facilityContext.Update(exerciseGym);
                await facilityContext.SaveChangesAsync();
            }
        }

		/// <summary>
		/// Small helper method for creating a DB context, looked better.
		/// </summary>
		/// <returns></returns>
		private async Task<FacilityContext> GetDbContextAsync()
		{
			return await _contextFactory.CreateDbContextAsync();
		}
	}
}
