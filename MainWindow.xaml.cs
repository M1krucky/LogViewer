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
    
        public MainWindow()  // constructor (runs automatically when the window is created)
        {
            InitializeComponent();  // load and initialize the UI from MainWindow.xaml

            LoadLogFile("sample.log");  // load the default log file when the application starts
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)  // handles the Click event raised by the OpenFileButton (WPF supplies: sender = the control that raised the event, e = information about the Click event; WPF requires both parameters)
        {
            OpenFileDialog dialog = new OpenFileDialog();  // create a Windows file selection dialog
            dialog.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";  // display .log files by default, with an option to show all files
            dialog.Title = "Select a log file";  // set the dialog window title

            if (dialog.ShowDialog() == true)  // continue only if the user clicks Open
            {
                LoadLogFile(dialog.FileName);  // load the selected log file
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)  // handles the TextChanged event raised by SearchTextBox
        {
            string searchText = SearchTextBox.Text;  // get the current text entered in the SearchTextBox

            List<LogEntry> filteredLogEntries = allLogEntries
                .Where(item =>
                {
                    bool matchesSearch =

                        item.Message.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                        || item.Level.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                        || item.Timestamp.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase);
                      
                    return matchesSearch;
                }).ToList();

            LogGrid.ItemsSource = filteredLogEntries;
        }

        private void LoadLogFile(string filePath)  // parse the specified log file and display its contents
        {
            try
            {
                LogParserService parser = new LogParserService();  // create a new LogParserService object
                allLogEntries = parser.Parse(filePath);  // parse the specified log file and store all loaded log entries

                LogGrid.ItemsSource = allLogEntries;  // display all loaded log entries in the DataGrid
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
    }
}