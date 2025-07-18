﻿using CSRedis;
using Infrastructure.SharedKernel;
using Listening.Domain;
using Listening.Domain.Interfaces;
using Listening.Infrastructure.Helper;
using Listening.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Listening.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddInfrastructureKernelCollection(configuration);
            services.AddDomainCollection(configuration);
            #region 注入redis
            string redisConnectionString = configuration["RedisConnection"];
            CSRedisClient cSRedisClient = new CSRedisClient(redisConnectionString);
            RedisHelper.Initialization(cSRedisClient);
            #endregion

            services.AddScoped<AppDbContext>();
            //special
            services.AddHttpClient<ApiClientHelper>();
            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IKindRepository, KindRepository>();
            services.AddScoped<IEpisodeRepository, EpisodeRepository>();
            return services;
        }
    }
}
