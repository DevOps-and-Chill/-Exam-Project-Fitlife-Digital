using FacilityServiceAPI.Models;

namespace FacilityServiceAPI.Repositories.Interfaces
{
    public interface IFacilityRepository
    {
        
        /// <summary>
        /// Method for returning a single facility, takes a  facilityId as parameter
        /// </summary>
        /// <param name="facilityId"></param>
        /// <returns> a single facility object</returns>
        public Task<Facility> GetFacility(string facilityId);
        
        /// <summary>
        /// Method for returning all the facilities, of all types.
        /// </summary>
        /// <returns>a list<Facility></Facility></returns>
        public Task<List<Facility>> GetFacilities();

        /// <summary>
        /// Method for deleting a single facility, takes a facilityId as parameter
        /// </summary>
        /// <param name="facilityId"></param>
        /// <returns>Task.CompletedTask</returns>
        public Task DeleteFacility(string facilityId);

        /// <summary>
        /// Endpoint for adding TestData To CosmosDB
        /// </summary>
        /// <returns></returns>
		public Task AddTestData();
	}
}
