using Listening.Api.Dtos.Mapper;
using Listening.Api.Helpers;
using Listening.Api.Middlewares;
using Listening.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace Listening.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCommonApiCollection(builder.Configuration);
            // Add services to the container.
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<MemoryCacheHelper>();
            builder.Services.AddAllMapper();
            var app = builder.Build();

            app.UseCors("AllowAll");
            app.UseMiddleware<CustomerExceptionMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.Urls.Add($"http://*:5001");
            }
            /*   else
               {
                   var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
                   app.Urls.Add($"http://*:{port}");

               }*/

            app.MapGet("/", [AllowAnonymous]()  => "Hello from User Listen!");
         //   app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}