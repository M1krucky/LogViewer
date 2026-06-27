using LogViewer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;  // provides classes for working with files and directories.

namespace LogViewer.Services
{
    internal class LogParserService
    {
        public List<LogEntry> Parse()  // parse log data and return a list of log entries (method definition)
        {
            LogEntry entry = new LogEntry();  // create a new log entry object.

            string[] lines = File.ReadAllLines("sample.log");  // reads all lines from the log file

            List<LogEntry> logEntries = new List<LogEntry>();  // creates an empty list of log entries

            return logEntries;  // returns the empty list
        }
    }
}