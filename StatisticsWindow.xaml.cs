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
using LiveChartsCore;  // Core chart interfaces (ISeries, etc.)
using LiveChartsCore.SkiaSharpView;  // WPF chart controls and chart types (Axis, ColumnSeries, etc.)

namespace LogViewer
{
    /// <summary>
    /// Displays summary statistics for the loaded log entries.
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        public ISeries[] LogLevelSeries { get; set; } // data series displayed on the chart.
        public Axis[] LogLevelXAxes { get; set; }  // X-axis configuration (INFO, WARNING, ERROR). 
        public Axis[] LogLevelYAxes { get; set; }  // Y-axis configuration (log count). 

        public StatisticsWindow(List<LogEntry> allLogEntries)  // constructor that receives the loaded log entries (from MainWindow)
        {
            InitializeComponent();  // // create the UI defined in StatisticsWindow.xaml

            DisplayStatistics(allLogEntries);  // calculate statistics and update the UI

            DataContext = this;  // // set this window as the source for XAML data bindings
        }

        private void DisplayStatistics(List<LogEntry> allLogEntries)  // calculate and display log statistics
        {
            int totalCount = allLogEntries.Count;
            int infoCount = allLogEntries.Count(item => item.Level == "INFO");
            int warningCount = allLogEntries.Count(item => item.Level == "WARNING");
            int errorCount = allLogEntries.Count(item => item.Level == "ERROR");
            DateTime latestTimestamp = allLogEntries.Max(item => item.Timestamp);  // find the latest timestamp

            TotalEntriesTextBlock.Text = totalCount.ToString();
            InfoCountTextBlock.Text = infoCount.ToString();
            WarningCountTextBlock.Text = warningCount.ToString();
            ErrorCountTextBlock.Text = errorCount.ToString();
            LatestLogTextBlock.Text = latestTimestamp.ToString("yyyy-MM-dd HH:mm:ss");

            LogLevelSeries = new ISeries[]  // create a new array of chart data series
            {
                new ColumnSeries<int>  // create a bar chart series that stores integer values
                {
                    Values = new int[] { infoCount, warningCount, errorCount }  // Set the bar heights using the calculated log counts
                }
            };
        }
    }
}
