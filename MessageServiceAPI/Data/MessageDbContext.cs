using Microsoft.EntityFrameworkCore;
using System.Data;
using MessageServiceAPI.Models;
using Microsoft.EntityFrameworkCore.Cosmos;

namespace MessageServiceAPI.Data;

public class MessageDbContext : DbContext
{
    public MessageDbContext(DbContextOptions<MessageDbContext> options) : base(options)
    {
    }
    public DbSet<DirectMessage> DirectMessages => Set<DirectMessage>();
    public DbSet<ClassMessage> ClassMessages => Set<ClassMessage>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DirectMessage>()
            .ToContainer("DirectMessages");
        modelBuilder.Entity<DirectMessage>()
            .HasPartitionKey(u => u.PartitionKey);
        modelBuilder.Entity<DirectMessage>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<ClassMessage>()
            .ToContainer("ClassMessages");
        modelBuilder.Entity<ClassMessage>()
            .HasPartitionKey(u => u.PartitionKey);
        modelBuilder.Entity<ClassMessage>()
            .HasKey(u => u.Id);
    }
}