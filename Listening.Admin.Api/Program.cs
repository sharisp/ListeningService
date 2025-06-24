
using Common.Jwt;
using Domain.SharedKernel.Interfaces;
using FluentValidation;
using Infrastructure.SharedKernel;
using Listening.Admin.Api.Controllers;
using Listening.Admin.Api.Middlewares;
using Listening.Domain;
using Listening.Infrastructure;
using Listening.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                //configure json options,long type bug
                options.JsonSerializerOptions.NumberHandling =
                    System.Text.Json.Serialization.JsonNumberHandling.WriteAsString
                    | System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
            }); 
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
              typeof(IUnitOfWork).Assembly,
              typeof(AlbumController).Assembly //
          ));
            // 如果是多个DB，这儿可以改成 自定义的DBContext, 继承于DbContext即可
            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
            });
            builder.Services.AddScoped<BaseDbContext>(sp => sp.GetRequiredService<AppDbContext>());

            builder.Services.AddHttpContextAccessor(); //for accessing HttpContext in services IHttpContextAccessor
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
