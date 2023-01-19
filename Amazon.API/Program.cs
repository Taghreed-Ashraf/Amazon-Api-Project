using Amazon.API.Extentions;
using Amazon.API.MiddleWares;
using Amazon.Core.Entities.Identity;
using Amazon.Repository.Data;
using Amazon.Repository.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amazon.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services From Startup
            // Add services to the container.

            builder.Services.AddControllers(); // ASP Web APIs

            // Store DbContext
            builder.Services.AddDbContext<StoreContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Identity DbContext
            builder.Services.AddDbContext<AppIdentityDbContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

            // Redis Db
            builder.Services.AddSingleton<IConnectionMultiplexer>(S =>
            {
                var connection = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));

                return ConnectionMultiplexer.Connect(connection);
            });

            builder.Services.AddApplicationServices();
            builder.Services.AddSwaggerServices();
            builder.Services.AddIdentityServices(builder.Configuration);


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });

            #endregion

            var app = builder.Build();


            #region Apply Migration And DataSedding

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var context = services.GetRequiredService<StoreContext>();
                await context.Database.MigrateAsync();

                await StoreContextSeed.SeedAsync(context, loggerFactory);

                var Identitycontext = services.GetRequiredService<AppIdentityDbContext>();
                await Identitycontext.Database.MigrateAsync();

                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUsersAsync(userManager);

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                //logger.LogError(ex, "an Error occured during Apply Migration");
                logger.LogError(ex, ex.Message);
            }
            #endregion



            #region Configure HTTP request pipeline
            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExeptionMiddleWare>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerDocumention();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            #endregion

            app.Run();
        }
    }
}
