using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Tour_Planner.Models
{
    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard,
        None
    }

    public class TourLog : INotifyPropertyChanged
    {
        public Tour Tour { get; set; }

        private DateTime dateTime;

        public DateTime DateTime
        {
            get 
            {
                return dateTime.ToLocalTime(); 
            }
            set
            {
                if (dateTime != value)
                {
                    dateTime = value;
                    OnPropertyChanged();
                }
            }
        }

        [Key]
        public int Id { get; set; }
        public string Comment { get; set; }
        public DifficultyLevel Difficulty { get; set; }
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
