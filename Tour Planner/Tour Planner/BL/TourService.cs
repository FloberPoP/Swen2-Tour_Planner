using Tour_Planner.DAL;
using Tour_Planner.Models;

namespace Tour_Planner.BL
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepository;

        public TourService(ITourRepository tourRepository)
        {
            _tourRepository = tourRepository;
        }

        public IEnumerable<Tour> GetAllTours()
        {
            return _tourRepository.GetAllTours();
        }

        public void AddTour(Tour tour)
        {
            _tourRepository.AddTour(tour);
        }    

        // Implement other methods for update, delete, etc.
    }
}
