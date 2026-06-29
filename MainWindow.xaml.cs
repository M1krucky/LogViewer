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
    public partial class MainWindow : Window  // define the application's main window class, split across XAML and C# files, and inherit the built-in WPF Window functionality.
    {
        public MainWindow()  // the constructor (created by VS).This method is executed automatically when the window is created.
        {
            InitializeComponent();  // created by VS, load MainWindow.xaml, create all UI controls (Button, DataGrid, etc.), assign their names (e.g., LogGrid), connect event handlers (e.g., Click), and initialize the window. 

            LogParserService parser = new LogParserService();  // create a new LogParserService object.
            List<LogEntry> parsedLogEntries = parser.Parse("sample.log");  // call the Parse() method and store the returned list.

            LogGrid.ItemsSource = parsedLogEntries;  // bind the parsed log entries to the DataGrid (connect the UI to the application logic)
        }
        private void OpenFileButton_Click(object sender, RoutedEventArgs e)  // handle the Open Log File button click event
        {

            OpenFileDialog dialog = new OpenFileDialog();  // create a Windows file selection dialog (it is a .NET class)

            dialog.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";  // display .log files by default, with an option to show all files
            dialog.Title = "Select a log file";  // set the dialog window title

            if (dialog.ShowDialog() == true)  // display the dialog and continue only if the user clicks Open (dialog.ShowDialog() is a method that opens the Windows file picker)
            {
                MessageBox.Show(dialog.FileName);  // display the full path of the selected file
            }
        }

}
}