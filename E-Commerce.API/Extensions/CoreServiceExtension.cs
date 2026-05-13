using Service;
using Service.Abstractions;
using Service.MappingProfiles;
using Shared.IdentityDtos;

namespace E_Commerce.API.Extensions
{
    public static class CoreServiceExtension
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddAutoMapper(cfg => cfg.AddProfile<ProductProfile>());
            services.AddAutoMapper(cfg => cfg.AddProfile<BasketProfile>());
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

            return services;
        }
    }
}
