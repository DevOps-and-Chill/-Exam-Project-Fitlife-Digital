using FacilityServiceAPI.Models;

namespace FacilityServiceAPI.Repositories
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
		/// Gets all the facilities of facility type ExerciseGym
		/// </summary>
		/// <returns>returns a list of all the exercise gyms </returns>
		public Task<List<ExerciseGym>> GetAllExerciseGyms();

        /// <summary>
        /// Gets al the facilities of facility type SwimmingPool
        /// </summary>
        /// <returns></returns>
        public Task<List<SwimmingPool>> GetAllSwimmingPools();
       
        /// <summary>
        /// Insert method for adding a facility, that is parsed as a parameter
        /// </summary>
        /// <param name="facility"></param>
        /// <returns>Task.CompletedTask</returns>
        public Task InsertFacility(Facility facility);
        
        /// <summary>
        /// Updates the parsed parameter.  
        /// </summary>
        /// <param name="facility"></param>
        /// <returns>Task.CompletedTask</returns>
        public Task UpdateFacility(Facility facility);

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
