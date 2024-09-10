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
        builder.Ignore(x => x.PrivateKey);

        builder.Property(x => x.EncryptedPrivateRsaKey);

        builder.HasOne(x => x.User)
            .WithOne(x => x.RsaKeyPairServer)
            .HasForeignKey<RsaKeyPairServer>(x => x.UserId);

        builder.HasOne(x => x.Argon2Key)
            .WithOne(x => x.RsaKeyPairServer)
            .HasForeignKey<RsaKeyPairServer>(x => x.Id);
    }
}