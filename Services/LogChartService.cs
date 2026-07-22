// prepare chart data for the Statistics window
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LogViewer.Models;
using SkiaSharp;
using System.Globalization;

namespace LogViewer.Services
{
    public static class LogChartService
    {
        
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

            CultureInfo culture = new CultureInfo("en-US");

            foreach (DateTime period in orderedPeriods)
            {
                if (grouping == ErrorTrendGrouping.Hour)
                {
                    labels.Add(period.ToString("dd MMM HH:mm", culture));  // display the date and hour in English
                }
                else if (grouping == ErrorTrendGrouping.Day)
                {
                    labels.Add(period.ToString("dd MMM", culture));  // display the day and month in English
                }
                else if (grouping == ErrorTrendGrouping.Week)
                {
                    labels.Add($"Week of {period.ToString("dd MMM", culture)}");  // display the first day of the week in English
                }
                else
                {
                    labels.Add(period.ToString("MMM yyyy", culture));  // display the month and year in English
                }
            }

            ISeries[] series =
            {
                new LineSeries<int>
                {
                    Values = errorCounts,
                    Name = "Errors",
                    Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 1.5f },  // configure the trend line color and thickness
                    GeometrySize = 3,  // set the size of the data point markers
                }

            };

            Axis[] xAxes =
            {
                new Axis
                {
                    Labels = labels,
                    TextSize = 12,
                    LabelsPaint = new SolidColorPaint(SKColors.White),  // display numeric labels clearly on the dark background
                }
            };

            Axis[] yAxes =
            {
                new Axis
                {
                    Name = "Errors",
                    NameTextSize = 16,
                    MinLimit = 0,
                    LabelsPaint = new SolidColorPaint(SKColors.White),  // display numeric labels clearly on the dark background
                    NamePaint = new SolidColorPaint(SKColors.White)  // display the axis title clearly on the dark background
                }   
            };

            return (series, xAxes, yAxes);  // return all error trend chart components
        }
    }
}