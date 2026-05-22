using Microsoft.EntityFrameworkCore;
using RapportServiceAPI.Models;

namespace RapportServiceAPI.Data
{
    public class RapportDbContext : DbContext
    {
        // Constructor som modtager konfiguration fra Program.cs via dependency injection
        public RapportDbContext(DbContextOptions<RapportDbContext> options) : base(options)
        {
        }

        // Repræsenterer Statistikker-containeren i CosmosDB
        public DbSet<Statistik> Statistics => Set<Statistik>();

        // Konfigurerer hvordan Statistik-objekter gemmes i CosmosDB
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Statistik>(entity =>
            {
                entity.ToContainer("Statistics");
                entity.HasKey(s => s.Id);
                entity.HasPartitionKey(s => s.PartitionKey);

                // Value converter + comparer for List<Guid> properties
                var guidListConverter = new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<List<Guid>, List<string>>(
                    v => v.Select(g => g.ToString()).ToList(),
                    v => v.Select(s => Guid.Parse(s)).ToList()
                );

                var guidListComparer = new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<Guid>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                );

                entity.Property(s => s.Registrered)
                    .HasConversion(guidListConverter)
                    .Metadata.SetValueComparer(guidListComparer);

                entity.Property(s => s.Attended)
                    .HasConversion(guidListConverter)
                    .Metadata.SetValueComparer(guidListComparer);

                entity.Property(s => s.WaitingList)
                    .HasConversion(guidListConverter)
                    .Metadata.SetValueComparer(guidListComparer);
            });
        }
    }
}