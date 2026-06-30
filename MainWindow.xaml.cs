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
using LogViewer.Services;
using LogViewer.Models;  // imports the LogEntry model
using Microsoft.Win32;  // provides the OpenFileDialog class for selecting files

namespace LogViewer // groups related classes together, like a folder for code (created by VS)
{
    /// <summary>
    /// Controls the application's main window and connects the UI to the application logic.
    /// </summary>
    public partial class MainWindow : Window  // define the application's main window (split between XAML and C#)
    {
        public MainWindow()  // constructor (runs automatically when the window is created)
        {
            InitializeComponent();  // load and initialize the UI from MainWindow.xaml

            LoadLogFile("sample.log");  // load the default log file when the application starts
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)  // handle the Open Log File button click event
        {
            OpenFileDialog dialog = new OpenFileDialog();  // create a Windows file selection dialog
            dialog.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";  // display .log files by default, with an option to show all files
            dialog.Title = "Select a log file";  // set the dialog window title

            if (dialog.ShowDialog() == true)  // continue only if the user clicks Open
            {
                LoadLogFile(dialog.FileName);  // load the selected log file
            }
        }

        private void LoadLogFile(string filePath)  // parse the specified log file and display its contents
        {
            LogParserService parser = new LogParserService();  // create a new LogParserService object
            List<LogEntry> parsedLogEntries = parser.Parse(filePath);  // parse the specified log file and store the returned list

            LogGrid.ItemsSource = parsedLogEntries;  // bind the parsed log entries to the DataGrid
        }
    }
}