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

            if (logEntries.Count > 0)  // check that the list contains at least one log entry.
            {
            MessageBox.Show(logEntries[0].Message);   // display the message of the first log entry.
            }
        }
    }
}