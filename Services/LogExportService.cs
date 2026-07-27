// -----------------------------------------------------------------------------
// LogExportService
// -----------------------------------------------------------------------------

using LogViewer.Models;
using System.IO;
using System.Text;

namespace LogViewer.Services
{
    /// <summary>
    /// Exports log entries to a CSV file.
    /// </summary>
    public static class LogExportService
    {
        public static void ExportToCsv(List<LogEntry> logEntries, string filePath)
        {
            StringBuilder csvBuilder = new StringBuilder();

            // Add the CSV header row.
            csvBuilder.AppendLine("Timestamp;Level;Message");

            // Export each log entry.
            foreach (LogEntry item in logEntries)
            {
                string timestamp = EscapeCsvValue(item.Timestamp.ToString());
                string level = EscapeCsvValue(item.Level);
                string message = EscapeCsvValue(item.Message);

                csvBuilder.AppendLine($"{timestamp};{level};{message}");
            }

            File.WriteAllText(filePath, csvBuilder.ToString());
        }

        private static string EscapeCsvValue(string value)
        {
            // Escape double quotation marks and wrap the value in quotes.
            string escapedValue = value.Replace("\"", "\"\"");

            return $"\"{escapedValue}\"";
        }
    }
}