using System;
using System.Collections.Generic;
using System.Text;

namespace LogViewer.Services
{
    namespace LogViewer.Services
    {
        /// <summary>
        /// Stores summary statistics calculated from a collection of log entries.
        /// </summary>
        
        public class LogStatistics
        {
            public int TotalCount { get; set; }  // total number of log entries
            public int InfoCount { get; set; }  // number of INFO log entries
            public int WarningCount { get; set; }  // number of WARNING log entries
            public int ErrorCount { get; set; }  // number of ERROR log entries
            public DateTime? LatestTimestamp { get; set; }  // latest log timestamp, or null when no log entries are available
        }
    }
}
