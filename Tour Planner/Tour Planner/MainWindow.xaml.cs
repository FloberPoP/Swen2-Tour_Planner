using System.Windows;
using System.Windows.Controls;
using Tour_Planner.Models;
using Tour_Planner.ViewModels;
using log4net.Config;
using log4net;
using Tour_Planner.DAL;

namespace Tour_Planner
{
    public partial class MainWindow : Window
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(App));

        private readonly TourContext _context;
        public MainWindow(TourContext context)
        {
            XmlConfigurator.Configure();
            Log.Info("Application starting...");

            InitializeComponent();
            _context = context;
            var tourRepository = new TourRepository(context);
            DataContext = new TourPlannerVM(tourRepository);
        }

        private void TourListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tourListBox.SelectedItem != null)
            {
                Tour selectedTour = tourListBox.SelectedItem as Tour;
                (DataContext as TourPlannerVM).SelectedTour = selectedTour;
            }
        }

        #region Menu
        private void HamburgerMenu_Checked(object sender, RoutedEventArgs e)
        {
            menuItemsPanel.Visibility = Visibility.Visible;
        }

        private void HamburgerMenu_Unchecked(object sender, RoutedEventArgs e)
        {
            menuItemsPanel.Visibility = Visibility.Collapsed;
        }

        private void AddTour_Click(object sender, RoutedEventArgs e)
        {
            // Implementation here
        }

        private void DeleteTour_Click(object sender, RoutedEventArgs e)
        {
            // Implementation here
        }

        private void UpdateTour_Click(object sender, RoutedEventArgs e)
        {
            // Implementation here
        }

        private void AddLogsButton_Click(object sender, RoutedEventArgs e)
        {
            // Implementation here
        }

        private void DeleteLogsButton_Click(object sender, RoutedEventArgs e)
        {
            // Implementation here
        }

        private void UpdateLogsButton_Click(object sender, RoutedEventArgs e)
        {
            // Implementation here
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            // Implementation here
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            // Implementation here
        }
        #endregion
    }
}
