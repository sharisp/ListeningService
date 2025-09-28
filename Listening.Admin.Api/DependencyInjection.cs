using Common.Jwt;
using Domain.SharedKernel.Interfaces;
using FluentValidation;
using Infrastructure.SharedKernel;
using Listening.Admin.Api.ActionFilter;
using Listening.Admin.Api.Helpers;
using Listening.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Listening.Admin.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCommonApiCollection(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<UnitOfWorkActionFilter>();
            services.AddScoped<CommonQuery>();
            services.AddControllers(options =>
            {
                options.Filters.AddService<UnitOfWorkActionFilter>();
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>

                     new BadRequestObjectResult(ApiResponse<string>.Fail("param error"));

            }).AddJsonOptions(options =>
            {
                //configure json options,long type bug
                options.JsonSerializerOptions.NumberHandling =
                    System.Text.Json.Serialization.JsonNumberHandling.WriteAsString
                    | System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
              typeof(IUnitOfWork).Assembly,
              Assembly.GetExecutingAssembly()
             // typeof(AlbumController).Assembly //
          ));
         

          //  services.AddScoped<RowPermissionInterceptor>();

            services.AddDbContext<AppDbContext>((sp, opt) =>
            {
               // var interceptor = sp.GetRequiredService<RowPermissionInterceptor>();
                opt.UseSqlServer(configuration.GetConnectionString("SqlServer"));
                  // .AddInterceptors(interceptor);
            });

            services.AddScoped<BaseDbContext>(sp => sp.GetRequiredService<AppDbContext>());
         
            services.AddHttpContextAccessor(); //for accessing HttpContext in services IHttpContextAccessor
            services.AddSwaggerGen();
            services.AddJWTAuthentication(configuration);
            // builder.Services.AddDomainCollection(builder.Configuration);
            services.AddMemoryCache();
            services.AddScoped<MemoryCacheHelper>();

            return services;
        }
    }
}
