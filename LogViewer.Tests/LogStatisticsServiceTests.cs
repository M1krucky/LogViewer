// -----------------------------------------------------------------------------
// LogStatisticsServiceTests
// -----------------------------------------------------------------------------

using LogViewer.Models;
using LogViewer.Services;

namespace LogViewer.Tests
{
    /// <summary>
    /// Contains unit tests for the LogStatisticsService class.
    /// </summary>
    [TestClass]
    public class LogStatisticsServiceTests
    {
        [TestMethod]
        public void CalculateStatistics_ShouldReturnCorrectCounts()
        {
            // Arrange: create sample log entries with different log levels
            List<LogEntry> entries = new List<LogEntry>
            {
                new LogEntry { Level = "INFO" },
                new LogEntry { Level = "INFO" },
                new LogEntry { Level = "WARNING" },
                new LogEntry { Level = "ERROR" }
            };

            // Act: calculate statistics for the sample log entries
            LogStatistics result = LogStatisticsService.CalculateStatistics(entries);

            // Assert: verify that the calculated statistics match the expected values
            Assert.AreEqual(4, result.TotalCount);
            Assert.AreEqual(2, result.InfoCount);
            Assert.AreEqual(1, result.WarningCount);
            Assert.AreEqual(1, result.ErrorCount);
        }

        [TestMethod]
        public void CalculateStatistics_ShouldReturnZeroCounts_WhenInputIsEmpty()
        {
            // Arrange: create an empty collection of log entries
            List<LogEntry> entries = new List<LogEntry>();

            // Act: calculate statistics for the empty collection
            LogStatistics result = LogStatisticsService.CalculateStatistics(entries);

            // Assert: verify that all statistics are zero and no latest timestamp is returned
            Assert.AreEqual(0, result.TotalCount);
            Assert.AreEqual(0, result.InfoCount);
            Assert.AreEqual(0, result.WarningCount);
            Assert.AreEqual(0, result.ErrorCount);
            Assert.IsNull(result.LatestTimestamp);
        }

        [TestMethod]
        public void CalculateStatistics_ShouldReturnLatestTimestamp()
        {
            // Arrange: create log entries with different timestamps
            List<LogEntry> entries = new List<LogEntry>
            {
                new LogEntry { Timestamp = new DateTime(2026, 7, 23, 12, 30, 00) },
                new LogEntry { Timestamp = new DateTime(2026, 7, 23, 12, 45, 00) },
                new LogEntry { Timestamp = new DateTime(2026, 7, 23, 12, 15, 00) }
            };

            // Act: calculate statistics for the sample log entries
            LogStatistics result = LogStatisticsService.CalculateStatistics(entries);

            // Assert: verify that the latest timestamp is returned
            Assert.AreEqual(
                new DateTime(2026, 7, 23, 12, 45, 00),
                result.LatestTimestamp);
        }

        [TestMethod]
        public void CalculateStatistics_ShouldIgnoreUnknownLogLevels()
        {
            // Arrange: create log entries with known and unknown log levels
            List<LogEntry> entries = new List<LogEntry>
            {
                new LogEntry { Level = "INFO" },
                new LogEntry { Level = "DEBUG" },
                new LogEntry { Level = "TRACE" },
                new LogEntry { Level = "ERROR" }
            };

            // Act: calculate statistics for the sample log entries
            LogStatistics result = LogStatisticsService.CalculateStatistics(entries);

            // Assert: verify that unknown log levels do not affect the calculated statistics
            Assert.AreEqual(4, result.TotalCount);
            Assert.AreEqual(1, result.InfoCount);
            Assert.AreEqual(0, result.WarningCount);
            Assert.AreEqual(1, result.ErrorCount);
        }

        [TestMethod]
        public void CalculateStatistics_ShouldCountOnlyWarningLogEntries()
        {
            // Arrange: create log entries containing only WARNING log levels
            List<LogEntry> entries = new List<LogEntry>
            {
                new LogEntry { Level = "WARNING" },
                new LogEntry { Level = "WARNING" },
                new LogEntry { Level = "WARNING" }
            };

            // Act: calculate statistics for the sample log entries
            LogStatistics result = LogStatisticsService.CalculateStatistics(entries);

            // Assert: verify that only the warning counter is incremented
            Assert.AreEqual(3, result.TotalCount);
            Assert.AreEqual(0, result.InfoCount);
            Assert.AreEqual(3, result.WarningCount);
            Assert.AreEqual(0, result.ErrorCount);
        }
    }
}