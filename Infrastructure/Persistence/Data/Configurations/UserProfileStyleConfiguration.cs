using Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    internal class UserProfileStyleConfiguration
        : IEntityTypeConfiguration<UserProfileStyle>
    {
        public void Configure(EntityTypeBuilder<UserProfileStyle> builder)
        {
            builder.Property(x => x.Style)
                .HasConversion<int>()
                .IsRequired();

            builder.HasOne(x => x.UserProfile)
                .WithMany(x => x.Styles)
                .HasForeignKey(x => x.UserProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new
            {
                x.UserProfileId,
                x.Style
            })
            .IsUnique();
        }
    }
}
