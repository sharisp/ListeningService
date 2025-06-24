
using Common.Jwt;
using FluentValidation;
using Listening.Admin.Api.Middlewares;
using Listening.Domain;
using Listening.Infrastructure;
using System.Reflection;

namespace Listening.Admin.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddJWTAuthentication(builder.Configuration);
           // builder.Services.AddDomainCollection(builder.Configuration);
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
