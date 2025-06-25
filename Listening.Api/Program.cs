
using Common.Jwt;
using Domain.SharedKernel.Interfaces;
using FluentValidation;
using Identity.Api.ActionFilter;
using Infrastructure.SharedKernel;
using Listening.Api.Dtos;
using Listening.Api.Dtos.Mapper;
using Listening.Api.Helpers;
using Listening.Api.Middlewares;
using Listening.Domain.Entities;
using Listening.Infrastructure;
using Listening.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;

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