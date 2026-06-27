using LogViewer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;  // provides classes for working with files and directories.

namespace LogViewer.Services
{
    internal class LogParserService
    {
        public List<LogEntry> Parse()  // Parse log data and return a list of log entries.
        {
            LogEntry entry = new LogEntry();  // Create a new log entry object.

            string[] lines = File.ReadAllLines("sample.log");  // reads all lines from the log file

            entry.Timestamp = DateTime.Now;  // Set the timestamp.
            entry.Level = "INFO";  // Set the log level.
            entry.Message = "Application started";  // Set the log message.

            return new List<LogEntry> { entry };  // Return a list containing the log entry.
        }
    }
}
