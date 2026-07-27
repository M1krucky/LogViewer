// -----------------------------------------------------------------------------
// MainWindow
// -----------------------------------------------------------------------------

using LogViewer.Models;
using LogViewer.Services;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace LogViewer
{
    /// <summary>
    /// Controls the application's main window and connects the UI to the application logic.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Loaded log data and current application state.
        private List<LogEntry> allLogEntries = new List<LogEntry>();
        private List<LogEntry> filteredLogEntries = new List<LogEntry>();
        private bool isLoading = false;
        private StatisticsWindow? statisticsWindow;

        // Recent files are persisted between application sessions.
        private readonly List<string> recentFiles = new List<string>();

        private readonly string recentFilesPath = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "LogViewer",
            "recent-files.txt");

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;

            LoadRecentFiles();
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadLogFileAsync("sample.log");
        }

        private async void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";
            dialog.Title = "Select a log file";

            if (dialog.ShowDialog() == true)
            {
                await LoadLogFileAsync(dialog.FileName);
            }
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();

            aboutWindow.Owner = this;

            aboutWindow.ShowDialog();
        }

        private void SearchTextBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SearchModeComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void LevelFilterComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void DatePicker_SelectedDateChanged(
            object? sender,
            SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void LogGrid_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            LogEntry? selectedLogEntry = LogGrid.SelectedItem as LogEntry;

            if (selectedLogEntry == null)
            {
                SelectedTimestampTextBlock.Text = "Timestamp: -";
                SelectedLevelTextBlock.Text = "Level: -";
                SelectedMessageTextBlock.Text = "Message: -";
                return;
            }

            SelectedTimestampTextBlock.Text =
                $"Timestamp: {selectedLogEntry.Timestamp}";

            SelectedLevelTextBlock.Text =
                $"Level: {selectedLogEntry.Level}";

            SelectedMessageTextBlock.Text =
                $"Message: {selectedLogEntry.Message}";
        }

        private void ApplyFilters()
        {
            // Prevent filter processing before the DataGrid is initialized.
            if (LogGrid == null)
            {
                return;
            }

            string searchText = SearchTextBox.Text;

            string[] searchTerms = searchText.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries);

            string selectedLevel =
                ((ComboBoxItem)LevelFilterComboBox.SelectedItem)
                .Content
                .ToString() ?? "All";

            string searchMode =
                ((ComboBoxItem)SearchModeComboBox.SelectedItem)
                .Content
                .ToString() ?? "OR";

            DateTime? fromDate = FromDatePicker.SelectedDate;
            DateTime? toDate = ToDatePicker.SelectedDate;

            filteredLogEntries = LogFilterService.Filter(
                allLogEntries,
                searchTerms,
                searchMode,
                selectedLevel,
                fromDate,
                toDate);

            LogGrid.ItemsSource = filteredLogEntries;

            // Keep the open Statistics window synchronized with active filters.
            if (statisticsWindow != null)
            {
                statisticsWindow.RefreshStatistics(filteredLogEntries);
            }
        }

        private async Task LoadLogFileAsync(string filePath)
        {
            // Prevent concurrent file-loading operations.
            if (isLoading)
            {
                return;
            }

            isLoading = true;

            LoadingPanel.Visibility = Visibility.Visible;
            OpenFileButton.IsEnabled = false;
            StatisticsButton.IsEnabled = false;

            try
            {
                LogParserService parser = new LogParserService();

                // Parse the file on a background thread to keep the UI responsive.
                allLogEntries = await Task.Run(
                    () => parser.Parse(filePath));

                ApplyFilters();

                CurrentFileTextBlock.Text =
                    $"Opened file:  {filePath}";

                AddRecentFile(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to load log file.\n\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                isLoading = false;

                LoadingPanel.Visibility = Visibility.Collapsed;
                OpenFileButton.IsEnabled = true;
                StatisticsButton.IsEnabled = true;
            }
        }

        private async void RecentFileMenuItem_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem &&
                menuItem.Tag is string filePath)
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show(
                        "The selected recent file could not be found.",
                        "File Not Found",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                await LoadLogFileAsync(filePath);
            }
        }

        private void AddRecentFile(string filePath)
        {
            // Move an existing entry to the top instead of creating a duplicate.
            recentFiles.Remove(filePath);
            recentFiles.Insert(0, filePath);

            if (recentFiles.Count > 5)
            {
                recentFiles.RemoveAt(5);
            }

            RefreshRecentFilesMenu();
            SaveRecentFiles();
        }

        private void RefreshRecentFilesMenu()
        {
            RecentFilesMenuItem.Items.Clear();

            foreach (string item in recentFiles)
            {
                MenuItem recentFileMenuItem = new MenuItem();

                recentFileMenuItem.Header =
                    System.IO.Path.GetFileName(item);

                recentFileMenuItem.Tag = item;
                recentFileMenuItem.Click += RecentFileMenuItem_Click;

                RecentFilesMenuItem.Items.Add(recentFileMenuItem);
            }
        }

        private void SaveRecentFiles()
        {
            string directory =
                System.IO.Path.GetDirectoryName(recentFilesPath)!;

            Directory.CreateDirectory(directory);

            File.WriteAllLines(recentFilesPath, recentFiles);
        }

        private void LoadRecentFiles()
        {
            if (!File.Exists(recentFilesPath))
            {
                return;
            }

            recentFiles.Clear();

            recentFiles.AddRange(
                File.ReadAllLines(recentFilesPath));

            RefreshRecentFilesMenu();
        }

        private void StatisticsButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            // Keep only one Statistics window open at a time.
            if (statisticsWindow == null)
            {
                statisticsWindow =
                    new StatisticsWindow(filteredLogEntries);

                statisticsWindow.Closed += StatisticsWindow_Closed;
                statisticsWindow.Show();
            }
            else
            {
                statisticsWindow.Activate();
            }
        }

        private void StatisticsWindow_Closed(
            object? sender,
            EventArgs e)
        {
            statisticsWindow = null;
        }

        private void ExportButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter =
                "CSV files (*.csv)|*.csv|All files (*.*)|*.*";

            dialog.Title =
                "Export filtered log entries";

            dialog.FileName =
                "log-export.csv";

            if (dialog.ShowDialog() == true)
            {
                LogExportService.ExportToCsv(
                    filteredLogEntries,
                    dialog.FileName);

                MessageBox.Show(
                    "Log entries exported successfully.",
                    "Export Complete",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
    }
}