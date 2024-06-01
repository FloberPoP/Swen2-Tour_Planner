﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tour_Planner.BL;
using Tour_Planner.DAL;
using Tour_Planner.Models;
using Tour_Planner.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace UnitTests
{
    [TestClass]
    public class ICommandTests
    {
        private TourContext context; 
        private TourRepository repository;
        private ITourService _tourService;
        private TourPlannerVM _tourPlannerVM;

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<TourContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            context = new TourContext(options);
            repository = new TourRepository(context);
            _tourService = new TourService(repository);
            _tourPlannerVM = new TourPlannerVM(_tourService);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Database.EnsureDeleted(); 
            context.Dispose();

            var options = new DbContextOptionsBuilder<TourContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            context = new TourContext(options); 
        }

        [TestMethod]
        public void AddTourCommand_CanExecute_WhenValidInput_ReturnsTrue()
        {
            _tourPlannerVM.NewTourName = "Test";
            _tourPlannerVM.NewTourDescr = "Test Description";
            _tourPlannerVM.NewTourFrom = "From";
            _tourPlannerVM.NewTourTo = "To";
            _tourPlannerVM.NewTourTransType = TransportType.Car;
            _tourPlannerVM.NewTourDistance = 100;
            _tourPlannerVM.NewTourEstTime = 60;

            bool canExecute = _tourPlannerVM.AddTourCommand.CanExecute(null);

            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public void AddTourCommand_Execute_AddsNewTour()
        {
            int initialCount = _tourPlannerVM.Tours.Count;

            _tourPlannerVM.NewTourName = "Test";
            _tourPlannerVM.NewTourDescr = "Test Description";
            _tourPlannerVM.NewTourFrom = "From";
            _tourPlannerVM.NewTourTo = "To";
            _tourPlannerVM.NewTourTransType = TransportType.Car;
            _tourPlannerVM.NewTourDistance = 100;
            _tourPlannerVM.NewTourEstTime = 60;

            _tourPlannerVM.AddTourCommand.Execute(null);

            Assert.AreEqual(initialCount + 1, _tourPlannerVM.Tours.Count);
            Assert.IsTrue(_tourPlannerVM.Tours.Any(t => t.Name == "Test"));
        }

        [TestMethod]
        public void UpdateTourCommand_CanExecute_WhenTourSelected_ReturnsTrue()
        {
            if (_tourPlannerVM.Tours.Any())
            {
                _tourPlannerVM.SelectedTour = _tourPlannerVM.Tours.First();
            }

            bool canExecute = _tourPlannerVM.UpdateTourCommand.CanExecute(null);

            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public void UpdateTourCommand_Execute_UpdatesSelectedTour()
        {
            _tourPlannerVM.Tours.Add(new Tour
            {
                Name = "test_Name",
                Description = "test_Descr",
                From = "3910 Zwettl",
                To = "1200 Wien",
                TransportType = TransportType.Car,
                Distance = 0,
                EstimatedTime = 0,
                Img = "tour1.jpg"
            });

            var selectedTour = _tourPlannerVM.Tours.First();
            var newName = "Updated Test Name";
            selectedTour.Name = newName;

            _tourPlannerVM.SelectedTour = selectedTour;
            _tourPlannerVM.UpdateTourCommand.Execute(null);

            Assert.AreEqual("Updated Test Name", _tourPlannerVM.SelectedTour.Name);
        }

        [TestMethod]
        public void DeleteTourCommand_CanExecute_WhenTourSelected_ReturnsTrue()
        {
            if (_tourPlannerVM.Tours.Any())
            {
                _tourPlannerVM.SelectedTour = _tourPlannerVM.Tours.First();
            }

            bool canExecute = _tourPlannerVM.DeleteTourCommand.CanExecute(null);

            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public void DeleteTourCommand_Execute_RemovesSelectedTour()
        {
            _tourPlannerVM.Tours.Add(new Tour
            {
                Name = "test_Name",
                Description = "test_Descr",
                From = "3910 Zwettl",
                To = "1200 Wien",
                TransportType = TransportType.Car,
                Distance = 0,
                EstimatedTime = 0,
                Img = "tour1.jpg"
            });

            var selectedTour = _tourPlannerVM.Tours.First();
            _tourPlannerVM.SelectedTour = selectedTour;

            _tourPlannerVM.DeleteTourCommand.Execute(null);

            Console.WriteLine(_tourPlannerVM.Tours.Any());

            Assert.IsFalse(_tourPlannerVM.Tours.Any());
        }

        [TestMethod]
        public void AddTourLogCommand_CanExecute_WhenTourSelected_ReturnsTrue()
        {
            _tourPlannerVM.Tours.Add(new Tour
            {
                Name = "test_Name",
                Description = "test_Descr",
                From = "3910 Zwettl",
                To = "1200 Wien",
                TransportType = TransportType.Car,
                Distance = 0,
                EstimatedTime = 0,
                Img = "tour1.jpg"
            });

            if (_tourPlannerVM.Tours.Any())
            {
                _tourPlannerVM.SelectedTour = _tourPlannerVM.Tours.First();
            }

            bool canExecute = _tourPlannerVM.AddTourLogCommand.CanExecute(null);

            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public void AddTourLogCommand_Execute_AddsNewTourLog()
        {
            var test = new Tour
            {
                Name = "test_Name",
                Description = "test_Descr",
                From = "3910 Zwettl",
                To = "1200 Wien",
                TransportType = TransportType.Car,
                Distance = 0,
                EstimatedTime = 0,
                Img = "tour1.jpg"
            };

            _tourPlannerVM.Tours.Add(test);

            _tourPlannerVM.SelectedTour = test;

            int initialCount = _tourPlannerVM.SelectedTour.TourLogs.Count;

            var newTourLog = new TourLog
            {
                Tour = test,
                DateTime = DateTime.Now,
                Comment = "testlog_Comment",
                Difficulty = DifficultyLevel.Medium,
                TotalDistance = "5000",
                TotalTime = "3600",
                Rating = 5
            };

            _tourPlannerVM.TourLogsSelectedTour = test;
            _tourPlannerVM.SelectedTourLog = newTourLog;

            _tourPlannerVM.AddTourLogCommand.Execute(null);

            Assert.AreEqual(initialCount + 1, _tourPlannerVM.SelectedTour.TourLogs.Count);
        }

        [TestMethod]
        public void DeleteTourLogCommand_CanExecute_WhenTourLogSelected_ReturnsTrue()
        {
            if (_tourPlannerVM.SelectedTour != null && _tourPlannerVM.SelectedTour.TourLogs.Any())
            {
                _tourPlannerVM.SelectedTourLog = _tourPlannerVM.SelectedTour.TourLogs.First();
            }

            bool canExecute = _tourPlannerVM.DeleteTourLogCommand.CanExecute(null);

            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public void DeleteTourLogCommand_Execute_RemovesSelectedTourLog()
        {
            var test = new Tour
            {
                Name = "test_Name",
                Description = "test_Descr",
                From = "3910 Zwettl",
                To = "1200 Wien",
                TransportType = TransportType.Car,
                Distance = 0,
                EstimatedTime = 0,
                Img = "tour1.jpg"
            };

            _tourPlannerVM.Tours.Add(test);

            var newTourLog = new TourLog
            {
                Tour = test,
                DateTime = DateTime.Now,
                Comment = "testlog_Comment",
                Difficulty = DifficultyLevel.Medium,
                TotalDistance = "5000",
                TotalTime = "3600",
                Rating = 5
            };

            test.TourLogs.Add(newTourLog);

            _tourPlannerVM.SelectedTour = test;

            int initialCount = _tourPlannerVM.SelectedTour.TourLogs.Count;

            _tourPlannerVM.SelectedTourLog = newTourLog;
            _tourPlannerVM.TourLogsSelectedTour = test;

            _tourPlannerVM.DeleteTourLogCommand.Execute(null);

            Assert.AreEqual(initialCount - 1, _tourPlannerVM.SelectedTour.TourLogs.Count);
            Assert.IsFalse(_tourPlannerVM.SelectedTour.TourLogs.Contains(newTourLog));
        }

        [TestMethod]
        public void SaveTourLogCommand_CanExecute_WhenTourLogSelected_ReturnsTrue()
        {
            var test = new Tour
            {
                Name = "test_Name",
                Description = "test_Descr",
                From = "3910 Zwettl",
                To = "1200 Wien",
                TransportType = TransportType.Car,
                Distance = 0,
                EstimatedTime = 0,
                Img = "tour1.jpg"
            };

            _tourPlannerVM.Tours.Add(test);

            var newTourLog = new TourLog
            {
                Tour = test,
                DateTime = DateTime.Now,
                Comment = "testlog_Comment",
                Difficulty = DifficultyLevel.Medium,
                TotalDistance = "5000",
                TotalTime = "3600",
                Rating = 5
            };

            test.TourLogs.Add(newTourLog);

            // Check if there is a selected tour and if it has any logs
            if (_tourPlannerVM.SelectedTour != null && _tourPlannerVM.SelectedTour.TourLogs.Any())
            {
                // Set the selected tour log to the first log in the list
                _tourPlannerVM.SelectedTourLog = _tourPlannerVM.SelectedTour.TourLogs.First();
            }

            // Check if the SaveTourLogCommand can be executed
            bool canExecute = _tourPlannerVM.SaveTourLogCommand.CanExecute(null);

            // Assert that the command can be executed
            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public void SaveTourLogCommand_Execute_UpdatesSelectedTourLog()
        {
            var test = new Tour
            {
                Name = "test_Name",
                Description = "test_Descr",
                From = "3910 Zwettl",
                To = "1200 Wien",
                TransportType = TransportType.Car,
                Distance = 50,
                EstimatedTime = 50,
                Img = "tour1.jpg"
            };

            _tourPlannerVM.Tours.Add(test);

            var newTourLog = new TourLog
            {
                Tour = test,
                DateTime = DateTime.Now,
                Comment = "testlog_Comment",
                Difficulty = DifficultyLevel.Medium,
                TotalDistance = "5000",
                TotalTime = "3600",
                Rating = 5
            };

            test.TourLogs.Add(newTourLog);

            _tourPlannerVM.SelectedTour = test;
            _tourPlannerVM.TourLogsSelectedTour = test;
            _tourPlannerVM.SelectedTourLog = newTourLog;

            var newComment = "Updated test log comment";
            var newRating = 4;

            _tourPlannerVM.NewComment = newComment;
            _tourPlannerVM.NewRating = newRating;

            _tourPlannerVM.SaveTourLogCommand.Execute(null);

            Assert.AreEqual(newComment, _tourPlannerVM.SelectedTourLog.Comment);
            Assert.AreEqual(newRating, _tourPlannerVM.SelectedTourLog.Rating);
        }
    }
}

