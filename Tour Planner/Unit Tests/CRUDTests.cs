﻿using Tour_Planner;
using Tour_Planner.ViewModels;

namespace UnitTests
{
    [TestClass]
    public class TourPlannerVMTests
    {
        private TourPlannerVM tourPlannerVM;

        [TestInitialize]
        public void SetUp()
        {
            tourPlannerVM = new TourPlannerVM();
        }

        [TestMethod]
        public void AddTour()
        {
            int initialCount = tourPlannerVM.Tours.Count;

            tourPlannerVM.AddTour();

            Assert.AreEqual(initialCount + 1, tourPlannerVM.Tours.Count);
        }

        [TestMethod]
        public void UpdateTour()
        {
            var selectedTour = new Tour { Name = "TestTour" };

            tourPlannerVM.SelectedTour = selectedTour;
            tourPlannerVM.UpdateTour();

            Assert.AreEqual(tourPlannerVM.NewTourName, selectedTour.Name);
        }

        [TestMethod]
        public void DeleteTour()
        {
            var tourToRemove = new Tour { Name = "TestTour" };

            tourPlannerVM.Tours.Add(tourToRemove);
            tourPlannerVM.SelectedTour = tourToRemove;
            tourPlannerVM.DeleteTour();

            Assert.IsFalse(tourPlannerVM.Tours.Contains(tourToRemove));
        }

        [TestMethod]
        public void AddTourLog()
        {
            var selectedTour = new Tour { Name = "TestTour" };
            tourPlannerVM.Tours.Add(selectedTour);
            tourPlannerVM.TourLogsSelectedTour = selectedTour;
            int initialCount = selectedTour.TourLogs.Count;

            tourPlannerVM.AddTourLog();

            Assert.AreEqual(initialCount + 1, selectedTour.TourLogs.Count);
        }

        [TestMethod]
        public void UpdateTourLog()
        {
            var selectedTour = new Tour { Name = "TestTour" };
            var selectedTourLog = new TourLog { tour = selectedTour, DateTime = DateTime.Now, Comment = "TestComment" };
            tourPlannerVM.TourLogsSelectedTour = selectedTour;
            tourPlannerVM.SelectedTourLog = selectedTourLog;

            tourPlannerVM.SaveTourLog();

            Assert.AreEqual(selectedTourLog.DateTime, tourPlannerVM.NewDateTime);
            Assert.AreEqual(selectedTourLog.Comment, tourPlannerVM.NewComment);
        }

        [TestMethod]
        public void DeleteTourLog()
        {
            var selectedTour = new Tour { Name = "TestTour" };
            var tourLogToDelete = new TourLog { tour = selectedTour, DateTime = DateTime.Now, Comment = "TestComment" };
            tourPlannerVM.TourLogsSelectedTour = selectedTour;
            tourPlannerVM.SelectedTourLog = tourLogToDelete;
            tourPlannerVM.AddTourLog();

            tourPlannerVM.DeleteTourLog();

            Assert.IsFalse(tourPlannerVM.TourLogsSelectedTour.TourLogs.Contains(tourLogToDelete));
        }
    }
}
