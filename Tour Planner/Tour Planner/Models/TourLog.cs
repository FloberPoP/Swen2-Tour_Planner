using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Tour_Planner.Models
{
    public class TourLog : INotifyPropertyChanged
    {
        public Tour Tour { get; set; }

        private DateTime dateTime;
        public DateTime DateTime
        {
            get { return dateTime; }
            set
            {
                if (dateTime != value)
                {
                    dateTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Comment { get; set; }
        public string Difficulty { get; set; }
        public string TotalDistance { get; set; }
        public string TotalTime { get; set; }
        public int Rating { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
