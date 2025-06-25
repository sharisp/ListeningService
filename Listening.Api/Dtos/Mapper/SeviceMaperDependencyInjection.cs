using System.Reflection;

namespace Listening.Api.Dtos.Mapper
{
    public static class SeviceMaperDependencyInjection
    {
        public static void AddAllMapper(this IServiceCollection services)
        {
            var mapperTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IMapperService).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

            foreach (var type in mapperTypes)
            {
                services.AddScoped(type);
            }
            // Add other mappers as needed
        }
    }
}
