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

            // Set DataContext to your ViewModel
            // DataContext = new TourViewModel();
        }

        // Handle Tour selection change event
        private void TourListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Update Tour Details Panel based on selected Tour
            // You can access the selected Tour through tourListBox.SelectedItem
        }

        // Handle Tour Log selection change event
        private void TourLogsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Update Tour Log Details Panel based on selected Tour Log
            // You can access the selected Tour Log through tourLogsListBox.SelectedItem
        }

        // Handle Add Tour Log button click event
        private void AddTourLog_Click(object sender, RoutedEventArgs e)
        {
            // Open a dialog or navigate to a new page for adding/editing Tour Logs
        }

        // Handle Hamburger Menu click event
        private void HamburgerMenu_Click(object sender, RoutedEventArgs e)
        {
            // Implement your Hamburger Menu functionality (e.g., show options for Add, Delete, Update)
        }
    }
}