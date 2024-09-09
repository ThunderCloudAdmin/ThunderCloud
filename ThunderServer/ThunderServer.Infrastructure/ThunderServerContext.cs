using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using ThunderServer.Infrastructure.Configurations;
using ThunderServer.Models;

namespace ThunderServer.Infrastructure;

public class ThunderServerContext : IdentityDbContext<ThunderUser, IdentityRole<Guid>, Guid>
{
    public DbSet<ThunderFile> Files { get; set; }
    public DbSet<ThunderFileShare> FileShares { get; set; }
    //public DbSet<ThunderFolderServer> ThunderFolders { get; set; }
    public DbSet<FileInDirectory> FilesInDirectories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<ThunderFileTag> FileTags { get; set; }

    public ThunderServerContext(DbContextOptions<ThunderServerContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
        .ApplyConfiguration(new ThunderFileConfiguration())
        .ApplyConfiguration(new ThunderFileShareConfiguration())
        //.ApplyConfiguration(new ThunderFolderServerConfiguration())
        .ApplyConfiguration(new FileInDirectoryConfiguration())
        .ApplyConfiguration(new TagConfiguration())
        .ApplyConfiguration(new ThunderFileTagConfiguration());
    }
}
