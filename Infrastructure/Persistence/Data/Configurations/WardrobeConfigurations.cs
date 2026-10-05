using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Persistence.Data.Configurations
{
    public class WardrobeConfiguration : IEntityTypeConfiguration<Wardrobe>
    {
        public void Configure(EntityTypeBuilder<Wardrobe> builder)
        {
            builder.Property(x => x.ApplicationUserId)
                .IsRequired();

            // ApplicationUser 1 : 1 Wardrobe
            builder.HasOne(x => x.User)
                .WithOne(x => x.Wardrobe)
                .HasForeignKey<Wardrobe>(x => x.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ensure that each user can have only one wardrobe
            builder.HasIndex(x => x.ApplicationUserId)
                .IsUnique();

            // Wardrobe 1 : N WardrobeItem
            builder.HasMany(x => x.Items)
                .WithOne(x => x.Wardrobe)
                .HasForeignKey(x => x.WardrobeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
