using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    internal class WardrobeItemConfiguration
        : IEntityTypeConfiguration<WardrobeItem>
    {
        public void Configure(
            EntityTypeBuilder<WardrobeItem> builder)
        {
            builder.Property(x => x.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            // WardrobeItem -> ItemAttribute
            builder.OwnsOne(
                x => x.Attributes,
                attributes =>
                {
                    attributes.Property(x => x.Category)
                        .HasMaxLength(100);

                    attributes.Property(x => x.Color)
                        .HasMaxLength(100);

                    attributes.Property(x => x.Fit)
                        .HasMaxLength(100);

                    attributes.Property(x => x.Style)
                        .HasMaxLength(100);

                    attributes.Property(x => x.Season)
                        .HasMaxLength(100);

                    attributes.Property(x => x.Pattern)
                        .HasMaxLength(100);
                });
        }
    }
}
