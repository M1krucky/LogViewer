using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;

namespace LogViewer.Models  
{
    /// <summary>
    /// Defines the structure of a single log entry.
    /// </summary>
   
    public class LogEntry  // model/class (LogEntry) defines the structure of one log entry
    {
        public string Message { get; set; } = ""; // stores the text of one log entry
        public string Level { get; set; } = "";  // stores the log level (INFO, WARNING, ERROR)
        public DateTime Timestamp { get; set; }  // stores the date and time of the log entry
    }

}
