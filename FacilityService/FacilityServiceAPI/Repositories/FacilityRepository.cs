using FacilityServiceAPI.Contexts;
using FacilityServiceAPI.Models;
using FacilityServiceAPI.Repositories.Interfaces;
using FacilityServiceAPI.TestData;
using Microsoft.EntityFrameworkCore;

namespace FacilityServiceAPI.Repositories
{
	public class FacilityRepository : IFacilityRepository
	{
		private readonly IDbContextFactory<FacilityContext> _contextFactory;

		public FacilityRepository(IDbContextFactory<FacilityContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="facilityId"></param>
		/// <returns></returns>
		public async Task<Facility> GetFacility(string facilityId)
		{
			using (var facilityContext = await GetDbContextAsync())
			{
				var result = await facilityContext.Facilities.SingleAsync(f => f.Id == facilityId);
				return result;
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns></returns>
		public async Task<List<Facility>> GetFacilities()
		{
			using (var facilityContext = await GetDbContextAsync())
			{
				var result = await facilityContext.Facilities.ToListAsync();
				return result;
			}
		}

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeleteFacility(string facilityId)
		{
			using (var facilityContext = await GetDbContextAsync())
			{
				facilityContext.Remove(facilityId);
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns></returns>
		public async Task AddTestData()
		{
			using (var facilityContext = await GetDbContextAsync())
			{
                await facilityContext.AddRangeAsync(
					FacilityTestData.ExerciseGyms);

                await facilityContext.AddRangeAsync(
                    FacilityTestData.SwimmingPools);
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
