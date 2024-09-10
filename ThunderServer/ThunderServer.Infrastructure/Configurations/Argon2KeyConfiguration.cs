using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThunderServer.Models;

namespace ThunderServer.Infrastructure.Configurations;

internal class Argon2KeyConfiguration : IEntityTypeConfiguration<Argon2Key>
{
    public void Configure(EntityTypeBuilder<Argon2Key> builder)
    {
        builder.ToTable(nameof(Argon2Key));

        builder.HasKey(fid => fid.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Ignore(x => x.DerivedKey);

        builder.Property(x => x.Salt);
        builder.Property(x => x.Parallelism);
        builder.Property(x => x.Iterations);
        builder.Property(x => x.MemorySize);
    }
}