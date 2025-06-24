using Domain.SharedKernel;
using Domain.SharedKernel.Interfaces;
using Listening.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Listening.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomainCollection(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddDomainShardKernelCollection(configuration);
            services.AddScoped<EpisodeDomainService>();
            services.AddScoped<KindDomainService>();
            services.AddScoped<CategoryDomainService>();
            services.AddScoped<AlbumDomainService>();

            return services;
        }
    }
}
