using log4net.Config;
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
using Tour_Planner.DAL;

namespace Tour_Planner
{
    public partial class App : Application
    {
        private IHost _host;
        public IConfiguration Configuration { get; }

        public App()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("confsettings\\appsettings.json", optional: false, reloadOnChange: true);

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

            var log4netConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Configuration["log4net:fileName"]);
            var log4netConfig = new FileInfo(log4netConfigFilePath);
            XmlConfigurator.ConfigureAndWatch(log4netConfig);
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
                    Name = "Mittagspause",
                    Description = "Tour von da HTL zum Mci",
                    From = "Hammerweg 1, 3910 Zwettl",
                    To = "Andre Freyskorn Str. 2, 3910 Zwettl",
                    TransportType = TransportType.Car,
                    Distance = 0,
                    EstimatedTime = 0,
                    Img = "tour1.jpg"
                });

                context.Tours.Add(new Tour
                {
                    Name = "Arbeitsweg",
                    Description = "Jürgens Arbeitsweg im Sommer",
                    From = "Gröblingerstraße 366, 3920 Groß Gerungs",
                    To = "Kreuzberg 107, 3920 Groß Gerungs",
                    TransportType = TransportType.Car,
                    Distance = 0,
                    EstimatedTime = 0,
                    Img = "tour2.jpg"
                });

                context.Tours.Add(new Tour
                {
                    Name = "Einkaufstour",
                    Description = "Shoppen zum Pfeiffer-Vogl idk",
                    From = "Arbesbach 224",
                    To = "Arbesbach 64",
                    TransportType = TransportType.Walk,
                    Distance = 0,
                    EstimatedTime = 0,
                    Img = "tour2.jpg"
                });

                context.Tours.Add(new Tour
                {
                    Name = "FH-Weg",
                    Description = "Meine Wohnung zur FH",
                    From = "Aignerstraße 6, 1200 Wien",
                    To = "Höchstädtpl. 6, 1200 Wien",
                    TransportType = TransportType.Walk,
                    Distance = 0,
                    EstimatedTime = 0,
                    Img = "tour2.jpg"
                });

                context.Tours.Add(new Tour
                {
                    Name = "LOCO-SQUAD",
                    Description = "GEMMA LOCO?!",
                    From = "Aignerstraße 6, 1200 Wien",
                    To = "Loco-Bar",
                    TransportType = TransportType.Car,
                    Distance = 0,
                    EstimatedTime = 0,
                    Img = "tour2.jpg"
                });

                context.SaveChanges();
            }
        }
    }
}