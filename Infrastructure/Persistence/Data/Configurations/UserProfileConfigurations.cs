using Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {

        builder.Property(x => x.Gender)
            .HasConversion<int>();

        builder.Property(x => x.AgeRange)
            .HasConversion<int>();

        builder.Property(x => x.SkinTone)
            .HasConversion<int>();

        builder.Property(x => x.Height)
            .HasPrecision(5, 2);

        builder.Property(x => x.Weight)
            .HasPrecision(5, 2);

        builder.Property(x => x.BodyType)
            .HasConversion<int>();

        builder.Property(x => x.Name)
            .HasMaxLength(200);

        builder.Property(x => x.Location)
            .HasMaxLength(300);

        // UserProfile 1 -> 1 User
        builder.HasOne(x => x.ApplicationUser)
            .WithOne(x => x.UserProfile)
            .HasForeignKey<UserProfile>(x => x.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}