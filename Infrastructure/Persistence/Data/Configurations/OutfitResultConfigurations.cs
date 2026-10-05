namespace Persistence.Data.Configurations
{
    using Domain.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class OutfitResultConfiguration
        : IEntityTypeConfiguration<OutfitResult>
    {
        public void Configure(
            EntityTypeBuilder<OutfitResult> builder)
        {
            builder.Property(x => x.ApplicationUserId)
                .IsRequired();

            builder.Property(x => x.Top)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Bottom)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Footwear)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Accessories)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.RecommendedImageUrl)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.SearchQuery)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.StylingTips)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(x => x.Summary)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.OutfitResults)
                .HasForeignKey(x => x.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.ApplicationUserId);
        }
    }
}
