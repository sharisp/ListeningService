using Listening.Admin.Api.Middlewares;
using Listening.Infrastructure;
using Listening.Infrastructure.Helper;

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

            app.UseCors("AllowAll");
            app.UseMiddleware<CustomerExceptionMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseMiddleware<CustomPermissionCheckMiddleware>();
            app.Run();
        }
    }
}
