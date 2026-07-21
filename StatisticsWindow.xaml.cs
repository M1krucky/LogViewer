
using LiveChartsCore;  // Core chart interfaces (ISeries, etc.)
using LiveChartsCore.SkiaSharpView;  // WPF chart controls and chart types (Axis, ColumnSeries, etc.)
using LiveChartsCore.SkiaSharpView.Painting;
using LogViewer.Models;
using LogViewer.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;  // provides LINQ methods such as Count()
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


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


        private List<LogEntry> currentLogEntries = new List<LogEntry>();  // store the entries currently displayed in the statistics window


        public StatisticsWindow(List<LogEntry> allLogEntries)  // constructor that receives the loaded log entries (from MainWindow)
        {
            InitializeComponent();  // // create the UI defined in StatisticsWindow.xaml

            currentLogEntries = allLogEntries;  // remember the entries for rebuilding the error trend chart

            DisplayStatistics(allLogEntries);  // calculate statistics and update the UI

            DataContext = this;  // // set this window as the source for XAML data bindings
        }


        public void RefreshStatistics(List<LogEntry> filteredLogEntries)  // refresh statistics and charts using the latest filtered log entries
        {
            currentLogEntries = filteredLogEntries;  // remember the latest log entries

            DisplayStatistics(filteredLogEntries);

            DataContext = null;  // reload the bindings from this StatisticsWindow object (disconnect all bindings)
            DataContext = this;  // reconnect and read the current property values again
        }


        private void ErrorTrendGroupingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)  // rebuild the charts when the selected grouping changes
        {
            if (currentLogEntries.Count == 0) return;  // do nothing until log entries are available

            DisplayStatistics(currentLogEntries);

            DataContext = null;  // disconnect the existing bindings
            DataContext = this;  // reconnect the bindings using the updated chart properties
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

                LogLevelSeries = Array.Empty<ISeries>();
                LogLevelXAxes = Array.Empty<Axis>();
                LogLevelYAxes = Array.Empty<Axis>();

                ErrorTrendSeries = Array.Empty<ISeries>();
                ErrorTrendXAxes = Array.Empty<Axis>();
                ErrorTrendYAxes = Array.Empty<Axis>();

                return;
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


            string selectedGrouping = ((ComboBoxItem)ErrorTrendGroupingComboBox.SelectedItem).Content.ToString() ?? "Day";  // read the selected grouping option

            ErrorTrendGrouping grouping = Enum.TryParse(selectedGrouping, out ErrorTrendGrouping parsedGrouping)
                        ? parsedGrouping
                        : ErrorTrendGrouping.Day;  // convert the selected grouping name into an enum value, or use Day if the conversion fails


            var errorTrendChart = LogChartService.CreateErrorTrendChart(allLogEntries, grouping);  // prepare the error trend chart using the selected grouping

            ErrorTrendSeries = errorTrendChart.Series;
            ErrorTrendXAxes = errorTrendChart.XAxes;
            ErrorTrendYAxes = errorTrendChart.YAxes;


            LogLevelSeries = new ISeries[]  // create a new array of chart data series
            {
                new ColumnSeries<int>  // create a bar chart series that stores integer values
                {
                    Values = new int[] { infoCount, warningCount, errorCount },  // Set the bar heights using the calculated log counts
                    Name = "Log Count",  // set the chart series name shown in the legend/tooltip
                }
            };


            LogLevelXAxes = new Axis[]  // create a new array of X-axis configurations
            {
                new Axis  // Create the X-axis
                {
                    Labels = new[] { "INFO", "WARNING", "ERROR" },  // set the labels displayed on the X-axis
                    TextSize = 14,  // font size of the axis labels
                    LabelsPaint = new SolidColorPaint(SKColors.White)  // display numeric labels clearly on the dark background
                }
            };


            LogLevelYAxes = new Axis[]  // create a new array of Y-axis configurations
            {
                new Axis  // create the Y-axis
                {
                    Name = "Count",  // display the Y-axis title
                    NameTextSize = 16,  // font size of the axis title
                    MinLimit = 0,  // start the Y-axis at 0 instead of auto-calculating the minimum
                    LabelsPaint = new SolidColorPaint(SKColors.White),  // display numeric labels clearly on the dark background
                    NamePaint = new SolidColorPaint(SKColors.White)  // display the axis title clearly on the dark background
                }

            };

            
        }
    }
    
}
