using Microsoft.EntityFrameworkCore;
using System.Data;
using ClassServiceAPI.Models;

namespace ClassServiceAPI.Data;

public class ClassDbContext : DbContext
{
    public ClassDbContext(DbContextOptions<ClassDbContext> options) : base(options)
    {
}
    public DbSet<Class> Classes => Set<Class>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //AO: Tells EF how we want to handle "User"-entities in the db
        modelBuilder.Entity<Class>()
            .ToContainer("Classes");

        //AO: What the documents (objects) are grouped by
        modelBuilder.Entity<Class>()
            .HasPartitionKey(u => u.PartitionKey);

        //AO: Defines the PK/unique identity of the docuemnts/objects
        modelBuilder.Entity<Class>()
            .HasKey(u => u.Id);
    }
}