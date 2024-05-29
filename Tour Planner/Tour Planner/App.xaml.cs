using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using Tour_Planner.BL;
using Tour_Planner.Models;

namespace Tour_Planner.DAL
{
    public partial class App : Application
    {
        private IHost _host;
        public IConfiguration Configuration { get; }

        public App()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings\\appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<TourContext>(options =>
                        options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
                    services.AddScoped<ITourRepository, TourRepository>();
                    services.AddScoped<ITourService, TourService>();
                    services.AddSingleton<MainWindow>();

                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            SeedDatabase();
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.StopAsync().Wait();
            base.OnExit(e);
        }

        private void SeedDatabase()
        {
            using var scope = _host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TourContext>();
            if (context.Database.EnsureCreated())
            {
                SeedData(context);
            }
        }

        private void SeedData(TourContext context)
        {
            if (!context.Tours.Any())
            {
                context.Tours.Add(new Tour
                {
                    Name = "Tour 1",
                    Description = "Description of Tour 1",
                    From = "Start Location",
                    To = "End Location",
                    TransportType = "Transport Type",
                    Distance = 100,
                    EstimatedTime = 120,
                    Img = "tour1.jpg"
                });

                context.Tours.Add(new Tour
                {
                    Name = "Tour 2",
                    Description = "Description of Tour 2",
                    From = "Start Location",
                    To = "End Location",
                    TransportType = "Transport Type",
                    Distance = 200,
                    EstimatedTime = 230,
                    Img = "tour2.jpg"
                });

                context.SaveChanges();
            }

            if (!context.TourLogs.Any())
            {
                var tour = context.Tours.First();

                context.TourLogs.Add(new TourLog
                {
                    Tour = tour,
                    DateTime = DateTime.Now,
                    Comment = "First tour log",
                    Difficulty = "Medium",
                    TotalDistance = "50 km",
                    TotalTime = "2 hours",
                    Rating = 4
                });

                context.SaveChanges();
            }
        }
    }
}
