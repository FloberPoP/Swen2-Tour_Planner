using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tour_Planner.Models
{
    public enum TransportType
    {
        Walk,
        Bicycle,
        Car,
        None
    }

    public class Tour : INotifyPropertyChanged
    {
        private ObservableCollection<TourLog> tourLogs;

        public Tour()
        {
            tourLogs = new ObservableCollection<TourLog>();
            tourLogs.CollectionChanged += (s, e) => UpdateComputedProperties();
        }

        public ObservableCollection<TourLog> TourLogs
        {
            get { return tourLogs; }
            set
            {
                tourLogs = value;
                OnPropertyChanged(nameof(TourLogs));
                UpdateComputedProperties();
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        private TransportType transportType;
        public TransportType TransportType
        {
            get { return transportType; }
            set
            {
                if (transportType != value)
                {
                    transportType = value;
                    OnPropertyChanged(nameof(TransportType));
                }
            }
        }

        public int Distance { get; set; }
        public int EstimatedTime { get; set; }
        public string Img { get; set; }
        public int Popularity => TourLogs?.Count ?? 0;
        public string ChildFriendliness
        {
            get
            {
                if (TourLogs == null || !TourLogs.Any())
                {
                    return "Not enough data";
                }

                double averageDifficulty = TourLogs.Average(log => (int)log.Difficulty);
                double averageTime = TourLogs.Average(log => Convert.ToDouble(log.TotalTime));
                double averageDistance = TourLogs.Average(log => Convert.ToDouble(log.TotalDistance));

                bool isChildFriendly = averageDifficulty <= 2 && averageTime <= 3600 && averageDistance <= 5000;

                return isChildFriendly ? "Child-Friendly" : "Not Child-Friendly";
            }
        }

        private void UpdateComputedProperties()
        {
            OnPropertyChanged(nameof(Popularity));
            OnPropertyChanged(nameof(ChildFriendliness));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
