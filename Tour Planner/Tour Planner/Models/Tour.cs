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
        }

        public ObservableCollection<TourLog> TourLogs
        {
            get { return tourLogs; }
            set
            {
                tourLogs = value;
                OnPropertyChanged(nameof(TourLogs));
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
