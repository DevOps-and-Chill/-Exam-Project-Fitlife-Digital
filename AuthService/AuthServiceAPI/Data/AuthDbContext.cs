using AuthServiceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthServiceAPI.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        public DbSet<Credential> AuthUsers => Set<Credential>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Credential>(entity =>
            {
                // This container only stores authentication details.
                entity.ToContainer("AuthUsers");

                // The Id originates from UserService and acts as the globally unique identifier across services.
                entity.HasKey(u => u.Id);

                // Partitioning strategy:
                // We partition by Id because it is immutable and provides high cardinality and good distribution in CosmosDB.
                // Using Email as partition key could optimize login queries, but emails may change over time, while partition keys cannot.
                // This is therefore a scalability-oriented design choice.
                entity.HasPartitionKey(u => u.Id);

                // Required fields ensure documents always contain the necessary authentication data.
                entity.Property(u => u.Id)
                    .IsRequired();

                entity.Property(u => u.Email)
                    .IsRequired();

                entity.Property(u => u.PasswordHash)
                    .IsRequired();

                // Email uniqueness is enforced to prevent duplicate accounts.
                entity.HasIndex(u => u.Email)
                    .IsUnique();
            });
        }
    }
}
