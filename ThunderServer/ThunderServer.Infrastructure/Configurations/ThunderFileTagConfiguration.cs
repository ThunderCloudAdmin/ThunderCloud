using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Models;

namespace ThunderServer.Infrastructure.Configurations;

public class ThunderFileTagConfiguration : IEntityTypeConfiguration<ThunderFileTag>
{
    public void Configure(EntityTypeBuilder<ThunderFileTag> builder)
    {
        builder.ToTable(nameof(ThunderFileTag));

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