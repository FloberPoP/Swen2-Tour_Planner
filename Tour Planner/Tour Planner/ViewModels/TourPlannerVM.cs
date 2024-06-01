using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tour_Planner.Models;
using Tour_Planner.BL;
using log4net;
using System.Windows;
using System.IO;
using System.Text;
using Tour_Planner.DAL;
using static ExportImportService;

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

            ClearValuesCommand = new RelayCommand(o => ClearTourFields());
            ExportDataCommand = new RelayCommand(param => { ExportAllTours((ExportFormat)param); ShowPopup("Information for Export All Tours Data");});

            _tourService = tourService;
            _routeService = new RouteService();

            NewDateTime = DateTime.Now;
            LoadTours();
        }

        public delegate Task RefreshMapDelegate();
        public RefreshMapDelegate RefreshMap { get; set; }

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

                    RefreshMap();
                }

                else
                {
                    NewTourDistance = 0;
                    NewTourEstTime = 0;
                }
            }
        }

        /* Map?
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

        public ICommand ClearValuesCommand { get; set; }
        public ICommand ExportDataCommand { get; set; }

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
                        OnPropertyChanged(nameof(FilteredTours));
                    }

                    else
                    {
                        ShowPopup("Error Importing Check for same Id`s");
                        Log.Info("Tours collection is not initialized.");
                    }
                }
                else 
                {
                    ShowPopup("Error Importing Check for same Id`s");
                }
            }     
        }

        public void ExportAllTours(ExportFormat format)
        {
            switch (format)
            {
                case ExportFormat.CSV:
                    ExportImportService.ExportAsCSV(Tours.ToList());
                    break;
                case ExportFormat.XML:
                    ExportImportService.ExportAsXML(Tours.ToList());
                    break;
                case ExportFormat.JSON:
                    ExportImportService.ExportAsJSON(Tours.ToList());
                    break;
                default:
                    break;
            }
        }

        public void AddTour()
        {
            if (ValidateTourInput())
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
                    Img = "Placeholder" //Img should be stored in Filesystem
                };

                Log.Info($"Tour Added: {newTour.Name}");
                _tourService.AddTour(newTour);
                Tours.Add(newTour);

                FilteredTours = new ObservableCollection<Tour>(Tours);
            }          
        }
        public void UpdateTour()
        {
            if (ValidateTourInput())
            {
                SelectedTour.Name = NewTourName;
                SelectedTour.Description = NewTourDescr;
                SelectedTour.From = NewTourFrom;
                SelectedTour.To = NewTourTo;
                SelectedTour.TransportType = NewTourTransType;
                SelectedTour.Distance = NewTourDistance;
                SelectedTour.EstimatedTime = NewTourEstTime;
                SelectedTour.Img = "Placeholder"; //Img should be stored in Filesystem

                Log.Info($"Tour Updated: {SelectedTour.Name}");
                _tourService.UpdateTour(SelectedTour);
                FilteredTours = new ObservableCollection<Tour>(Tours);
            }
        }
        public void DeleteTour()
        {
            if (SelectedTour != null)
            {
                Log.Info($"Tour Deleted: {SelectedTour.Name}");
                _tourService.DeleteTour(SelectedTour.Id);
                Tours.Remove(SelectedTour);
                FilteredTours = new ObservableCollection<Tour>(Tours);
            }
        }
        private void LoadTours()
        {
            Tours = new ObservableCollection<Tour>(_tourService.GetAllTours());
            FilteredTours = new ObservableCollection<Tour>(Tours);

            foreach (Tour tour in Tours) { Log.Info(tour.Name+": "+tour.Id); }

            Log.Info($"Tours Loading Count: {Tours.Count()}");
        }
        public void AddTourLog()
        {
            if (ValidateTourLogInput())
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
                FilteredTours = new ObservableCollection<Tour>(Tours);
            }           
        }
        public void DeleteTourLog()
        {
            if (SelectedTourLog != null && TourLogsSelectedTour != null)
            {
                Log.Info($"Deleting Tourlog from Tour: {SelectedTourLog.Tour.Name}");
                TourLogsSelectedTour.TourLogs.Remove(SelectedTourLog);
                _tourService.UpdateTour(TourLogsSelectedTour);
                FilteredTours = new ObservableCollection<Tour>(Tours);
            }
        }
        public void SaveTourLog()
        {
            if (ValidateTourLogInput())
            {
                SelectedTourLog.DateTime = NewDateTime;
                SelectedTourLog.Comment = NewComment;
                SelectedTourLog.Difficulty = NewDifficulty;
                SelectedTourLog.TotalDistance = NewTotalDistance;
                SelectedTourLog.TotalTime = NewTotalTime;
                SelectedTourLog.Rating = NewRating;

                Log.Info($"Updating Tourlog from Tour: {SelectedTourLog.Tour.Name}");
                _tourService.UpdateTour(TourLogsSelectedTour);
                FilteredTours = new ObservableCollection<Tour>(Tours);
            }
        }

        public ObservableCollection<Tour> Tours { get; set; }

        private ObservableCollection<Tour> filteredTours;
        public ObservableCollection<Tour> FilteredTours
        {
            get { return filteredTours; }
            set { filteredTours = value; OnPropertyChanged(); }
        }

        private string _searchFilter;
        public string SearchFilter
        {
            get
            {
                return _searchFilter;
            }

            set
            {
                _searchFilter = value;
                ApplyFilter();
                OnPropertyChanged();
            }
        }

        private void ApplyFilter()
        {
            string searchText = SearchFilter?.ToLower() ?? "";

            FilteredTours = new ObservableCollection<Tour>(Tours.Where(tour =>
                tour.Name.ToLower().Contains(searchText) ||
                tour.Description.ToLower().Contains(searchText) ||
                tour.From.ToLower().Contains(searchText) ||
                tour.To.ToLower().Contains(searchText) ||
                tour.ChildFriendliness.ToLower().Contains(searchText)

            ));
        }

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
            NewTourTransType = TransportType.None;
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

        private TransportType newTourTransType;
        public TransportType NewTourTransType
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
                OnPropertyChanged(nameof(FormattedDistance));
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
                OnPropertyChanged(nameof(FormattedEstimatedTime));
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
            NewDifficulty = DifficultyLevel.None;
            NewTotalDistance = string.Empty;
            NewTotalTime = string.Empty;
            NewRating = 0;
        }

        public DateTime newdateTime { get; set; }
        public string newcomment { get; set; }
        public DifficultyLevel newdifficulty { get; set; }
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

        public DifficultyLevel NewDifficulty
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

        public string FormattedDistance
        {
            get
            {
                return $"{NewTourDistance / 1000.0:F2} km";
            }
        }

        public string FormattedEstimatedTime
        {
            get
            {
                int hours = NewTourEstTime / 3600;
                int minutes = (NewTourEstTime % 3600) / 60;
                int seconds = NewTourEstTime % 60;
                return $"{hours}h {minutes}min {seconds}sec";
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

        private bool ValidateTourLogInput()
        {
            string error = null;

            if (TourLogsSelectedTour == null)
            {
                error = "No tour selected. Please select a tour before adding a tour log.";
            }
            else if (NewDateTime == null)
            {
                error = "Date cannot be null or empty";
            }
            else if (string.IsNullOrWhiteSpace(NewComment))
            {
                error = "Comment cannot be null or empty.";
            }
            else if (string.IsNullOrWhiteSpace(NewTotalDistance) || !int.TryParse(NewTotalDistance, out int distance) || distance <= 0)
            {
                error = "Total Distance must be a positive integer.";
            }
            else if (string.IsNullOrWhiteSpace(NewTotalTime) || !int.TryParse(NewTotalTime, out int time) || time <= 0)
            {
                error = "Total Time must be a positive integer.";
            }
            else if (NewRating == null || NewRating < 0 || NewRating > 10)
            {
                error = "Rating must be between 0 and 10.";
            }

            if (error != null)
            {
                MessageBox.Show($"Validation failed: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Log.Info($"Validation failed: {error}");

                return false;
            }
            else
            {
                return true;
            }
        }
        private bool ValidateTourInput()
        {
            string error = null;

            if (string.IsNullOrEmpty(NewTourName))
            {
                error = "Name cannot be null or empty.";
            }
            else if (NewTourName.Length > 100)
            {
                error = "Name cannot be longer than 100 characters.";
            }

            else if (string.IsNullOrEmpty(NewTourDescr))
            {
                error = "Description cannot be null or empty.";
            }
            else if (NewTourDescr.Length > 500)
            {
                error = "Description cannot be longer than 500 characters.";
            }

            else if (string.IsNullOrEmpty(NewTourFrom))
            {
                error = "From cannot be null or empty.";
            }

            else if (string.IsNullOrEmpty(NewTourTo))
            {
                error = "To cannot be null or empty.";
            }

            else if (!Enum.IsDefined(typeof(TransportType), NewTourTransType))
            {
                error = "Transport Type is not valid.";
            }

            else if (NewTourDistance < 0)
            {
                error = "Distance must be a positive integer.";
            }
            else if (NewTourEstTime < 0)
            {
                error = "Estimated Time must be a positive integer.";
            }

            if (error != null)
            {
                MessageBox.Show($"Validation failed: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Log.Info($"Validation failed: {error}");

                return false;
            }

            else
            {
                return true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
