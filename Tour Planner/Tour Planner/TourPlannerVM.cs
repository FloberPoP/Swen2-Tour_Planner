using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tour_Planner
{
    public class TourPlannerVM : INotifyPropertyChanged
    {
        private Tour selectedTour;
        private ObservableCollection<Tour> tours;

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

        public TourPlannerVM()
        {
            tours = new ObservableCollection<Tour>();

            tours.Add(new Tour() { Name = "TourA", Description = "asdf", From = "A", To = "Z", TransportType = "Bim", Distance = 2, EstimatedTime = 3 });
            tours.Add(new Tour() { Name = "TourB", Description = "jklö", From = "B", To = "Y", TransportType = "UBahn", Distance = 2, EstimatedTime = 3 });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
