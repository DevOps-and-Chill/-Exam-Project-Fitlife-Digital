using FacilityServiceAPI.Models;

namespace FacilityServiceAPI.Repositories
{
    public interface IFacilityRepository
    {
        public Task<Facility> GetFacility(string facilityId);

        public Task<List<Facility>> GetFacilities();

        public Task InsertFacility(Facility facility);

        public Task UpdateFacility(Facility facility);

        public Task DeleteFacility(string facilityId);
    }
}
