using LogViewer.Models;
using System.IO;
using System.Text;

namespace LogViewer.Services
{
    public static class LogExportService
    {
        public static void ExportToCsv(List<LogEntry> logEntries, string filePath)
        {
            StringBuilder csvBuilder = new StringBuilder();  // collect all CSV rows before writing the file

            csvBuilder.AppendLine("Timestamp;Level;Message");  // add the CSV header row

            foreach (LogEntry item in logEntries)  // process each log entry
            {
                string timestamp = EscapeCsvValue(item.Timestamp.ToString());  // prepare the timestamp for CSV
                string level = EscapeCsvValue(item.Level);  // prepare the log level for CSV
                string message = EscapeCsvValue(item.Message);  // prepare the log message for CSV

                csvBuilder.AppendLine($"{timestamp};{level};{message}");  // add one CSV row
            }

            File.WriteAllText(filePath, csvBuilder.ToString());  // save the CSV file to disk
        }
    
        private static string EscapeCsvValue(string value)
        {
            string escapedValue = value.Replace("\"", "\"\"");  // duplicate quotation marks as required by the CSV format

            return $"\"{escapedValue}\"";  // wrap the value in quotation marks
        }
    }
}