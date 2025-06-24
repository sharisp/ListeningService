using CSRedis;
using Infrastructure.SharedKernel;
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

            #region 注入redis
            string redisConnectionString = configuration["RedisConnection"];
            CSRedisClient cSRedisClient = new CSRedisClient(redisConnectionString);
            RedisHelper.Initialization(cSRedisClient);
            #endregion


            return services;
        }
    }
}
