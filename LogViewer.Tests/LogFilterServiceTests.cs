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


        [TestMethod]
        public void Filter_ShouldReturnMatchingEntries_WhenSearchingByMessage()
        {
            // Arrange: create log entries with different messages
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

            string[] searchTerms = { "database" };
            string searchMode = "OR";
            string selectedLevel = "All";
            DateTime? fromDate = null;
            DateTime? toDate = null;

            // Act: filter the log entries by message text
            List<LogEntry> result = LogFilterService.Filter(
                entries,
                searchTerms,
                searchMode,
                selectedLevel,
                fromDate,
                toDate);

            // Assert: verify that only the matching log entry is returned
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Database connection failed", result[0].Message);
        }


        [TestMethod]
        public void Filter_ShouldReturnMatchingEntries_WhenSearchingByLogLevel()
        {
            // Arrange: create log entries with different log levels
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

            string[] searchTerms = { "error" };
            string searchMode = "OR";
            string selectedLevel = "All";
            DateTime? fromDate = null;
            DateTime? toDate = null;

            // Act: filter the log entries by log level
            List<LogEntry> result = LogFilterService.Filter(
                entries,
                searchTerms,
                searchMode,
                selectedLevel,
                fromDate,
                toDate);

            // Assert: verify that only the matching log entry is returned
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("ERROR", result[0].Level);
        }


        [TestMethod]
        public void Filter_ShouldReturnMatchingEntries_WhenSearchingByTimestamp()
        {
            // Arrange: create log entries with different timestamps
            List<LogEntry> entries = new List<LogEntry>
            {
                new LogEntry
                {
                    Message = "Application started",
                    Level = "INFO",
                    Timestamp = new DateTime(2026, 7, 23, 10, 15, 00)
                },
                new LogEntry
                {
                    Message = "Database connection failed",
                    Level = "ERROR",
                    Timestamp = new DateTime(2026, 7, 24, 11, 30, 00)
                }
            };

            string[] searchTerms = { "24.07.2026" };
            string searchMode = "OR";
            string selectedLevel = "All";
            DateTime? fromDate = null;
            DateTime? toDate = null;

            // Act: filter the log entries by timestamp
            List<LogEntry> result = LogFilterService.Filter(
                entries,
                searchTerms,
                searchMode,
                selectedLevel,
                fromDate,
                toDate);

            // Assert: verify that only the matching log entry is returned
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(new DateTime(2026, 7, 24, 11, 30, 00), result[0].Timestamp);
        }


        [TestMethod]
        public void Filter_ShouldIgnoreCase_WhenSearching()
        {
            // Arrange: create log entries with mixed-case text
            List<LogEntry> entries = new List<LogEntry>
            {
                new LogEntry
                {
                    Message = "Database Connection Failed",
                    Level = "ERROR",
                    Timestamp = new DateTime(2026, 7, 23, 10, 00, 00)
                }
            };

            string[] searchTerms = { "database" };
            string searchMode = "OR";
            string selectedLevel = "All";
            DateTime? fromDate = null;
            DateTime? toDate = null;

            // Act: filter the log entries using lowercase search text
            List<LogEntry> result = LogFilterService.Filter(
                entries,
                searchTerms,
                searchMode,
                selectedLevel,
                fromDate,
                toDate);

            // Assert: verify that the search is case-insensitive
            Assert.AreEqual(1, result.Count);
        }


        [TestMethod]
        public void Filter_ShouldReturnEntriesMatchingAnyTerm_WhenSearchModeIsOr()
        {
            // Arrange: create log entries matching different search terms
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
                },
                new LogEntry
                {
                    Message = "User logged out",
                    Level = "INFO",
                    Timestamp = new DateTime(2026, 7, 23, 12, 00, 00)
                }
            };

            string[] searchTerms = { "application", "database" };
            string searchMode = "OR";
            string selectedLevel = "All";
            DateTime? fromDate = null;
            DateTime? toDate = null;

            // Act: filter entries that match at least one search term
            List<LogEntry> result = LogFilterService.Filter(
                entries,
                searchTerms,
                searchMode,
                selectedLevel,
                fromDate,
                toDate);

            // Assert: verify that both matching entries are returned
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Application started", result[0].Message);
            Assert.AreEqual("Database connection failed", result[1].Message);


        }


        [TestMethod]
        public void Filter_ShouldReturnEntriesMatchingAllTerms_WhenSearchModeIsAnd()
        {
            // Arrange: create log entries with different messages
            List<LogEntry> entries = new List<LogEntry>
            {
                new LogEntry
                {
                    Message = "Database connection failed",
                    Level = "ERROR",
                    Timestamp = new DateTime(2026, 7, 23, 10, 00, 00)
                },
                new LogEntry
                {
                    Message = "Database connection established",
                    Level = "INFO",
                    Timestamp = new DateTime(2026, 7, 23, 11, 00, 00)
                }
            };

            string[] searchTerms = { "database", "failed" };
            string searchMode = "AND";
            string selectedLevel = "All";
            DateTime? fromDate = null;
            DateTime? toDate = null;

            // Act: filter entries that match all search terms
            List<LogEntry> result = LogFilterService.Filter(
                entries,
                searchTerms,
                searchMode,
                selectedLevel,
                fromDate,
                toDate);

            // Assert: verify that only the entry matching all terms is returned
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Database connection failed", result[0].Message);
        }


        [TestMethod]
        public void Filter_ShouldReturnOnlySelectedLogLevel()
        {
            // Arrange: create log entries with different log levels
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
                },
                new LogEntry
                {
                    Message = "Low disk space",
                    Level = "WARNING",
                    Timestamp = new DateTime(2026, 7, 23, 12, 00, 00)
                }
            };

            string[] searchTerms = Array.Empty<string>();
            string searchMode = "OR";
            string selectedLevel = "ERROR";
            DateTime? fromDate = null;
            DateTime? toDate = null;

            // Act: filter entries by selected log level
            List<LogEntry> result = LogFilterService.Filter(
                entries,
                searchTerms,
                searchMode,
                selectedLevel,
                fromDate,
                toDate);

            // Assert: verify that only ERROR entries are returned
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("ERROR", result[0].Level);
        }


        [TestMethod]
        public void Filter_ShouldReturnEntriesOnOrAfterFromDate()
        {
            // Arrange: create log entries on different dates
            List<LogEntry> entries = new List<LogEntry>
            {
                new LogEntry
                {
                    Message = "First",
                    Level = "INFO",
                    Timestamp = new DateTime(2026, 7, 22, 10, 00, 00)
                },
                new LogEntry
                {
                    Message = "Second",
                    Level = "INFO",
                    Timestamp = new DateTime(2026, 7, 23, 10, 00, 00)
                },
                new LogEntry
                {
                    Message = "Third",
                    Level = "INFO",
                    Timestamp = new DateTime(2026, 7, 24, 10, 00, 00)
                }
            };

            string[] searchTerms = Array.Empty<string>();
            string searchMode = "OR";
            string selectedLevel = "All";
            DateTime? fromDate = new DateTime(2026, 7, 23);
            DateTime? toDate = null;

            // Act: filter entries using the From Date
            List<LogEntry> result = LogFilterService.Filter(
                entries,
                searchTerms,
                searchMode,
                selectedLevel,
                fromDate,
                toDate);

            // Assert: verify that only entries on or after the From Date are returned
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Second", result[0].Message);
            Assert.AreEqual("Third", result[1].Message);
        }


        [TestMethod]
        public void Filter_ShouldReturnEntriesOnOrBeforeToDate()
        {
            // Arrange: create log entries on different dates
            List<LogEntry> entries = new List<LogEntry>
            {
                new LogEntry
                {
                    Message = "First",
                    Level = "INFO",
                    Timestamp = new DateTime(2026, 7, 22, 10, 00, 00)
                },
                new LogEntry
                {
                    Message = "Second",
                    Level = "INFO",
                    Timestamp = new DateTime(2026, 7, 23, 10, 00, 00)
                },
                new LogEntry
                {
                    Message = "Third",
                    Level = "INFO",
                    Timestamp = new DateTime(2026, 7, 24, 10, 00, 00)
                }
            };

            string[] searchTerms = Array.Empty<string>();
            string searchMode = "OR";
            string selectedLevel = "All";
            DateTime? fromDate = null;
            DateTime? toDate = new DateTime(2026, 7, 23);

            // Act: filter entries using the To Date
            List<LogEntry> result = LogFilterService.Filter(
                entries,
                searchTerms,
                searchMode,
                selectedLevel,
                fromDate,
                toDate);

            // Assert: verify that only entries on or before the To Date are returned
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("First", result[0].Message);
            Assert.AreEqual("Second", result[1].Message);
        }


        [TestMethod]
        public void Filter_ShouldReturnEntriesWithinDateRange()
        {
            // Arrange: create log entries on different dates
            List<LogEntry> entries = new List<LogEntry>
            {
                new LogEntry
                {
                    Message = "First",
                    Level = "INFO",
                    Timestamp = new DateTime(2026, 7, 22, 10, 00, 00)
                },
                new LogEntry
                {
                    Message = "Second",
                    Level = "INFO",
                    Timestamp = new DateTime(2026, 7, 23, 10, 00, 00)
                },
                new LogEntry
                {
                    Message = "Third",
                    Level = "INFO",
                    Timestamp = new DateTime(2026, 7, 24, 10, 00, 00)
                }
            };

            string[] searchTerms = Array.Empty<string>();
            string searchMode = "OR";
            string selectedLevel = "All";
            DateTime? fromDate = new DateTime(2026, 7, 23);
            DateTime? toDate = new DateTime(2026, 7, 23);

            // Act: filter entries within the date range
            List<LogEntry> result = LogFilterService.Filter(
                entries,
                searchTerms,
                searchMode,
                selectedLevel,
                fromDate,
                toDate);

            // Assert: verify that only entries within the date range are returned
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Second", result[0].Message);
        }


        [TestMethod]
        public void Filter_ShouldApplySearchLevelAndDateFiltersTogether()
        {
            // Arrange: create log entries with different messages, levels and dates
            List<LogEntry> entries = new List<LogEntry>
            {
                new LogEntry
                {
                    Message = "Database connection failed",
                    Level = "ERROR",
                    Timestamp = new DateTime(2026, 7, 23, 10, 00, 00)
                },
                new LogEntry
                {
                    Message = "Database connection failed",
                    Level = "INFO",
                    Timestamp = new DateTime(2026, 7, 23, 11, 00, 00)
                },
                new LogEntry
                {
                    Message = "Database connection failed",
                    Level = "ERROR",
                    Timestamp = new DateTime(2026, 7, 24, 12, 00, 00)
                }
            };

            string[] searchTerms = { "database" };
            string searchMode = "OR";
            string selectedLevel = "ERROR";
            DateTime? fromDate = new DateTime(2026, 7, 23);
            DateTime? toDate = new DateTime(2026, 7, 23);

            // Act: apply all filters together
            List<LogEntry> result = LogFilterService.Filter(
                entries,
                searchTerms,
                searchMode,
                selectedLevel,
                fromDate,
                toDate);

            // Assert: verify that only the entry matching all filters is returned
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(new DateTime(2026, 7, 23, 10, 00, 00), result[0].Timestamp);
        }


        [TestMethod]
        public void Filter_ShouldReturnEmptyList_WhenNoEntriesMatch()
        {
            // Arrange: create log entries that do not match the search term
            List<LogEntry> entries = new List<LogEntry>
            {
                new LogEntry
                {
                    Message = "Application started",
                    Level = "INFO",
                    Timestamp = new DateTime(2026, 7, 23, 10, 00, 00)
                }
            };

            string[] searchTerms = { "database" };
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

            // Assert: verify that no entries are returned
            Assert.AreEqual(0, result.Count);
        }
    }
}