// -----------------------------------------------------------------------------
// LogChartService
// -----------------------------------------------------------------------------

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LogViewer.Models;
using SkiaSharp;
using System.Globalization;

namespace LogViewer.Services
{
    /// <summary>
    /// Prepares error trend chart data for the Statistics window.
    /// </summary>
    public static class LogChartService
    {
        public static (
            ISeries[] Series,
            Axis[] XAxes,
            Axis[] YAxes)
            CreateErrorTrendChart(
                List<LogEntry> logEntries,
                ErrorTrendGrouping grouping)
        {
            List<LogEntry> errorEntries = logEntries
                .Where(item => item.Level == "ERROR")
                .ToList();

            Dictionary<DateTime, int> errorCountsByPeriod =
                new Dictionary<DateTime, int>();

            foreach (LogEntry item in errorEntries)
            {
                DateTime periodStart;

                // Determine the beginning of the selected grouping period.
                if (grouping == ErrorTrendGrouping.Hour)
                {
                    periodStart = new DateTime(
                        item.Timestamp.Year,
                        item.Timestamp.Month,
                        item.Timestamp.Day,
                        item.Timestamp.Hour,
                        0,
                        0);
                }
                else if (grouping == ErrorTrendGrouping.Day)
                {
                    periodStart = item.Timestamp.Date;
                }
                else if (grouping == ErrorTrendGrouping.Week)
                {
                    // Use Monday as the beginning of the week.
                    int daysSinceMonday =
                        ((int)item.Timestamp.DayOfWeek + 6) % 7;

                    periodStart =
                        item.Timestamp.Date.AddDays(-daysSinceMonday);
                }
                else
                {
                    periodStart = new DateTime(
                        item.Timestamp.Year,
                        item.Timestamp.Month,
                        1);
                }

                // Count errors within each grouping period.
                if (errorCountsByPeriod.ContainsKey(periodStart))
                {
                    errorCountsByPeriod[periodStart]++;
                }
                else
                {
                    errorCountsByPeriod[periodStart] = 1;
                }
            }

            List<DateTime> orderedPeriods = errorCountsByPeriod.Keys
                .OrderBy(item => item)
                .ToList();

            List<int> errorCounts = orderedPeriods
                .Select(item => errorCountsByPeriod[item])
                .ToList();

            List<string> labels = new List<string>();

            CultureInfo culture = new CultureInfo("en-US");

            // Format X-axis labels according to the selected grouping.
            foreach (DateTime period in orderedPeriods)
            {
                if (grouping == ErrorTrendGrouping.Hour)
                {
                    labels.Add(
                        period.ToString("dd MMM HH:mm", culture));
                }
                else if (grouping == ErrorTrendGrouping.Day)
                {
                    labels.Add(
                        period.ToString("dd MMM", culture));
                }
                else if (grouping == ErrorTrendGrouping.Week)
                {
                    labels.Add(
                        $"Week of {period.ToString("dd MMM", culture)}");
                }
                else
                {
                    labels.Add(
                        period.ToString("MMM yyyy", culture));
                }
            }

            ISeries[] series =
            {
                new LineSeries<int>
                {
                    Values = errorCounts,
                    Name = "Errors",
                    Stroke = new SolidColorPaint(SKColors.Red)
                    {
                        StrokeThickness = 1.5f
                    },
                    GeometrySize = 3
                }
            };

            Axis[] xAxes =
            {
                new Axis
                {
                    Labels = labels,
                    TextSize = 12,
                    LabelsPaint = new SolidColorPaint(SKColors.White)
                }
            };

            Axis[] yAxes =
            {
                new Axis
                {
                    Name = "Errors",
                    NameTextSize = 16,
                    MinLimit = 0,
                    LabelsPaint = new SolidColorPaint(SKColors.White),
                    NamePaint = new SolidColorPaint(SKColors.White)
                }
            };

            return (series, xAxes, yAxes);
        }
    }
}