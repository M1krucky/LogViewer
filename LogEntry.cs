// -----------------------------------------------------------------------------
// LogEntry
// Represents a single log entry parsed from a log file.
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;

namespace LogViewer.Models
{
    /// <summary>
    /// Represents a single log entry parsed from a log file.
    /// </summary>
    public class LogEntry
    {
        // Log entry properties.
        public string Message { get; set; } = "";
        public string Level { get; set; } = "";
        public DateTime Timestamp { get; set; }
    }
}