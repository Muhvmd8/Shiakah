using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class NotificationConfigurations : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasOne(n => n.ApplicationUser)
                  .WithMany(u => u.Notifications)
                  .HasForeignKey(n => n.ApplicationUserId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
