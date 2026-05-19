using Microsoft.EntityFrameworkCore;
using System.Data;
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

        public DbSet<Member> Members => Set<Member>();

        public DbSet<Employee> Employees => Set<Employee>();

        //AO: Configures how the entity is mapped to the database. Defines how models are saved, keys, partitions, containers etc. 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //AO: Tells EF how we want to handle "User"-entities in the db
            modelBuilder.Entity<User>()
                .ToContainer("Users");

            //AO: What the documents (objects) are grouped by
            modelBuilder.Entity<User>()
                .HasPartitionKey(u => u.PartitionKey);

            //AO: Defines the PK/unique identity of the docuemnts/objects
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            //AO: Tells EF “ Når du gemmer objekter i Users-containeren, skal du gemme hvilken subtype objektet er." Gøres med udgangspunkt i typen
            modelBuilder.Entity<User>()
               .HasDiscriminator<string>("userType")
               .HasValue<Member>("Member")
               .HasValue<Employee>("Employee");

            //CS: Registrerer Member eksplicit så EF Core Cosmos lettere kan håndtere queries på den afledte type
            modelBuilder.Entity<Member>();

            //CS: Registrerer Employee eksplicit så EF Core Cosmos lettere kan håndtere queries på den afledte type
            modelBuilder.Entity<Employee>();
        }

    }
}