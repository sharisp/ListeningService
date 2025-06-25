using Listening.Admin.Api.Middlewares;
using Listening.Infrastructure;

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

            var app = builder.Build();

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

            app.Run();
        }
    }
}
