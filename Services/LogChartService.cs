// prepare chart data for the Statistics window
using LogViewer.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace LogViewer.Services
{
    public static class LogChartService
    {
        public static (ISeries[] Series, Axis[] XAxes, Axis[] YAxes) CreateLogLevelChart(List<LogEntry> logEntries)
        {
            int infoCount = logEntries.Count(item => item.Level == "INFO");  // count INFO log entries
            int warningCount = logEntries.Count(item => item.Level == "WARNING");  // count WARNING log entries
            int errorCount = logEntries.Count(item => item.Level == "ERROR");  // count ERROR log entries

            ISeries[] series =
            {
                new ColumnSeries<int>
                {
                    Values = new[] { infoCount, warningCount, errorCount },  // set the bar heights
                    Name = "Log Count"
                }
            };

            Axis[] xAxes =
            {
                new Axis
                {
                    Labels = new[] { "INFO", "WARNING", "ERROR" },  // display log levels on the X-axis
                    TextSize = 14
                }
            };

            Axis[] yAxes =
            {
                new Axis
                {
                    Name = "Count",  // display the number of log entries
                    NameTextSize = 16,
                    MinLimit = 0
                }
            };

            return (series, xAxes, yAxes);  // return all chart components together
        }


        public static (ISeries[] Series, Axis[] XAxes, Axis[] YAxes) CreateErrorTrendChart()
        {
            ISeries[] series =
            {
                new LineSeries<int>
                {
                    Values = new[] { 3, 7, 5, 10, 6 }  // temporary sample data
                }
            };

            Axis[] xAxes =
            {
                new Axis
                {
                    Labels = new[] { "Mon", "Tue", "Wed", "Thu", "Fri" }  // temporary X-axis labels
                }
            };

            Axis[] yAxes =
            {
                new Axis
                {
                    Name = "Errors",  // display the number of ERROR log entries
                    MinLimit = 0
                }
            };

            return (series, xAxes, yAxes);

        }
     
    }
}