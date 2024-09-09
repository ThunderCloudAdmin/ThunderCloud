using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Models;
using ThunderServer.Models;

namespace ThunderServer.Infrastructure.Configurations;

public class FileInDirectoryConfiguration : IEntityTypeConfiguration<FileInDirectory>
{
    public void Configure(EntityTypeBuilder<FileInDirectory> builder)
    {
        builder.ToTable(nameof(FileInDirectory));

        builder.HasKey(fid => fid.FileInDirectoryId);

        // Relationships
        builder.HasOne<ThunderFile>()
               .WithMany() // A file can belong to multiple directories
               .HasForeignKey(fid => fid.FileId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<ThunderFolder>()
               .WithMany() // A directory can have many files
               .HasForeignKey(fid => fid.DirectoryId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}