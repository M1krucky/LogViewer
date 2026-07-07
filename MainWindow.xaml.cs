using LogViewer.Models;  // imports the LogEntry model
using LogViewer.Services;
using Microsoft.Win32;  // provides the OpenFileDialog class for selecting files
using System.Linq;
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

namespace LogViewer // groups related classes together, like a folder for code (created by VS)
{
    /// <summary>
    /// Controls the application's main window and connects the UI to the application logic.
    /// </summary>
    public partial class MainWindow : Window  // define the application's main window (split between XAML and C#)
    {
        
        private List<LogEntry> allLogEntries = new List<LogEntry>();  // store all loaded log entries for search and filtering

        private List<LogEntry> filteredLogEntries = new List<LogEntry>();  // store the log entries after applying the current filters

        private bool isLoading = false;  // tracks whether a log file is currently being loaded

        private StatisticsWindow? statisticsWindow;  // stores the currently opened Statistics window (or null until a window is created)

        public MainWindow()  // constructor (runs automatically when the window is created)
        {
            InitializeComponent();  // load and initialize the UI from MainWindow.xaml

            Loaded += MainWindow_Loaded;  // load the default log file after the window is fully initialized ("When the Loaded event happens, call MainWindow_Loaded")
        }


        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)  // loads the default log file after the window is ready
        {
            await LoadLogFileAsync("sample.log");  // load the default log file without blocking the UI during startup
        }


        private async void OpenFileButton_Click(object sender, RoutedEventArgs e)  // handles the Click event raised by the OpenFileButton without blocking the UI while a log file is loading
        {
            OpenFileDialog dialog = new OpenFileDialog();  // create a Windows file selection dialog
            dialog.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";  // display .log files by default, with an option to show all files
            dialog.Title = "Select a log file";  // set the dialog window title

            if (dialog.ShowDialog() == true)  // continue only if the user clicks Open
            {
                await LoadLogFileAsync(dialog.FileName);  // / load the selected log file without blocking the UI
            }
        }


        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)  // handles the TextChanged event raised by SearchTextBox
        {
            ApplyFilters();
        }


        private void LevelFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)  // handles the SelectionChanged event raised by the LevelFilterComboBox
        {
            ApplyFilters();  
        }


        private void DatePicker_SelectedDateChanged(object? sender, SelectionChangedEventArgs e)  // handles date changes in FromDatePicker and ToDatePicker
        {
            ApplyFilters();  // apply all active filters whenever the selected date range changes
        }


        private void LogGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)  // handles the SelectionChanged event raised by the LogGrid
        {
            LogEntry? selectedLogEntry = LogGrid.SelectedItem as LogEntry;  // get the currently selected log entry (null if no row is selected)

            if (selectedLogEntry == null)
            {
                SelectedTimestampTextBlock.Text = "Timestamp: -";
                SelectedLevelTextBlock.Text = "Level: -";
                SelectedMessageTextBlock.Text = "Message: -";
                return;
            }

            SelectedTimestampTextBlock.Text = $"Timestamp: {selectedLogEntry.Timestamp}";
            SelectedLevelTextBlock.Text = $"Level: {selectedLogEntry.Level}";
            SelectedMessageTextBlock.Text = $"Message: {selectedLogEntry.Message}";
        }


        private void ApplyFilters()  // applies all active filters
        {
            if (LogGrid == null)  // exit the method if the DataGrid has not been created yet during window initialization (t's a guard against code running too early)
            {
                return;  // prevent a NullReferenceException while the UI is still being initialized
            }

            string searchText = SearchTextBox.Text;  // get the current text entered in the SearchTextBox

            string selectedLevel = ((ComboBoxItem)LevelFilterComboBox.SelectedItem).Content.ToString() ?? "All";  // get the selected log level from the ComboBox (use "All" as a safe default if the result is null)

            DateTime? fromDate = FromDatePicker.SelectedDate;  // get the selected start date (null means no lower date limit)

            DateTime? toDate = ToDatePicker.SelectedDate;  // get the selected end date (null means no upper date limit)

            filteredLogEntries = allLogEntries
                .Where(item =>
                {
                    bool matchesSearch =

                        item.Message.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                        || item.Level.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                        || item.Timestamp.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase);

                    bool matchesLevel =

                         selectedLevel == "All"
                         || item.Level == selectedLevel;

                    bool matchesFromDate =
                         fromDate == null
                         || item.Timestamp.Date >= fromDate.Value.Date;

                    bool matchesToDate =
                         toDate == null
                         || item.Timestamp.Date <= toDate.Value.Date;

                    return matchesSearch && matchesLevel && matchesFromDate && matchesToDate;

                }).ToList();

            LogGrid.ItemsSource = filteredLogEntries; // display the filteredLogEntries list in the table with logs on the MainWindow

            if (statisticsWindow != null)  // refresh the Statistics window only if the StatisticsWindow object exists
            {
                statisticsWindow.RefreshStatistics(filteredLogEntries);  // update the statistics and chart using the latest filtered log entries
            }
        }


        private async Task LoadLogFileAsync(string filePath)  // load the specified log file without blocking the UI (async load)
        {
            try
            {
                LogParserService parser = new LogParserService();  // create a new LogParserService object

                allLogEntries = await Task.Run(() => parser.Parse(filePath));  // parse the log file on a background thread so the UI stays responsive

                filteredLogEntries = allLogEntries;  // show all entries before any filters are applied

                LogGrid.ItemsSource = filteredLogEntries;  // display all loaded log entries in the DataGrid (table with logs on the MainWindow)
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load log file.\n\n{ex.Message}",  // display a user-friendly error message followed by the exception details
                    "Error",  // set the message box window title
                    MessageBoxButton.OK,  // display an OK button
                    MessageBoxImage.Error  // display the standard Windows error icon
                 );
            }

        }
        private void StatisticsButton_Click(object sender, RoutedEventArgs e)  // handles the Click event raised by the StatisticsButton
        {
            if (statisticsWindow == null)  // create a new Statistics window only if no Statistics window is currently open; otherwise, reuse the existing one
            {
                statisticsWindow = new StatisticsWindow(filteredLogEntries);  // create a new StatisticsWindow object
                statisticsWindow.Closed += StatisticsWindow_Closed; // call StatisticsWindow_Closed when the Statistics window is closed
                statisticsWindow.Show();  // display the Statistics window
            }
            else
            {
                statisticsWindow.Activate();  // each time the Statistics button is clicked, bring the existing Statistics window to the front and make it the active window to prevent it from remaining hidden behind another window
            }
        }


        private void StatisticsWindow_Closed(object? sender, EventArgs e)  // handles the Closed event raised by the Statistics window
        {
            statisticsWindow = null;  // clear the reference so MainWindow knows the Statistics window is no longer open and can create a new one when the Statistics button is clicked again, fixing the issue where the Statistics window could not be reopened after being closed
        }
          
    }
}