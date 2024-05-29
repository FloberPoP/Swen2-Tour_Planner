﻿using System.Windows;
using System.Windows.Controls;
using Tour_Planner.Models;
using Tour_Planner.ViewModels;
using log4net.Config;
using log4net;
using Tour_Planner.DAL;
using Tour_Planner.BL;


namespace Tour_Planner
{
    public partial class MainWindow : Window
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(App));

        private readonly ITourService _tourService;

        public MainWindow(ITourService tourService)
        {
            XmlConfigurator.Configure();
            Log.Info("Application starting...");

            InitializeComponent();
            _tourService = tourService;
            DataContext = new TourPlannerVM(_tourService);
        }

        private void TourListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tourListBox.SelectedItem != null)
            {
                Tour selectedTour = tourListBox.SelectedItem as Tour;
                (DataContext as TourPlannerVM).SelectedTour = selectedTour;
                Log.Info($"Tour selected: {selectedTour.Name}");
            }
        }

        #region Menu
        /*
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
        */
        #endregion
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        
    }
}