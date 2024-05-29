using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tour_Planner.Models;
using Tour_Planner.BL;
using log4net;
using Tour_Planner.DAL;

namespace Tour_Planner.ViewModels
{
    public class TourPlannerVM : INotifyPropertyChanged
    {
        private readonly ITourService _tourService;
        private static readonly ILog Log = LogManager.GetLogger(typeof(App));
        public TourPlannerVM(ITourService tourService)
        {
            AddTourCommand = new RelayCommand(o => AddTour());
            UpdateTourCommand = new RelayCommand(o => UpdateTour());
            DeleteTourCommand = new RelayCommand(o => DeleteTour());

            AddTourLogCommand = new RelayCommand(o => AddTourLog());
            DeleteTourLogCommand = new RelayCommand(o => DeleteTourLog());
            SaveTourLogCommand = new RelayCommand(o => SaveTourLog());

            TourReportCommand = new RelayCommand(o => TourReport());
            SummarizedTourReportCommand = new RelayCommand(o => SummarizedTourReport());
            ExportTourDataCommand = new RelayCommand(o => ExportTourData());
            ImportTourDataCommand = new RelayCommand(filePath => ImportTourData(filePath));

            _tourService = tourService;
            NewDateTime = DateTime.Now;
            LoadTours();
        }

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
                SelectedTour.Img = "PlaceHolder";

                Log.Info($"Dowload TourReport: {SelectedTour.Name}");
                ReportGenerator.GenerateTourReport( SelectedTour);
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
                SelectedTour.Img = "PlaceHolder";

                Log.Info($"Dowload SummarizedTourReport: {SelectedTour.Name}");
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
                SelectedTour.Img = "PlaceHolder";

                ExportImportService.ExportTourToFile(selectedTour);
                Log.Info($"Export Tour Data: {SelectedTour.Name}");
            }
        }
        public void ImportTourData(object filePath)
        {
            string path = filePath as string;
            if (!string.IsNullOrEmpty(path))
            {
                var tmpTour = new Tour();
                tmpTour = ExportImportService.ImportTourFromFile(path,Tours.ToList());
                if(tmpTour != null)
                {
                    _tourService.AddTour(tmpTour);
                    Tours.Add(tmpTour);
                }          
            }
            Log.Info($"Importing tour data from: {path}");
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
                    NewTourDistance = selectedTour.Distance;
                    NewTourEstTime = selectedTour.EstimatedTime;
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
