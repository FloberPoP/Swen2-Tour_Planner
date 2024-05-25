using System.Windows;
using System.Windows.Controls;
using Tour_Planner.Models;
using Tour_Planner.ViewModels;
using log4net.Config;
using log4net;
using Tour_Planner.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Tour_Planner
{
    public partial class MainWindow : Window
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(App));

        public MainWindow()
        {
            XmlConfigurator.Configure();
            Log.Info("Application starting...");
            InitializeComponent();

            // Build configuration
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings/appsettings.json")
                .Build();

            // Get connection string from appsettings.json
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            // Configure the DbContextOptions for TourContext
            var optionsBuilder = new DbContextOptionsBuilder<TourContext>();
            optionsBuilder.UseNpgsql(connectionString);

            // Create TourRepository with the configured TourContext
            var tourRepository = new TourRepository(new TourContext(optionsBuilder.Options));

            // Create TourPlannerVM with the TourRepository
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


        private void TourLogsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

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

        }

        private void DeleteTour_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void UpdateTour_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddLogsButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void DeleteLogsButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void UpdateLogsButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}