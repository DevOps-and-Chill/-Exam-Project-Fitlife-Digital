using FacilityServiceAPI.Models;
using FacilityServiceAPI.Repositories.Interfaces;
using FacilityServiceAPI.TestData;

namespace FacilityServiceAPI.Repositories
{
	public class FacilityRepositoryMoq : IFacilityRepository, IExerciseGymRepository, ISwimmingPoolRepository
	{
		public Task AddTestData()
		{
			throw new NotImplementedException();
		}

		public Task DeleteFacility(string facilityId)
		{
			return Task.FromResult(FacilityTestData.ExerciseGyms.Remove((FacilityTestData.ExerciseGyms.First(x => x.Id.ToString() == facilityId))));
		}

		public Task<List<ExerciseGym>> GetAllExerciseGyms()
		{
			return Task.FromResult(FacilityTestData.ExerciseGyms);
		}

		public Task<List<SwimmingPool>> GetAllSwimmingPools()
		{
			return Task.FromResult(FacilityTestData.SwimmingPools);
		}

		public Task<List<Facility>> GetFacilities()
		{
			var concatList = new List<Facility>();
			concatList.AddRange(FacilityTestData.ExerciseGyms);
			concatList.AddRange(FacilityTestData.SwimmingPools);

			return Task.FromResult(concatList);
		}

		public Task<Facility> GetFacility(string facilityId)
		{
			var concatList = new List<Facility>();
			concatList.AddRange(FacilityTestData.ExerciseGyms);
			concatList.AddRange(FacilityTestData.SwimmingPools);
			return Task.FromResult(concatList.Single(f => f.Id.ToString() == facilityId));
		}

        public Task InsertExerciseGym(ExerciseGym exerciseGym)
        {
            throw new NotImplementedException();
        }

        public Task InsertFacility(Facility facility)
		{
			if (facility.GetType() == typeof(ExerciseGym))
			{
				FacilityTestData.ExerciseGyms.Add((ExerciseGym)facility);
			}
			else
			{
				FacilityTestData.SwimmingPools.Add((SwimmingPool)facility);
			}

			return Task.CompletedTask;
		}

        public Task InsertSwimmingpool(SwimmingPool swimmingPool)
        {
            throw new NotImplementedException();
        }

        public Task UpdateExerciseGym(ExerciseGym exerciseGym)
        {
            throw new NotImplementedException();
        }

        public Task UpdateFacility(Facility facility)
		{
			if (facility.GetType() == typeof(ExerciseGym))
			{
				FacilityTestData.ExerciseGyms.Remove((ExerciseGym)facility);
				FacilityTestData.ExerciseGyms.Add((ExerciseGym)facility);
			}
			else
			{
				FacilityTestData.SwimmingPools.Remove((SwimmingPool)facility);
				FacilityTestData.SwimmingPools.Add((SwimmingPool)facility);
			}

			return Task.CompletedTask;
		}

        public Task UpdateSwimmingPool(SwimmingPool swimmingPool)
        {
            throw new NotImplementedException();
        }
    }
}
