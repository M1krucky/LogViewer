// -----------------------------------------------------------------------------
// LogFilterService
// -----------------------------------------------------------------------------

using LogViewer.Models;
using System.Linq;

namespace LogViewer.Services
{
    /// <summary>
    /// Filters log entries using search criteria, log level, and date range.
    /// </summary>
    public static class LogFilterService
    {
        public static List<LogEntry> Filter(List<LogEntry> logEntries, string[] searchTerms, string searchMode, string selectedLevel, DateTime? fromDate, DateTime? toDate)
        {
            return logEntries.Where(item =>
            {
                // Match search terms using the selected search mode (AND / OR).
                bool matchesSearch = searchTerms.Length == 0 ||
                    (searchMode == "AND"
                        ? searchTerms.All(term => MatchesSearchTerm(item, term))
                        : searchTerms.Any(term => MatchesSearchTerm(item, term)));

                // Match the selected log level.
                bool matchesLevel = selectedLevel == "All" || item.Level == selectedLevel;

                // Match the selected date range.
                bool matchesFromDate = fromDate == null || item.Timestamp.Date >= fromDate.Value.Date;
                bool matchesToDate = toDate == null || item.Timestamp.Date <= toDate.Value.Date;

                return matchesSearch && matchesLevel && matchesFromDate && matchesToDate;
            }).ToList();
        }

        private static bool MatchesSearchTerm(LogEntry item, string term)
        {
            return item.Message.Contains(term, StringComparison.OrdinalIgnoreCase)
                || item.Level.Contains(term, StringComparison.OrdinalIgnoreCase)
                || item.Timestamp.ToString().Contains(term, StringComparison.OrdinalIgnoreCase);
        }
    }
}