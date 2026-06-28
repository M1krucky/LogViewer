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
            string[] lines = File.ReadAllLines("sample.log");  // reads all lines from the log file

            List<LogEntry> logEntries = new List<LogEntry>();  // creates an empty list of log entries

            foreach (string line in lines)  // processes each line from the log file
            {
                string[] parts = line.Split(' ', 4);  // splits the line into date, time, level, and message

                LogEntry entry = new LogEntry();  // creates a new log entry object

                entry.Timestamp = DateTime.Parse(parts[0] + " " + parts[1]);  // parses date and time (DateTime.Parse(...) is a built-in method that converts text into an object).
                entry.Level = parts[2];  // stores the log level
                entry.Message = parts[3];  // stores the log message

                logEntries.Add(entry);  // // adds the parsed log entry object to the list
            }

            return logEntries;  // returns all parsed log entries
        }
    }
}