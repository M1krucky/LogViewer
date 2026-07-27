// -----------------------------------------------------------------------------
// LogParserService
// -----------------------------------------------------------------------------

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
        public List<LogEntry> Parse(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            return ParseLines(lines);
        }

        public List<LogEntry> ParseLines(string[] lines)
        {
            List<LogEntry> logEntries = new List<LogEntry>();

            foreach (string line in lines)
            {
                string[] parts = line.Split(' ', 4);

                // Ignore malformed lines that do not contain all required fields.
                if (parts.Length < 4)
                {
                    continue;
                }

                // Ignore lines with an invalid date or time value.
                if (!DateTime.TryParse(parts[0] + " " + parts[1], out DateTime timestamp))
                {
                    continue;
                }

                LogEntry entry = new LogEntry();

                entry.Timestamp = timestamp;

                // Treat bracketed and unbracketed log levels consistently.
                string level = parts[2].Trim('[', ']');

                entry.Level = level;
                entry.Message = parts[3];

                logEntries.Add(entry);
            }

            return logEntries;
        }
    }
}