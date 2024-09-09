using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Models;
using ThunderServer.Models;

namespace ThunderServer.Infrastructure.Configurations;

public class FileTagConfiguration : IEntityTypeConfiguration<FileTag>
{
    public void Configure(EntityTypeBuilder<FileTag> builder)
    {
        builder.ToTable(nameof(FileTag));

        builder.HasKey(ft => ft.FileTagId);

        // Relationships
        builder.HasOne<ThunderFile>()
               .WithMany() // A file can have multiple tags
               .HasForeignKey(ft => ft.FileId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Tag>()
               .WithMany() // A tag can belong to multiple files
               .HasForeignKey(ft => ft.TagId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}