using Domain.Models;
using Domain.Models.Security;
using Domain.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<LoginAudit> LoginAudits { get; set; }
        public DbSet<BlockedIp> BlockedIps { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<EmailVerificationCode> EmailVerificationCodes { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyReference).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
