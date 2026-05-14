using Microsoft.EntityFrameworkCore;
using UserServiceAPI.Models;

namespace UserServiceAPI.Data
{
    public class UserDbContext : DbContext
    {
        //AO: Constructor for EF Core, used for config such as connectionstring, CosmosDB settings, logging etc., handled mostly by EF, which gets it from program.cs, appsettings, DI etc.
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        //AO: Represents the Users container/collection (in this case its a container in CosmosDB that contains "User"-objects)
        public DbSet<User> Users => Set<User>();

        //AO: Configures how the entity is mapped to the database. Defines how models are saved, keys, partitions, containers etc. 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //AO: Tells EF how we want to handle "User"-entities in the db
            modelBuilder.Entity<User>()
                .ToContainer("Users");

            //AO: What the documents (objects) are grouped by
            modelBuilder.Entity<User>()
                .HasPartitionKey(u => u.Id);

            //AO: Defines the PK/unique identity of the docuemnts/objects
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
        }

    }
}
