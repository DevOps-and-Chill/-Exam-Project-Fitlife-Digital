using Microsoft.EntityFrameworkCore;
using PTServiceAPI.Models;

namespace PTServiceAPI.Data
{
    public class PTDbContext : DbContext
    {
        // Constructor som modtager konfiguration fra Program.cs via dependency injection
        public PTDbContext(DbContextOptions<PTDbContext> options) : base(options)
        {
        }

        // Repræsenterer Sessions-containeren i CosmosDB
        public DbSet<Session> Sessions => Set<Session>();

        // Konfigurerer hvordan Session-objekter gemmes i CosmosDB
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Session>(entity =>
            {
                // Navnet på containeren i CosmosDB
                entity.ToContainer("Sessions");

                // Primærnøgle
                entity.HasKey(s => s.Id);

                // Partitionsnøgle — bruges til at fordele data i CosmosDB
                entity.HasPartitionKey(s => s.PartitionKey);
            });
        }
    }
}