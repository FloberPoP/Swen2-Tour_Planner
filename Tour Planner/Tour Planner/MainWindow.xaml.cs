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

            tourLogsDataGrid.ItemsSource = new List<TourLog>
            {
                new TourLog { DateTime = DateTime.Now, Duration = "2 hours", Distance = "10 km" },
                new TourLog { DateTime = DateTime.Now.AddDays(-1), Duration = "1.5 hours", Distance = "8 km" },
            };
        }

        private void TourListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
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
    }
}