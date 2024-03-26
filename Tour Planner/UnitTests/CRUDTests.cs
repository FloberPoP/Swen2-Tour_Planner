using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    [TestFixture]
    public class TourPlannerVMTests
    {
        private TourPlannerVM _tourPlannerVM;

        [SetUp]
        public void SetUp()
        {
            _tourPlannerVM = new TourPlannerVM();
        }

        [Test]
        public void AddTour_AddsTourToToursCollection()
        {
            // Arrange
            int initialCount = _tourPlannerVM.Tours.Count;

            // Act
            _tourPlannerVM.AddTour();

            // Assert
            Assert.AreEqual(initialCount + 1, _tourPlannerVM.Tours.Count);
        }

        [Test]
        public void UpdateTour_UpdatesSelectedTourProperties()
        {
            // Arrange
            var selectedTour = new Tour { Name = "TestTour" };
            _tourPlannerVM.SelectedTour = selectedTour;

            // Act
            _tourPlannerVM.UpdateTour();

            // Assert
            Assert.AreEqual(_tourPlannerVM.NewTourName, selectedTour.Name);
            // Add assertions for other properties as needed
        }

        [Test]
        public void DeleteTour_RemovesSelectedTourFromToursCollection()
        {
            // Arrange
            var tourToRemove = new Tour { Name = "TestTour" };
            _tourPlannerVM.Tours.Add(tourToRemove);
            _tourPlannerVM.SelectedTour = tourToRemove;

            // Act
            _tourPlannerVM.DeleteTour();

            // Assert
            Assert.IsFalse(_tourPlannerVM.Tours.Contains(tourToRemove));
        }

        [Test]
        public void AddTourLog_AddsTourLogToSelectedTourLogsCollection()
        {
            // Arrange
            var selectedTour = new Tour { Name = "TestTour" };
            _tourPlannerVM.Tours.Add(selectedTour);
            _tourPlannerVM.TourLogsSelectedTour = selectedTour;
            int initialCount = selectedTour.TourLogs.Count;

            // Act
            _tourPlannerVM.AddTourLog();

            // Assert
            Assert.AreEqual(initialCount + 1, selectedTour.TourLogs.Count);
        }
    }
}
