// prepare chart data for the Statistics window
using LogViewer.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace LogViewer.Services
{
    public static class LogChartService
    {
        public static (ISeries[] Series, Axis[] XAxes, Axis[] YAxes) CreateLogLevelChart(List<LogEntry> logEntries)  // create the log level distribution bar chart
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

        public static (ISeries[] Series, Axis[] XAxes, Axis[] YAxes) CreateErrorTrendChart(List<LogEntry> logEntries, ErrorTrendGrouping grouping)  // create the error trend line chart using the selected time grouping
        {
            List<LogEntry> errorEntries = logEntries
                .Where(item => item.Level == "ERROR")
                .ToList();  // keep only ERROR log entries

            Dictionary<DateTime, int> errorCountsByPeriod = new Dictionary<DateTime, int>();  // store the number of errors for each time period

            foreach (LogEntry item in errorEntries)
            {
                DateTime periodStart;

                if (grouping == ErrorTrendGrouping.Hour)
                {
                    periodStart = new DateTime(item.Timestamp.Year, item.Timestamp.Month, item.Timestamp.Day, item.Timestamp.Hour, 0, 0);  // use the beginning of the hour
                }
                else if (grouping == ErrorTrendGrouping.Day)
                {
                    periodStart = item.Timestamp.Date;  // use the beginning of the day
                }
                else if (grouping == ErrorTrendGrouping.Week)
                {
                    int daysSinceMonday = ((int)item.Timestamp.DayOfWeek + 6) % 7;  // calculate how many days have passed since Monday
                    periodStart = item.Timestamp.Date.AddDays(-daysSinceMonday);  // use Monday as the beginning of the week
                }
                else
                {
                    periodStart = new DateTime(item.Timestamp.Year, item.Timestamp.Month, 1);  // use the first day of the month
                }

                if (errorCountsByPeriod.ContainsKey(periodStart))
                {
                    errorCountsByPeriod[periodStart]++;  // increase the existing error count for this period
                }
                else
                {
                    errorCountsByPeriod[periodStart] = 1;  // create the first error count for this period
                }
            }

            List<DateTime> orderedPeriods = errorCountsByPeriod.Keys
                .OrderBy(item => item)
                .ToList();  // arrange the periods from earliest to latest

            List<int> errorCounts = orderedPeriods
                .Select(item => errorCountsByPeriod[item])
                .ToList();  // prepare the error counts for the chart line

            List<string> labels = new List<string>();  // prepare readable labels for the X-axis

            foreach (DateTime period in orderedPeriods)
            {
                if (grouping == ErrorTrendGrouping.Hour)
                {
                    labels.Add(period.ToString("dd MMM HH:mm"));  // display the date and hour
                }
                else if (grouping == ErrorTrendGrouping.Day)
                {
                    labels.Add(period.ToString("dd MMM"));  // display the day and month
                }
                else if (grouping == ErrorTrendGrouping.Week)
                {
                    labels.Add($"Week of {period:dd MMM}");  // display the first day of the week
                }
                else
                {
                    labels.Add(period.ToString("MMM yyyy"));  // display the month and year
                }
            }

            ISeries[] series =
            {
                new LineSeries<int>
                {
                    Values = errorCounts,
                    Name = "Errors"
                }
            };

            Axis[] xAxes =
            {
                new Axis
                {
                    Labels = labels,
                    TextSize = 12
                }
            };

            Axis[] yAxes =
            {
                new Axis
                {
                    Name = "Errors",
                    NameTextSize = 16,
                    MinLimit = 0
                }
            };

            return (series, xAxes, yAxes);  // return all error trend chart components
        }
    }
}