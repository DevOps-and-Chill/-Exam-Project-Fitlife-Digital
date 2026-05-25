using Microsoft.EntityFrameworkCore;
using System.Data;
using DigitalContentServiceAPI.Models;

namespace DigitalContentServiceAPI.Data;

public class DigitalContentDbContext : DbContext
{
    public DigitalContentDbContext(DbContextOptions<DigitalContentDbContext> options) : base(options)
    {
    }

    public DbSet<DigitalContent> DigitalContent => Set<DigitalContent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //AO: Tells EF how we want to handle "DigitalContent"-entities in the db
        modelBuilder.Entity<DigitalContent>()
            .ToContainer("DigitalContent");

        //AO: What the documents (objects) are grouped by
        modelBuilder.Entity<DigitalContent>()
            .HasPartitionKey(u => u.PartitionKey);

        //AO: Defines the PK/unique identity of the docuemnts/objects
        modelBuilder.Entity<DigitalContent>()
            .HasKey(u => u.Id);
        
        modelBuilder.Entity<WorkoutProgram>()
            .Ignore(w => w.Workouts);

        //AO: Tells EF “ Når du gemmer objekter i DigitalContents-containeren, skal du gemme hvilken subtype objektet er." Gøres med udgangspunkt i typen
        modelBuilder.Entity<DigitalContent>()
            .HasDiscriminator<string>("DigitalContentType")
            .HasValue<WorkoutProgram>("WorkoutPrograms")
            .HasValue<WorkoutVideo>("WorkoutVideos");
    }
}