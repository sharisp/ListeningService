using Listening.Admin.Api.Middlewares;
using Listening.Infrastructure;
using Listening.Infrastructure.Helper;
using Microsoft.AspNetCore.Authorization;

namespace Listening.Admin.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCommonApiCollection(builder.Configuration);
            // Add services to the container.
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddScoped<PermissionCheckHelper>();
            var app = builder.Build();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseMiddleware<CustomerExceptionMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                app.Urls.Add($"http://*:5017");
            }
         /*   else
            {
                var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
                app.Urls.Add($"http://*:{port}");

               
            }*/

          //  app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/", [AllowAnonymous] () => "Hello from Listen Admin!");
            app.MapControllers();

            app.UseMiddleware<CustomPermissionCheckMiddleware>();
            app.Run();
        }
    }
}
