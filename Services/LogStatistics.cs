// -----------------------------------------------------------------------------
// LogStatistics
// -----------------------------------------------------------------------------

using System;

namespace LogViewer.Services
{
    /// <summary>
    /// Stores summary statistics calculated from a collection of log entries.
    /// </summary>
    public class LogStatistics
    {
        // Summary statistics.
        public int TotalCount { get; set; }
        public int InfoCount { get; set; }
        public int WarningCount { get; set; }
        public int ErrorCount { get; set; }
        public DateTime? LatestTimestamp { get; set; }
    }
}