using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tour_Planner
{
    public class TourPlannerVM : INotifyPropertyChanged
    {
        private Tour selectedTour;
        private ObservableCollection<Tour> tours;

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

            AddTourCommand = new RelayCommand(o => AddTour());
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

            Tours.Add(t);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
