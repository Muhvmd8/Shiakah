using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using Persistence.Repositories;
using Persistence.Seeding;

namespace Persistence
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            Console.WriteLine(configuration.GetConnectionString("DefaultConnection"));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IDataSeeding, DataSeeding>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<SuperAdminSeeder>();
            services.AddScoped<RolesSeeder>();
            services.AddScoped<AvatarSeeder>();

            return services;
        }
    }
}