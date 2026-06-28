using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;

namespace LogViewer.Models
{
    internal class LogEntry
    {
        public string Message { get; set; } = ""; // Message stores the text of one log entry
        public string Level { get; set; } = "";  // Level stores the log level (INFO, WARNING, ERROR)
        public DateTime Timestamp { get; set; }  // Timestamp stores the date and time of the log entry
    }

}
