using LogViewer.Models;  // imports the LogEntry model
using LogViewer.Services;
using Microsoft.Win32;  // provides the OpenFileDialog class for selecting files
using System.IO;
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

        private readonly List<string> recentFiles = new List<string>();  // store the most recently opened log files for quick access from the File menu

        
        private readonly string recentFilesPath = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "LogViewer",
            "recent-files.txt");  // store the recent files list outside the application installation folder so it persists between application launches


        public MainWindow()  // constructor (runs automatically when the window is created)
        {
            InitializeComponent();  // load and initialize the UI from MainWindow.xaml

            Loaded += MainWindow_Loaded;  // load the default log file after the window is fully initialized ("When the Loaded event happens, call MainWindow_Loaded")

            LoadRecentFiles();  // restore the recent files list when the application starts
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


        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)  // handles the Click event raised by the Exit menu item
        {
            Close();  // close the main application window and exit the application (Window provides a built-in Close() method)
        }


        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)  // handles the Click event raised by the About menu item
        {
            AboutWindow aboutWindow = new AboutWindow();  // create a new About window

            aboutWindow.Owner = this;  // set MainWindow as the owner so the About dialog stays centered relative to the application

            aboutWindow.ShowDialog();  // display the About window as a modal dialog and wait until it is closed
        }


        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)  // handles the TextChanged event raised by SearchTextBox
        {
            ApplyFilters();
        }


        private void SearchModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)  // handles changes between OR and AND search modes
        {
            ApplyFilters();  // immediately refresh the displayed logs using the selected search mode
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

            if (isLoading)  // prevent starting another file load while the current load is still running
            {
                return;
            }

            isLoading = true;  // mark the application as busy while the log file is loading

            LoadingPanel.Visibility = Visibility.Visible;  // display the loading indicator while the log file is loading

            OpenFileButton.IsEnabled = false;  // prevent the user from starting another file load while the current file is loading

            StatisticsButton.IsEnabled = false;  // prevent opening statistics while log data is still loading

            try
            {
                LogParserService parser = new LogParserService();  // create a new LogParserService object

                allLogEntries = await Task.Run(() => parser.Parse(filePath));  // parse the log file on a background thread so the UI stays responsive
                
                filteredLogEntries = allLogEntries;  // show all entries before any filters are applied

                LogGrid.ItemsSource = filteredLogEntries;  // display all loaded log entries in the DataGrid (table with logs on the MainWindow)

                CurrentFileTextBlock.Text = $"Opened file:  {filePath}";  // display the currently opened log file in the application status bar

                AddRecentFile(filePath);  // update the recent files list after the log file is successfully loaded
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load log file.\n\n{ex.Message}",  // display a user-friendly error message followed by the exception details
                    "Error",  // set the message box window title
                    MessageBoxButton.OK,  // display an OK button
                    MessageBoxImage.Error  // display the standard Windows error icon
                 );
            }

            finally
            {
                isLoading = false;  // mark the application as ready after loading finishes or fails

                LoadingPanel.Visibility = Visibility.Collapsed;  // hide the loading indicator after loading finishes

                OpenFileButton.IsEnabled = true;  // allow the user to open another log file after loading finishes

                StatisticsButton.IsEnabled = true;  // allow viewing statistics after loading finishes
            }
        }


        private async void RecentFileMenuItem_Click(object sender, RoutedEventArgs e)  // handles opening a file selected from the Recent Files menu
        {
            if (sender is MenuItem menuItem && menuItem.Tag is string filePath)
            {
                if (!File.Exists(filePath))  // stop if the recent file no longer exists at the saved location
                {
                    MessageBox.Show(
                        "The selected recent file could not be found.",
                        "File Not Found",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                await LoadLogFileAsync(filePath);  // load the selected recent file without blocking the UI
            }
        }


        private void AddRecentFile(string filePath)  // add the successfully opened file to the recent files list
        {
            recentFiles.Remove(filePath);  // prevent duplicate entries by removing the file if it already exists

            recentFiles.Insert(0, filePath);  // add the file to the top of the recent files list

            if (recentFiles.Count > 5)  // keep only the five most recent files
            {
                recentFiles.RemoveAt(5);  // remove the oldest file from the list
            }

        RefreshRecentFilesMenu();  // update the Recent Files menu after the recent files list changes

        SaveRecentFiles();  // save the updated recent files list
        }


        private void RefreshRecentFilesMenu()  // rebuild the Recent Files submenu using the latest file list
        {
            RecentFilesMenuItem.Items.Clear();  // remove previously displayed recent files

            foreach (string item in recentFiles)  // create one menu item for each recent file
            {
                MenuItem recentFileMenuItem = new MenuItem();  // create a new menu item

                recentFileMenuItem.Header = System.IO.Path.GetFileName(item);  // display only the file name
                recentFileMenuItem.Tag = item;  // store the full file path
                recentFileMenuItem.Click += RecentFileMenuItem_Click;  // handle clicks on this menu item

                RecentFilesMenuItem.Items.Add(recentFileMenuItem);  // add the menu item to the Recent Files submenu (attaches the object to the UI)
            }
        }


        private void SaveRecentFiles()  // save the current recent files list to a text file
        {
            string directory = System.IO.Path.GetDirectoryName(recentFilesPath)!;  // get the folder that will contain the recent files file
                                                                                   
            Directory.CreateDirectory(directory);  // create the folder if it does not already exist

            File.WriteAllLines(recentFilesPath, recentFiles);  // save the current recent files list as one file path per line
        }


        private void LoadRecentFiles()  // restore the saved recent files list when the application starts
        {
            if (!File.Exists(recentFilesPath))  // stop if the recent files settings file has not been created yet
            {
                return;
            }

            recentFiles.Clear();  // remove any current entries before restoring the saved list

            recentFiles.AddRange(File.ReadAllLines(recentFilesPath));  // read each saved file path and add it to the recent files list

            RefreshRecentFilesMenu();  // display the restored recent files in the File menu
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