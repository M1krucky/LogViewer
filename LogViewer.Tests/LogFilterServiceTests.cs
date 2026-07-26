using LogViewer.Models;
using LogViewer.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogViewer.Tests
{
    /// <summary>
    /// Tests for log filtering functionality.
    /// </summary>

    [TestClass]
    public class LogFilterServiceTests
    {
        [TestMethod]
        public void Filter_ShouldReturnAllEntries_WhenNoFiltersAreApplied()
        {
            // Arrange: create sample log entries and apply no filters
            List<LogEntry> entries = new List<LogEntry>
            {
                new LogEntry
                {
                    Message = "Application started",
                    Level = "INFO",
                    Timestamp = new DateTime(2026, 7, 23, 10, 00, 00)
                },
                new LogEntry
                {
                    Message = "Database connection failed",
                    Level = "ERROR",
                    Timestamp = new DateTime(2026, 7, 23, 11, 00, 00)
                }
            };

            string[] searchTerms = Array.Empty<string>();
            string searchMode = "OR";
            string selectedLevel = "All";
            DateTime? fromDate = null;
            DateTime? toDate = null;

            // Act: filter the log entries
            List<LogEntry> result = LogFilterService.Filter(
                entries,
                searchTerms,
                searchMode,
                selectedLevel,
                fromDate,
                toDate);

            // Assert: verify that all log entries are returned
            Assert.AreEqual(2, result.Count);
        }
    }
}