using FacilityServiceAPI.Contexts;
using FacilityServiceAPI.Models;
using FacilityServiceAPI.Repositories.Interfaces;
using FacilityServiceAPI.TestData;
using Microsoft.EntityFrameworkCore;

namespace FacilityServiceAPI.Repositories
{
	public class SwimmingPoolRepository : ISwimmingPoolRepository
	{
		private readonly IDbContextFactory<FacilityContext> _contextFactory;

		public SwimmingPoolRepository(IDbContextFactory<FacilityContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <returns></returns>
		public async Task<List<SwimmingPool>> GetAllSwimmingPools()
		{
			using (var facilityContext = await GetDbContextAsync())
			{
				var result = await facilityContext.Facilities.OfType<SwimmingPool>().ToListAsync();
				return result;
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <param name="swimmingPool"></param>
		/// <returns></returns>
		public async Task InsertSwimmingpool(SwimmingPool swimmingPool)
		{
			using (var facilityContext = await GetDbContextAsync())
			{
				await facilityContext.Facilities.AddAsync(swimmingPool);
				await facilityContext.SaveChangesAsync();
			}
		}


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="swimmingPool"></param>
        /// <returns></returns>
        /// <exception></exception>
        public async Task UpdateSwimmingPool(SwimmingPool swimmingPool)
		{
            if (swimmingPool == null)
            {
                throw new Exception("Cannot update with a null-ified object");
            }
            using (var facilityContext = await GetDbContextAsync())
            {
                facilityContext.Update(swimmingPool);
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
