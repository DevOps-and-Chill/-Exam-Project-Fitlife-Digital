using FacilityServiceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FacilityServiceAPI.Contexts
{
	public class FacilityContext : DbContext
	{
		public FacilityContext(DbContextOptions<FacilityContext> options) : base(options)
		{

		}

		public DbSet<Facility> Facilities => Set<Facility>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Facility>().ToContainer("Facilities");

			modelBuilder.Entity<Facility>()
				.HasPartitionKey(f => f.PartitionKey)
				.HasKey(f => f.Id);

			modelBuilder.Entity<Facility>().HasDiscriminator<string>("facilityType")
				.HasValue<ExerciseGym>("ExerciseGym")
				.HasValue<SwimmingPool>("SwimmingPool");

            modelBuilder.Entity<Facility>()
				.OwnsMany(f => f.OpeningHours);
        }
	}
}
