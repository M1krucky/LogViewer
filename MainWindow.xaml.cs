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

namespace LogViewer
{
    /// <summary>
    /// Controls the application's main window and connects the UI to the application logic.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           
            LogParserService parser = new LogParserService();  // create a new LogParserService object.
            List<LogEntry> parsedLogEntries = parser.Parse("sample.log");  // call the Parse() method and store the returned list.

            LogGrid.ItemsSource = parsedLogEntries;  // bind the parsed log entries to the DataGrid (connect the UI to the application logic)
        }
        
    }
}