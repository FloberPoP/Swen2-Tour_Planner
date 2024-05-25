using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tour_Planner.Models
{
    public class TourLog : INotifyPropertyChanged
    {
        public Tour tour { get; set; }
        private DateTime dateTime { get; set; }
        public string Comment { get; set; }
        public string Difficulty { get; set; }
        public string TotalDistance { get; set; }
        public string TotalTime { get; set; }
        public int Rating { get; set; }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
