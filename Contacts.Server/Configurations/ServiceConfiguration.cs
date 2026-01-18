using Contacts.Server.Repositories;
using Contacts.Server.Repositories.Interfaces;
using Contacts.Server.Services;
using Contacts.Server.Services.Interfaces;
using Contacts.Server.Validators;
using Microsoft.AspNetCore.Identity;

namespace Contacts.Server.Configurations
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IPhoneNumberService, PhoneNumberService>();
            services.AddScoped<ContactValidator>();

            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IPhoneNumberRepository, PhoneNumberRepository>();

            return services;
        }
    }
}
