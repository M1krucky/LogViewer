// -----------------------------------------------------------------------------
// LogStatisticsService
// -----------------------------------------------------------------------------

using LogViewer.Models;
using System;
using System.Collections.Generic;

namespace LogViewer.Services
{
    /// <summary>
    /// Calculates summary statistics for a collection of log entries.
    /// </summary>
    public static class LogStatisticsService
    {
        public static LogStatistics CalculateStatistics(List<LogEntry> entries)
        {
            if (entries.Count == 0)
            {
                return new LogStatistics
                {
                    TotalCount = 0,
                    InfoCount = 0,
                    WarningCount = 0,
                    ErrorCount = 0,
                    LatestTimestamp = null
                };
            }

            // Calculate summary statistics for the provided log entries.
            return new LogStatistics
            {
                TotalCount = entries.Count,
                InfoCount = entries.Count(item => item.Level == "INFO"),
                WarningCount = entries.Count(item => item.Level == "WARNING"),
                ErrorCount = entries.Count(item => item.Level == "ERROR"),
                LatestTimestamp = entries.Max(item => item.Timestamp)
            };
        }
    }
}