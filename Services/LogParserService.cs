using LogViewer.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace LogViewer.Services
{
    /// <summary>
    /// Reads log data and converts it into LogEntry objects.
    /// </summary>

    public class LogParserService
    {
        public List<LogEntry> Parse(string filePath)  // read the selected file and parse all log entries
        {
            string[] lines = File.ReadAllLines(filePath);  // reads all lines from the selected log file

            return ParseLines(lines);  // parses the loaded lines
        }

        public List<LogEntry> ParseLines(string[] lines)  // parse log lines and return a list of log entries
        {
            List<LogEntry> logEntries = new List<LogEntry>();  // creates an empty list of log entries

            foreach (string line in lines)  // processes each line from the log file
            {
                string[] parts = line.Split(' ', 4);  // splits the line into date, time, level, and message

                LogEntry entry = new LogEntry();  // creates a new log entry object

                entry.Timestamp = DateTime.Parse(parts[0] + " " + parts[1]);  // parses date and time from the log line

                string level = parts[2].Trim('[', ']');  // normalize bracketed log levels so [ERROR] and ERROR are treated the same

                entry.Level = level;  // stores the normalized log level

                entry.Message = parts[3];  // stores the log message

                logEntries.Add(entry);  // adds the parsed log entry object to the list
            }

            return logEntries;  // returns all parsed log entries
        }
    }
}