using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Models;
using ThunderServer.Models;

namespace ThunderServer.Infrastructure.Configurations;

public class ThunderFileShareConfiguration : IEntityTypeConfiguration<ThunderFileShare>
{
    public void Configure(EntityTypeBuilder<ThunderFileShare> builder)
    {
        builder.ToTable(nameof(ThunderFileShare));

        builder.HasKey(fs => fs.FileShareId);

        builder.Property(fs => fs.EncryptedFileKeyForRecipient)
               .IsRequired();

        builder.Property(fs => fs.SharedAt)
               .IsRequired();

        // Relationships
        builder.HasOne<ThunderFile>()
               .WithMany() // A file can have multiple shares
               .HasForeignKey(fs => fs.FileId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<ThunderUser>() // File owner
               .WithMany()
               .HasForeignKey(fs => fs.OwnerId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ThunderUser>() // Recipient of the file
               .WithMany()
               .HasForeignKey(fs => fs.RecipientId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}