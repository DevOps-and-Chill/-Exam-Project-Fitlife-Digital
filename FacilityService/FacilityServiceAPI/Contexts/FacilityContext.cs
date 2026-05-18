using FacilityServiceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FacilityServiceAPI.Contexts
{
	public class FacilityContext:DbContext
	{
		public FacilityContext(DbContextOptions<FacilityContext> options):base (options)
		{
			
		}

		public DbSet<Facility> db => Set<Facility>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Facility>().ToContainer("Facilities");

			mo
		}
	}
}
