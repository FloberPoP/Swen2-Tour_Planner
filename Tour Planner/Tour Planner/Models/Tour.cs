using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Tour_Planner.Models
{
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

        public int Id;
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string TransportType { get; set; }
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
