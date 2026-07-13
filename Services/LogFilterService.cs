using LogViewer.Models;
using System.Linq;

namespace LogViewer.Services
{
    public static class LogFilterService
    {
        public static List<LogEntry> Filter(List<LogEntry> logEntries, string[] searchTerms, string searchMode, string selectedLevel, DateTime? fromDate, DateTime? toDate)
        {
            return logEntries.Where(item =>
            {
                bool matchesSearch = searchTerms.Length == 0 || (searchMode == "AND" ? searchTerms.All(term => MatchesSearchTerm(item, term)) : searchTerms.Any(term => MatchesSearchTerm(item, term)));

                bool matchesLevel = selectedLevel == "All" || item.Level == selectedLevel;

                bool matchesFromDate = fromDate == null || item.Timestamp.Date >= fromDate.Value.Date;

                bool matchesToDate = toDate == null || item.Timestamp.Date <= toDate.Value.Date;

                return matchesSearch && matchesLevel && matchesFromDate && matchesToDate;
            }).ToList();
        }

        private static bool MatchesSearchTerm(LogEntry item, string term)
        {
            return item.Message.Contains(term, StringComparison.OrdinalIgnoreCase) || item.Level.Contains(term, StringComparison.OrdinalIgnoreCase) || item.Timestamp.ToString().Contains(term, StringComparison.OrdinalIgnoreCase);
        }
    }
}