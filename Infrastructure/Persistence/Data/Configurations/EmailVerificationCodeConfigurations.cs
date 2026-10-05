using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data.Configurations
{
    public class EmailVerificationCodeConfigurations:IEntityTypeConfiguration<EmailVerificationCode>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<EmailVerificationCode> builder)
        {
            builder.HasOne(e => e.ApplicationUser)
           .WithOne(u => u.EmailVerificationCode)
           .HasForeignKey<EmailVerificationCode>(e => e.ApplicationUserId)
           .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
