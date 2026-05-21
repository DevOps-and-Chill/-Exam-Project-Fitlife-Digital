using FacilityServiceAPI.Models;

namespace FacilityServiceAPI.Repositories.Interfaces
{
    public interface ISwimmingPoolRepository
    {
        /// <summary>
        /// Gets al the facilities of facility type SwimmingPool
        /// </summary>
        /// <returns></returns>
        public Task<List<SwimmingPool>> GetAllSwimmingPools();
       
        /// <summary>
        /// Insert method for adding a swimmingpool, that is parsed as a parameter
        /// </summary>
        /// <param name="facility"></param>
        /// <returns>Task.CompletedTask</returns>
        public Task InsertSwimmingpool(SwimmingPool swimmingPool);

        /// <summary>
        /// Updates the added swimmingpool 
        /// </summary>
        /// <param name="swimmingPool"></param>
        /// <returns>Task.CompletedTask</returns>
        public Task UpdateSwimmingPool(SwimmingPool swimmingPool);
	}
}
