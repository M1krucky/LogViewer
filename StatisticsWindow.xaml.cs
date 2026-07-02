using LogViewer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;  // provides LINQ methods such as Count()

namespace LogViewer
{
    /// <summary>
    /// Displays summary statistics for the loaded log entries.
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        public StatisticsWindow(List<LogEntry> logEntries)  // constructor that receives the loaded log entries (from MainWindow)
        {
            InitializeComponent();

            DisplayStatistics(logEntries);
        }

        private void DisplayStatistics(List<LogEntry> logEntries)  // calculate and display log statistics
        {
            int totalCount = logEntries.Count;
            int infoCount = logEntries.Count(item => item.Level == "INFO");
            int warningCount = logEntries.Count(item => item.Level == "WARNING");
            int errorCount = logEntries.Count(item => item.Level == "ERROR");
            DateTime latestTimestamp = logEntries.Max(item => item.Timestamp);  // find the latest timestamp

            StatisticsTextBlock.Text =
                $"Total entries: {totalCount}\n" +
                $"INFO: {infoCount}\n" +
                $"WARNING: {warningCount}\n" +
                $"ERROR: {errorCount}\n" +
                $"Latest log: {latestTimestamp}";
        }
    }
}
