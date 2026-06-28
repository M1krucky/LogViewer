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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           
            LogParserService parser = new LogParserService();  // create a new LogParserService object.
            var logEntries = parser.Parse();  // call the Parse() method and store the returned list.

            LogGrid.ItemsSource = logEntries;  // bind the parsed log entries to the DataGrid
        }
        
    }
}