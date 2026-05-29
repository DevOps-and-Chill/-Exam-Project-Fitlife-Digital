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
    public DbSet<Message> SystemMessages => Set<Message>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DirectMessage>()
            .ToContainer("DirectMessages")
            .HasPartitionKey(u => u.PartitionKey)
            .HasKey(u => u.Id);

        modelBuilder.Entity<Message>()
            .ToContainer("SystemMessages")
            .HasPartitionKey(u => u.PartitionKey)
            .HasKey(u => u.Id);
    }
}