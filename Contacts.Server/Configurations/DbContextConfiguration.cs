using Microsoft.EntityFrameworkCore;

namespace Contacts.Server.Configurations
{
    public static class DbContextExtensions
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new Exception("Connection string is missing!");

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}
