using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tour_Planner.Models;
using Tour_Planner.BL;
using log4net;
using Tour_Planner.DAL;
using System.Windows;
using Tour_Planner.BL.GeoLocationAPI;
using iTextSharp.text.pdf.codec;
using System.Windows.Media.Imaging;
using static Tour_Planner.BL.GeoLocationAPI.CalculatedRouteResponse;
using System.IO;
using System.Text;
using Microsoft.Web.WebView2.WinForms;

namespace Tour_Planner.ViewModels
{
    public class TourPlannerVM : INotifyPropertyChanged
    {
        private readonly ITourService _tourService;
        private readonly RouteService _routeService;
        private static readonly ILog Log = LogManager.GetLogger(typeof(App));

        public TourPlannerVM(ITourService tourService)
        {
            AddTourCommand = new RelayCommand(o => AddTour());
            UpdateTourCommand = new RelayCommand(o => UpdateTour());
            DeleteTourCommand = new RelayCommand(o => DeleteTour());

            AddTourLogCommand = new RelayCommand(o => AddTourLog());
            DeleteTourLogCommand = new RelayCommand(o => DeleteTourLog());
            SaveTourLogCommand = new RelayCommand(o => SaveTourLog());

            TourReportCommand = new RelayCommand(o => { TourReport(); ShowPopup("Information for Single-Tour-Report"); });
            SummarizedTourReportCommand = new RelayCommand(o => { SummarizedTourReport(); ShowPopup("Information for Summarized Tour Report"); });
            ExportTourDataCommand = new RelayCommand(o => { ExportTourData(); ShowPopup("Information for Export Tour Data"); });
            ImportTourDataCommand = new RelayCommand(filePath => ImportTourData(filePath));

            _tourService = tourService;
            _routeService = new RouteService();

            NewDateTime = DateTime.Now;
            LoadTours();
        }

