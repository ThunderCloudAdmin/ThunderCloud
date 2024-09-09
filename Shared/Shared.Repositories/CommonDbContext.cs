using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Shared.Repositories;

public class CommonDbContext : DbContext
{
    public DbSet<EncryptionKey> EncryptionKeys { get; set; }
    public object Directories { get; internal set; }

    public CommonDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder
        //.ApplyConfiguration(new FileConfiguration())
        //.ApplyConfiguration(new FileShareConfiguration())
        //.ApplyConfiguration(new DirectoryConfiguration())
        //.ApplyConfiguration(new FileInDirectoryConfiguration())
        //.ApplyConfiguration(new TagConfiguration())
        //.ApplyConfiguration(new FileTagConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configure PostgreSQL as the database provider
        optionsBuilder.UseNpgsql("YourPostgresConnectionString");
    }
}
