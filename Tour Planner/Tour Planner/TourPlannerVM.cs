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
            AddTourLogCommand = new RelayCommand(o => AddTourLog());
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

            if(TourLogsSelectedTour != null)
                TourLogsSelectedTour.TourLogs.Add(t);
        }

        private Tour tourLogsSelectedTour;
        private Tour selectedTour;
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
        public ICommand RemoveTourCommand { get; set; }

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
                newrating = value;
                OnPropertyChanged();
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
        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
