using Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APP.Infrastructure.Persistence.Configurations;

internal class UserProfilePreferredColorConfiguration
    : IEntityTypeConfiguration<UserProfilePreferredColor>
{
    public void Configure(EntityTypeBuilder<UserProfilePreferredColor> builder)
    {
        builder.Property(x => x.Color)
            .HasConversion<int>()
            .IsRequired();

        builder.HasOne(x => x.UserProfile)
            .WithMany(x => x.PreferredColors)
            .HasForeignKey(x => x.UserProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new
        {
            x.UserProfileId,
            x.Color
        })
        .IsUnique();
    }
}