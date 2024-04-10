using Hangfire;
using HangFireServiceAPI.BackgroundTasks;
using HangFireServiceAPI.DbContexts;
using HangFireServiceAPI.DTOs.Config;
using HangFireServiceAPI.Models.Config;
using HangFireServiceAPI.Repositories;
using HangFireServiceAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HangFireServiceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Binding config settings

            builder.Services.AddOptions<HangFireServiceSettings>()
                    .Bind(builder.Configuration.GetSection(nameof(HangFireServiceSettings)))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();

            builder.Services.AddOptions<ConnectionStrings>()
                .Bind(builder.Configuration.GetSection(nameof(ConnectionStrings)))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            #endregion

            #region Hang fire config

            builder.Services.AddHangfire((serviceProvider, config) =>
            {
                var dbSettings = serviceProvider.GetRequiredService<IOptionsMonitor<ConnectionStrings>>().CurrentValue;
                config.UseSqlServerStorage(dbSettings.HangFireDbConnection);
            });

            builder.Services.AddHangfireServer();

            #endregion

            builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var dbSettings = serviceProvider.GetRequiredService<IOptionsMonitor<ConnectionStrings>>().CurrentValue;
                options.UseSqlServer(dbSettings.DbConnection);
            });

            builder.Services.AddSingleton<IConfigService, ConfigService>();
            builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();

            builder.Services.AddTransient<ICountEmployeeDataJob, CountEmployeeDataJob>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseHangfireDashboard("/hangfire-dashboard");

            // Get the hang fire settings from configuration file
            var hangFireServiceSettings = app.Services.GetRequiredService<IOptionsMonitor<HangFireServiceSettings>>().CurrentValue;

            // Setting recurring job using hang fire
            RecurringJob.AddOrUpdate<ICountEmployeeDataJob>(nameof(ICountEmployeeDataJob.CountEmployeeDataAsync), s => s.CountEmployeeDataAsync(hangFireServiceSettings.FilePath), hangFireServiceSettings.CronExpression);

            app.Run();
        }
    }
}
