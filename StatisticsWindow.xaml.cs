// -----------------------------------------------------------------------------
// StatisticsWindow
// -----------------------------------------------------------------------------

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LogViewer.Models;
using LogViewer.Services;
using SkiaSharp;
using System.Windows;
using System.Windows.Controls;

namespace LogViewer
{
    /// <summary>
    /// Displays summary statistics for the loaded log entries.
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        // Log-level chart configuration.
        public ISeries[] LogLevelSeries { get; set; } = Array.Empty<ISeries>();
        public Axis[] LogLevelXAxes { get; set; } = Array.Empty<Axis>();
        public Axis[] LogLevelYAxes { get; set; } = Array.Empty<Axis>();

        // Error-trend chart configuration.
        public ISeries[] ErrorTrendSeries { get; set; } = Array.Empty<ISeries>();
        public Axis[] ErrorTrendXAxes { get; set; } = Array.Empty<Axis>();
        public Axis[] ErrorTrendYAxes { get; set; } = Array.Empty<Axis>();

        private List<LogEntry> currentLogEntries = new List<LogEntry>();

        public StatisticsWindow(List<LogEntry> allLogEntries)
        {
            InitializeComponent();

            currentLogEntries = allLogEntries;

            DisplayStatistics(allLogEntries);

            DataContext = this;
        }

        public void RefreshStatistics(List<LogEntry> filteredLogEntries)
        {
            currentLogEntries = filteredLogEntries;

            DisplayStatistics(filteredLogEntries);

            // Reconnect the bindings to read the updated chart properties.
            DataContext = null;
            DataContext = this;
        }

        private void ErrorTrendGroupingComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (currentLogEntries.Count == 0)
            {
                return;
            }

            DisplayStatistics(currentLogEntries);

            // Reconnect the bindings to read the updated chart properties.
            DataContext = null;
            DataContext = this;
        }

        private void DisplayStatistics(List<LogEntry> allLogEntries)
        {
            if (allLogEntries.Count == 0)
            {
                LogLevelSeries = Array.Empty<ISeries>();
                LogLevelXAxes = Array.Empty<Axis>();
                LogLevelYAxes = Array.Empty<Axis>();

                ErrorTrendSeries = Array.Empty<ISeries>();
                ErrorTrendXAxes = Array.Empty<Axis>();
                ErrorTrendYAxes = Array.Empty<Axis>();

                return;
            }

            LogStatistics statistics =
                LogStatisticsService.CalculateStatistics(allLogEntries);

            TotalEntriesTextBlock.Text =
                statistics.TotalCount.ToString("N0");

            InfoCountTextBlock.Text =
                statistics.InfoCount.ToString("N0");

            WarningCountTextBlock.Text =
                statistics.WarningCount.ToString("N0");

            ErrorCountTextBlock.Text =
                statistics.ErrorCount.ToString("N0");

            LatestLogTextBlock.Text =
                statistics.LatestTimestamp?.ToString("yyyy-MM-dd HH:mm:ss") ?? "-";

            string selectedGrouping =
                ((ComboBoxItem)ErrorTrendGroupingComboBox.SelectedItem)
                .Content
                .ToString() ?? "Day";

            // Convert the selected grouping name to its enum value.
            ErrorTrendGrouping grouping =
                Enum.TryParse(
                    selectedGrouping,
                    out ErrorTrendGrouping parsedGrouping)
                    ? parsedGrouping
                    : ErrorTrendGrouping.Day;

            var errorTrendChart =
                LogChartService.CreateErrorTrendChart(
                    allLogEntries,
                    grouping);

            ErrorTrendSeries = errorTrendChart.Series;
            ErrorTrendXAxes = errorTrendChart.XAxes;
            ErrorTrendYAxes = errorTrendChart.YAxes;

            ColumnSeries<int> logLevelColumnSeries =
                new ColumnSeries<int>
                {
                    Values = new int[]
                    {
                        statistics.InfoCount,
                        statistics.WarningCount,
                        statistics.ErrorCount
                    },
                    Name = "Log Count"
                };

            // Apply a distinct color to each log-level bar.
            logLevelColumnSeries.PointMeasured += point =>
            {
                if (point.Context.Visual is null)
                {
                    return;
                }

                point.Context.Visual.Fill = point.Index switch
                {
                    0 => new SolidColorPaint(SKColors.SteelBlue),
                    1 => new SolidColorPaint(new SKColor(180, 110, 0)),
                    2 => new SolidColorPaint(SKColors.Red),
                    _ => new SolidColorPaint(SKColors.SteelBlue)
                };
            };

            LogLevelSeries = new ISeries[]
            {
                logLevelColumnSeries
            };

            LogLevelXAxes = new Axis[]
            {
                new Axis
                {
                    Labels = new[] { "INFO", "WARNING", "ERROR" },
                    LabelsPaint = new SolidColorPaint(SKColors.White),
                    TextSize = 14
                }
            };

            LogLevelYAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Count",
                    NameTextSize = 14,
                    MinLimit = 0,
                    LabelsPaint = new SolidColorPaint(SKColors.White),
                    NamePaint = new SolidColorPaint(SKColors.White)
                }
            };
        }
    }
}