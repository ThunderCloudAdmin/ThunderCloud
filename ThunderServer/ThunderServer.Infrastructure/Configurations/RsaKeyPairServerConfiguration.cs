using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThunderServer.Models;

namespace ThunderServer.Infrastructure.Configurations;

class RsaKeyPairServerConfiguration : IEntityTypeConfiguration<RsaKeyPairServer>
{
    public void Configure(EntityTypeBuilder<RsaKeyPairServer> builder)
    {
        builder.ToTable(nameof(RsaKeyPairServer));

        builder.HasKey(fid => fid.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Ignore(x => x.PublicKey);

        builder.Property(x => x.PrivateKey).HasConversion<byte[]>();

        builder.Property(x => x.Salt);

        builder.HasOne(x => x.User)
            .WithOne(x => x.RsaKeyPairServer)
            .HasForeignKey<RsaKeyPairServer>(x => x.UserId);
    }
}