        private void ShowPopup(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private string imageUrl;
        public string ImageUrl
        {
            get
            {
                return imageUrl;
            }
            set
            {
                imageUrl = value;
                OnPropertyChanged();
            }
        }

        private async void GetRouteInfo()
        {
            if (SelectedTour != null)
            {
                var (responseBody, routeResponse) = await _routeService.GetRoute(SelectedTour.From, SelectedTour.To, SelectedTour.TransportType);

                string filePath = "Resources/directions.js";

                string newJsonDirection = "var directions = " + responseBody.ToString();

                // Override the directions.js-File
                try
                {
                    await File.WriteAllTextAsync(filePath, newJsonDirection, Encoding.UTF8);
                }

                catch (IOException e)
                {
                    Console.WriteLine($"Fehler beim Schreiben der Datei: {e.Message}");
                }

                if (routeResponse != null && routeResponse.Features != null && routeResponse.Features.Count > 0)
                {
                    var firstFeature = routeResponse.Features[0];
                    var coordinates = firstFeature.Geometry.Coordinates;
                    var summary = firstFeature.Properties.Summary;

                    //string message = $"Distance: {summary.Distance} meters\nDuration: {summary.Duration} seconds";
                    //MessageBox.Show(message);

                    NewTourDistance = (int)summary.Distance;
                    NewTourEstTime = (int)summary.Duration;

                    //ImageUrl = await ConstructMapUrl(SelectedTour.From, SelectedTour.To);
                    //SelectedTour.Img = ImageUrl;
                }

                else
                {
                    NewTourDistance = 0;
                    NewTourEstTime = 0;
                }
            }
        }

        /*
        // Construct the map with the Long-Lat-Coords
        // Basically only needed for Single-Report-Generation ("Image")
        private async Task<string> ConstructMapUrl(string address1, string address2)
        {
            var startCoordinates = await _routeService.GetCoordinates(address1);
            var endCoordinates = await _routeService.GetCoordinates(address2);

            string baseUrl = "https://tile.openstreetmap.org/{0}/{1}/{2}.png";
            int zoomLevel = 14;

            // Calculate the center coordinates between the start and end points
            double centerLat = (startCoordinates.Latitude + endCoordinates.Latitude) / 2;
            double centerLon = (startCoordinates.Longitude + endCoordinates.Longitude) / 2;

            // Calculate the tile coordinates for the center
            int xTile = (int)((centerLon + 180) / 360 * (1 << zoomLevel));
            int yTile = (int)((1 - Math.Log(Math.Tan(centerLat * Math.PI / 180) + 1 / Math.Cos(centerLat * Math.PI / 180)) / Math.PI) / 2 * (1 << zoomLevel));

            string mapUrl = string.Format(baseUrl, zoomLevel, xTile, yTile);

            Console.WriteLine($"Center Tile Coordinates: xTile={xTile}, yTile={yTile}");
            Console.WriteLine($"Map URL: {mapUrl}");

            return mapUrl;
        }
        */

        public ICommand TourReportCommand { get; set; }
        public ICommand SummarizedTourReportCommand { get; set; }
        public ICommand ExportTourDataCommand { get; set; }
        public ICommand ImportTourDataCommand { get; set; }

        public ICommand AddTourCommand { get; set; }
        public ICommand UpdateTourCommand { get; set; }
        public ICommand DeleteTourCommand { get; set; }

        public ICommand AddTourLogCommand { get; set; }
        public ICommand DeleteTourLogCommand { get; set; }
        public ICommand SaveTourLogCommand { get; set; }

        public void TourReport()
        {
            if (SelectedTour != null)
            {
                SelectedTour.Name = NewTourName;
                SelectedTour.Description = NewTourDescr;
                SelectedTour.From = NewTourFrom;
                SelectedTour.To = NewTourTo;
                SelectedTour.TransportType = NewTourTransType;
                SelectedTour.Distance = NewTourDistance;
                SelectedTour.EstimatedTime = NewTourEstTime;

                Log.Info($"Download TourReport: {SelectedTour.Name}");
                ReportGenerator.GenerateTourReport(SelectedTour);
            }
        }
        public void SummarizedTourReport()
        {
            if (SelectedTour != null)
            {
                SelectedTour.Name = NewTourName;
                SelectedTour.Description = NewTourDescr;
                SelectedTour.From = NewTourFrom;
                SelectedTour.To = NewTourTo;
                SelectedTour.TransportType = NewTourTransType;
                SelectedTour.Distance = NewTourDistance;
                SelectedTour.EstimatedTime = NewTourEstTime;

                Log.Info($"Download SummarizedTourReport: {SelectedTour.Name}");
                ReportGenerator.GenerateSummarizeReport(SelectedTour);
            }
        }
        public void ExportTourData()
        {
            if (SelectedTour != null)
            {
                SelectedTour.Name = NewTourName;
                SelectedTour.Description = NewTourDescr;
                SelectedTour.From = NewTourFrom;
                SelectedTour.To = NewTourTo;
                SelectedTour.TransportType = NewTourTransType;
                SelectedTour.Distance = NewTourDistance;
                SelectedTour.EstimatedTime = NewTourEstTime;

                ExportImportService.ExportTourToFile(selectedTour);
                Log.Info($"Export Tour Data: {SelectedTour.Name}");
            }
        }
        public void ImportTourData(object filePath)
        {
            string path = filePath as string;
            if (!string.IsNullOrEmpty(path))
            {
                var tmpTour = ExportImportService.ImportTourFromFile(path, Tours.ToList());
                if (tmpTour != null)
                {
                    if (Tours != null)
                    {
                        Log.Info($"Importing tour data from: {path}");
                        Log.Info($"---{tmpTour.Name},{tmpTour.Id}---");
                        _tourService.AddTour(tmpTour);
                        Tours.Add(tmpTour);                   
                    }

                    else
                    {
                        Log.Info("Tours collection is not initialized.");
                    }
                }
            }     
        }
        public void AddTour()
        {
            var newTour = new Tour
            {
                Name = NewTourName,
                Description = NewTourDescr,
                From = NewTourFrom,
                To = NewTourTo,
                TransportType = NewTourTransType,
                Distance = NewTourDistance,
                EstimatedTime = NewTourEstTime,
                Img = "PlaceHolder"
            };

            Log.Info($"Tour Added: {newTour.Name}");
            _tourService.AddTour(newTour);
            Tours.Add(newTour);
        }
        public void UpdateTour()
        {
            if (SelectedTour != null)
            {
                SelectedTour.Name = NewTourName;
                SelectedTour.Description = NewTourDescr;
                SelectedTour.From = NewTourFrom;
                SelectedTour.To = NewTourTo;
                SelectedTour.TransportType = NewTourTransType;
                SelectedTour.Distance = NewTourDistance;
                SelectedTour.EstimatedTime = NewTourEstTime;
                SelectedTour.Img = "PlaceHolder";

                Log.Info($"Tour Updated: {SelectedTour.Name}");
                _tourService.UpdateTour(SelectedTour);
            }
        }
        public void DeleteTour()
        {
            if (SelectedTour != null)
            {
                Log.Info($"Tour Deleted: {SelectedTour.Name}");
                _tourService.DeleteTour(SelectedTour.Id);
                Tours.Remove(SelectedTour);
            }
        }
        private void LoadTours()
        {
            Tours = new ObservableCollection<Tour>(_tourService.GetAllTours());
            foreach (Tour tour in Tours) { Log.Info(tour.Name+": "+tour.Id); }
            Log.Info($"Tours Loading Count: {Tours.Count()}");
        }
        public void AddTourLog()
        {
            if (TourLogsSelectedTour != null)
            {
                var newTourLog = new TourLog
                {
                    Tour = TourLogsSelectedTour,
                    DateTime = NewDateTime,
                    Comment = NewComment,
                    Difficulty = NewDifficulty,
                    TotalDistance = NewTotalDistance,
                    TotalTime = NewTotalTime,
                    Rating = NewRating
                };

                Log.Info($"Adding Tourlog to Tour: {newTourLog.Tour.Name}");
                TourLogsSelectedTour.TourLogs.Add(newTourLog);
                _tourService.UpdateTour(TourLogsSelectedTour);
            }
        }
        public void DeleteTourLog()
        {
            if (SelectedTourLog != null && TourLogsSelectedTour != null)
            {
                Log.Info($"Deleting Tourlog from Tour: {SelectedTourLog.Tour.Name}");
                TourLogsSelectedTour.TourLogs.Remove(SelectedTourLog);
                _tourService.UpdateTour(TourLogsSelectedTour);
            }
        }
        public void SaveTourLog()
        {
            if (SelectedTourLog != null && TourLogsSelectedTour != null)
            {
                SelectedTourLog.DateTime = NewDateTime;
                SelectedTourLog.Comment = NewComment;
                SelectedTourLog.Difficulty = NewDifficulty;
                SelectedTourLog.TotalDistance = NewTotalDistance;
                SelectedTourLog.TotalTime = NewTotalTime;
                SelectedTourLog.Rating = NewRating;

                Log.Info($"Updating Tourlog from Tour: {SelectedTourLog.Tour.Name}");
                _tourService.UpdateTour(TourLogsSelectedTour);
            }
        }

        public ObservableCollection<Tour> Tours { get; set; }

        private Tour selectedTour;
        public Tour SelectedTour
        {
            get { return selectedTour; }
            set
            {
                selectedTour = value;
                OnPropertyChanged();

                if (selectedTour != null)
                {
                    NewTourName = selectedTour.Name;
                    NewTourDescr = selectedTour.Description;
                    NewTourFrom = selectedTour.From;
                    NewTourTo = selectedTour.To;
                    NewTourTransType = selectedTour.TransportType;

                    GetRouteInfo();
                }

                else
                {
                    ClearTourFields();
                }
            }
        }

        private void ClearTourFields()
        {
            NewTourName = string.Empty;
            NewTourDescr = string.Empty;
            NewTourFrom = string.Empty;
            NewTourTo = string.Empty;
            NewTourTransType = string.Empty;
            NewTourDistance = 0;
            NewTourEstTime = 0;
        }

        private string newTourName;
        public string NewTourName
        {
            get { return newTourName; }
            set
            {
                newTourName = value;
                OnPropertyChanged();
            }
        }

        private string newTourDescr;
        public string NewTourDescr
        {
            get { return newTourDescr; }
            set
            {
                newTourDescr = value;
                OnPropertyChanged();
            }
        }

        private string newTourFrom;
        public string NewTourFrom
        {
            get { return newTourFrom; }
            set
            {
                newTourFrom = value;
                OnPropertyChanged();
            }
        }

        private string newTourTo;
        public string NewTourTo
        {
            get { return newTourTo; }
            set
            {
                newTourTo = value;
                OnPropertyChanged();
            }
        }

        private string newTourTransType;
        public string NewTourTransType
        {
            get { return newTourTransType; }
            set
            {
                newTourTransType = value;
                OnPropertyChanged();
            }
        }

        private int newTourDistance;
        public int NewTourDistance
        {
            get { return newTourDistance; }
            set
            {
                newTourDistance = value;
                OnPropertyChanged();
            }
        }

        private int newTourEstTime;
        public int NewTourEstTime
        {
            get { return newTourEstTime; }
            set
            {
                newTourEstTime = value;
                OnPropertyChanged();
            }
        }        

        private Tour tourLogsSelectedTour;
        public Tour TourLogsSelectedTour
        {
            get { return tourLogsSelectedTour; }
            set
            {
                tourLogsSelectedTour = value;
                OnPropertyChanged();
            }
        }

        private TourLog selectedTourLog;
        public TourLog SelectedTourLog
        {
            get { return selectedTourLog; }
            set
            {
                selectedTourLog = value;
                OnPropertyChanged();

                if (selectedTourLog != null)
                {
                    NewDateTime = selectedTourLog.DateTime;
                    NewComment = selectedTourLog.Comment;
                    NewDifficulty = selectedTourLog.Difficulty;
                    NewTotalDistance = selectedTourLog.TotalDistance;
                    NewTotalTime = selectedTourLog.TotalTime;
                    NewRating = selectedTourLog.Rating;
                }
                else
                {
                    ClearTourLogFields();
                }
            }
        }

        private void ClearTourLogFields()
        {
            NewDateTime = DateTime.Now;
            NewComment = string.Empty;
            NewDifficulty = string.Empty;
            NewTotalDistance = string.Empty;
            NewTotalTime = string.Empty;
            NewRating = 0;
        }

        public DateTime newdateTime { get; set; }
        public string newcomment { get; set; }
        public string newdifficulty { get; set; }
        public string newtotalDistance { get; set; }
        public string newtotalTime { get; set; }
        public int newrating { get; set; }

        public DateTime NewDateTime
        {
            get { return newdateTime; }
            set
            {
                newdateTime = value;
                OnPropertyChanged();
            }
        }

        public string NewComment
        {
            get { return newcomment; }
            set
            {
                newcomment = value;
                OnPropertyChanged();
            }
        }

        public string NewDifficulty
        {
            get { return newdifficulty; }
            set
            {
                newdifficulty = value;
                OnPropertyChanged();
            }
        }

        public string NewTotalDistance
        {
            get { return newtotalDistance; }
            set
            {
                newtotalDistance = value;
                OnPropertyChanged();
            }
        }

        public string NewTotalTime
        {
            get { return newtotalTime; }
            set
            {
                newtotalTime = value;
                OnPropertyChanged();
            }
        }

        public int NewRating
        {
            get { return newrating; }
            set
            {
                newrating = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
