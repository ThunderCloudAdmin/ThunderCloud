using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThunderServer.Models;

namespace ThunderServer.Infrastructure.Configurations;

public class ThunderFolderServerConfiguration : IEntityTypeConfiguration<ThunderFolderServer>
{
    public void Configure(EntityTypeBuilder<ThunderFolderServer> builder)
    {
        builder.ToTable(nameof(ThunderFolderServer));

        builder.HasKey(d => d.Id);

  //      builder.Property(d => d.Name)
  //             .IsRequired()
  //             .HasMaxLength(255);

  //      builder.Property(d => d.CreatedAt)
  //             .IsRequired();

		//builder.Property(d => d.IsRootFolder);

		//// Self-referencing relationship for subdirectories
		//builder.HasOne<ThunderFolder>()
  //             .WithMany()
  //             .HasForeignKey(d => d.ParentFolderId)
  //             .OnDelete(DeleteBehavior.Restrict);

  //      // Relationships
  //      builder.HasOne<ThunderUser>() // Directory owner
  //             .WithMany()
  //             .HasForeignKey(d => d.Id)
  //             .OnDelete(DeleteBehavior.Cascade);

		//builder.HasOne<ThunderUser>() // Directory owner
	 //  .WithMany()
	 //  .HasForeignKey(d => d.Id)
	 //  .OnDelete(DeleteBehavior.Cascade);
	}
}