using FacilityServiceAPI.Models;
using FacilityServiceAPI.TestData;

namespace FacilityServiceAPI.Repositories
{
	public class FacilityRepositoryMoq : IFacilityRepository
	{
		public Task DeleteFacility(string facilityId)
		{
			return Task.FromResult(FacilityTestData.Facilities.Remove((FacilityTestData.Facilities.First(x => x.Id.ToString() == facilityId))));
		}

		public Task<List<Facility>> GetFacilities()
		{
			return Task.FromResult(FacilityTestData.Facilities);
		}

		public Task<Facility> GetFacility(string facilityId)
		{
			return Task.FromResult(FacilityTestData.Facilities.Single(f => f.Id.ToString() == facilityId));
		}

		public Task InsertFacility(Facility facility)
		{
			FacilityTestData.Facilities.Add(facility);

			return Task.CompletedTask;
		}

		public Task UpdateFacility(Facility facility)
		{
			FacilityTestData.Facilities.Remove(facility);
			FacilityTestData.Facilities.Add(facility);

			return Task.CompletedTask;
		}
	}
}
