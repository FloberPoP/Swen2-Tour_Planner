using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Tour_Planner
{
    public class TourPlannerVM : INotifyPropertyChanged
    {
        public TourPlannerVM()
        {
            tours = new ObservableCollection<Tour>();

            tours.Add(new Tour() { Name = "TourA", Description = "asdf", From = "A", To = "Z", TransportType = "Bim", Distance = 2, EstimatedTime = 3 });
            tours.Add(new Tour() { Name = "TourB", Description = "jklö", From = "B", To = "Y", TransportType = "UBahn", Distance = 2, EstimatedTime = 3 });

            AddTourCommand = new RelayCommand(o => AddTour());
            UpdateTourCommand = new RelayCommand(o => UpdateTour());
            DeleteTourCommand = new RelayCommand(o =>  DeleteTour());

            AddTourLogCommand = new RelayCommand(o => AddTourLog());
            DeleteTourLogCommand = new RelayCommand(o => DeleteTourLog());
            SaveTourLogCommand = new RelayCommand(o => SaveTourLog());

            NewDateTime = DateTime.Now;
        }
     
        public void AddTour()
        {
            Tour t = new Tour();
            t.Name = newTourName;
            t.Description = newTourDescr;
            t.From = newTourFrom;
            t.To = newTourTo;
            t.TransportType = newTourTransType;
            t.Distance = newTourDistance;
            t.EstimatedTime = newTourEstTime;

            tours.Add(t);
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
            }
        }

        public void DeleteTour()
        {
            if (SelectedTour != null)
            {
                Tours.Remove(SelectedTour);
                SelectedTour = null;
            }
        }

        public void AddTourLog()
        {
            TourLog t = new TourLog();
            t.tour = selectedTour;
            t.DateTime = newdateTime;
            t.Comment = newcomment;
            t.Difficulty = newdifficulty;
            t.TotalDistance = newtotalDistance;
            t.TotalTime = newtotalTime;
            t.Rating = newrating;

            if (TourLogsSelectedTour != null)
                TourLogsSelectedTour.TourLogs.Add(t);                                 
        }
        public void DeleteTourLog()
        {
            if (SelectedTourLog != null && TourLogsSelectedTour != null)
            {
                TourLogsSelectedTour.TourLogs.Remove(SelectedTourLog);
                SelectedTourLog = null;
            }
        }
        public void SaveTourLog()
        {
            if (SelectedTourLog != null && TourLogsSelectedTour != null)
            {
                int index = TourLogsSelectedTour.TourLogs.IndexOf(SelectedTourLog);
                if (index != -1)
                {
                    SelectedTourLog.DateTime = NewDateTime;
                    SelectedTourLog.Comment = NewComment;
                    SelectedTourLog.Difficulty = NewDifficulty;
                    SelectedTourLog.TotalDistance = NewTotalDistance;
                    SelectedTourLog.TotalTime = NewTotalTime;
                    SelectedTourLog.Rating = NewRating;

                    TourLogsSelectedTour.TourLogs[index] = SelectedTourLog;
                }
            }
        }

        private Tour tourLogsSelectedTour;
        private Tour selectedTour;
        private TourLog selectedTourLog;
        private ObservableCollection<Tour> tours;

        public ObservableCollection<Tour> Tours
        {
            get
            {
                return tours;
            }

            set
            {
                tours = value;
                OnPropertyChanged();
            }
        }

        #region Tour Data
        private string newTourName { get; set; }
        private string newTourDescr { get; set; }
        private string newTourFrom { get; set; }
        private string newTourTo { get; set; }
        private string newTourTransType { get; set; }
        private int newTourDistance { get; set; }
        private int newTourEstTime { get; set; }

        public ICommand AddTourCommand { get; set; }
        public ICommand UpdateTourCommand { get; set; }
        public ICommand DeleteTourCommand { get; set; }

        public Tour SelectedTour
        {
            get
            {
                return selectedTour;
            }

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
                    NewTourName = "";
                    NewTourDescr = "";
                    NewTourFrom = "";
                    NewTourTo = "";
                    NewTourTransType = "";
                    NewTourDistance = 0;
                    NewTourEstTime = 0;
                }
            }
        }
        public string NewTourName
        {
            get
            {
                return newTourName;
            }

            set
            {
                newTourName = value;
                OnPropertyChanged();
            }
        }
        public string NewTourDescr
        {
            get
            {
                return newTourDescr;
            }

            set
            {
                newTourDescr = value;
                OnPropertyChanged();
            }
        }
        public string NewTourFrom
        {
            get
            {
                return newTourFrom;
            }

            set
            {
                newTourFrom = value;
                OnPropertyChanged();
            }
        }
        public string NewTourTo
        {
            get
            {
                return newTourTo;
            }

            set
            {
                newTourTo = value;
                OnPropertyChanged();
            }
        }
        public string NewTourTransType
        {
            get
            {
                return newTourTransType;
            }

            set
            {
                newTourTransType = value;
                OnPropertyChanged();
            }
        }
        public int NewTourDistance
        {
            get
            {
                return newTourDistance;
            }

            set
            {
                newTourDistance = value;
                OnPropertyChanged();
            }
        }
        public int NewTourEstTime
        {
            get
            {
                return newTourEstTime;
            }

            set
            {
                newTourEstTime = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region TourLogs Data
        public DateTime newdateTime { get; set; }
        public string newcomment { get; set; }
        public string newdifficulty { get; set; }
        public string newtotalDistance { get; set; }
        public string newtotalTime { get; set; }
        public int newrating { get; set; }
        
        public ICommand AddTourLogCommand { get; set; }
        public ICommand DeleteTourLogCommand { get; set; }
        public ICommand SaveTourLogCommand { get; set; }

        public DateTime NewDateTime
        {
            get
            {
                return newdateTime;
            }

            set
            {
                newdateTime = value;
                OnPropertyChanged();
            }
        }
        public string NewComment
        {
            get
            {
                return newcomment;
            }

            set
            {
                newcomment = value;
                OnPropertyChanged();
            }
        }
        public string NewDifficulty
        {
            get
            {
                return newdifficulty;
            }

            set
            {
                newdifficulty = value;
                OnPropertyChanged();
            }
        }
        public string NewTotalDistance
        {
            get
            {
                return newtotalDistance;
            }

            set
            {
                newtotalDistance = value;
                OnPropertyChanged();
            }
        }
        public string NewTotalTime
        {
            get
            {
                return newtotalTime;
            }

            set
            {
                newtotalTime = value;
                OnPropertyChanged();
            }
        }
        public int NewRating
        {
            get
            {
                return newrating;
            }

            set
            {
                if (int.TryParse(value.ToString(), out int intValue))
                {
                    newrating = intValue;
                    OnPropertyChanged();
                }
            }
        }
        public Tour TourLogsSelectedTour
        {
            get
            {
                return tourLogsSelectedTour;
            }

            set
            {
                tourLogsSelectedTour = value;
                OnPropertyChanged();
            }
        }
        public TourLog SelectedTourLog
        {
            get { return selectedTourLog; }
            set
            {
                selectedTourLog = value;
                OnPropertyChanged(nameof(SelectedTourLog));

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
                    NewDateTime = DateTime.Now;
                    NewComment = "";
                    NewDifficulty = "";
                    NewTotalDistance = "";
                    NewTotalTime = "";
                    NewRating = 0;
                }
            }
        }
        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
