using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Tour_Planner.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;


namespace Tour_Planner.DAL
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TourContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
        }


        private void SeedData(TourContext context)
        {
            // Seed Tours table
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

            // Seed TourLogs table
            if (!context.TourLogs.Any())
            {
                var tour = context.Tours.First(); // Get the first tour for demonstration purposes

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
