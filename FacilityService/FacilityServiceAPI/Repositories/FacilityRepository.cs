using FacilityServiceAPI.Models;

namespace FacilityServiceAPI.Repositories
{
    public class FacilityRepository : IFacilityRepository
	{
        public Task DeleteFacility(string facilityId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Facility>> GetFacilities()
        {
            throw new NotImplementedException();
        }

        public Task<Facility> GetFacility(string facilityId)
        {
            throw new NotImplementedException();
        }

        public Task InsertFacility(Facility facility)
        {
            throw new NotImplementedException();
        }

        public Task UpdateFacility(Facility facility)
        {
            throw new NotImplementedException();
        }
    }
}
