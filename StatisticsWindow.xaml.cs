
using LogViewer.Services;
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
        public ISeries[] LogLevelSeries { get; set; } = Array.Empty<ISeries>(); // stores the chart data series (initialized as an empty array). "Array.Empty<ISeries>()" -> call the built-in Empty() method of the Array class to get an empty array of ISeries.
        public Axis[] LogLevelXAxes { get; set; } = Array.Empty<Axis>();  // stores the X-axis configuration (initialized as an empty array)
        public Axis[] LogLevelYAxes { get; set; } = Array.Empty<Axis>();  // stores the Y-axis configuration (initialized as an empty array)


        public ISeries[] ErrorTrendSeries { get; set; } = Array.Empty<ISeries>();  // stores the error trend chart data series
        public Axis[] ErrorTrendXAxes { get; set; } = Array.Empty<Axis>();  // stores the error trend X-axis configuration
        public Axis[] ErrorTrendYAxes { get; set; } = Array.Empty<Axis>();  // stores the error trend Y-axis configuration


        public StatisticsWindow(List<LogEntry> allLogEntries)  // constructor that receives the loaded log entries (from MainWindow)
        {
            InitializeComponent();  // // create the UI defined in StatisticsWindow.xaml

            DisplayStatistics(allLogEntries);  // calculate statistics and update the UI

            DataContext = this;  // // set this window as the source for XAML data bindings
        }


        public void RefreshStatistics(List<LogEntry> filteredLogEntries)  // refresh statistics and chart using the latest filtered log entries
        {
            DisplayStatistics(filteredLogEntries);

            DataContext = null; // Reload the bindings from this StatisticsWindow object (bug fix solution - synchronization of chart with filtering) )
            DataContext = this;
        }


        private void DisplayStatistics(List<LogEntry> allLogEntries)  // calculate and display log statistics
        {
            if (allLogEntries.Count == 0)
            {
                TotalEntriesTextBlock.Text = "0";
                InfoCountTextBlock.Text = "0";
                WarningCountTextBlock.Text = "0";
                ErrorCountTextBlock.Text = "0";
                LatestLogTextBlock.Text = "-";

                var chart = LogChartService.CreateLogLevelChart(allLogEntries);  // prepare the log level distribution chart

                LogLevelSeries = chart.Series;  // receive the chart data series
                LogLevelXAxes = chart.XAxes;  // receive the X-axis configuration
                LogLevelYAxes = chart.YAxes;  // receive the Y-axis configuration

                return;  // exit the entire method/function 
            }

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
                    Values = new int[] { infoCount, warningCount, errorCount },  // Set the bar heights using the calculated log counts
                    Name = "Log Count"  // set the chart series name shown in the legend/tooltip
                }
            };


            LogLevelXAxes = new Axis[]  // create a new array of X-axis configurations
            {
                new Axis  // Create the X-axis
                {
                    Labels = new[] { "INFO", "WARNING", "ERROR" },  // set the labels displayed on the X-axis
                    TextSize = 14  // font size of the axis labels
                }
            };


            LogLevelYAxes = new Axis[]  // create a new array of Y-axis configurations
            {
                new Axis  // create the Y-axis
                {
                    Name = "Count",  // display the Y-axis title
                    NameTextSize = 16,  // font size of the axis title
                    MinLimit = 0  // start the Y-axis at 0 instead of auto-calculating the minimum
                }
            };
        }
    }
}
