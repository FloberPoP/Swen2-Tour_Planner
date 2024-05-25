using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tour_Planner.Models;
using Tour_Planner.ViewModels;

namespace Tour_Planner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Initialize your tour data and set DataContext
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TourListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tourListBox.SelectedItem != null)
            {
                // Get the selected tour from the ListBox
                Tour selectedTour = tourListBox.SelectedItem as Tour;

                // Set the selected tour in the view model
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