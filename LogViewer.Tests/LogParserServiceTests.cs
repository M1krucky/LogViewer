// -----------------------------------------------------------------------------
// LogParserServiceTests
// -----------------------------------------------------------------------------

using LogViewer.Models;
using LogViewer.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogViewer.Tests
{
    /// <summary>
    /// Contains unit tests for the LogParserService class.
    /// </summary>
    [TestClass]
    public class LogParserServiceTests
    {
        [TestMethod]
        public void ParseLines_ShouldParseMultipleLogEntries()
        {
            // Arrange: create the log parser service and prepare test input
            LogParserService parser = new LogParserService();

            string[] lines =
            {
                "2026-07-23 12:34:56 INFO Application started",
                "2026-07-23 12:35:10 ERROR Database connection failed",
                "2026-07-23 12:36:20 WARNING Low disk space"
            };

            // Act: parse the log lines
            List<LogEntry> result = parser.ParseLines(lines);

            // Assert: verify that the parsed log entries match the expected results
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("INFO", result[0].Level);
            Assert.AreEqual("ERROR", result[1].Level);
            Assert.AreEqual("WARNING", result[2].Level);
        }

        [TestMethod]
        public void ParseLines_ShouldReturnEmptyList_WhenInputIsEmpty()
        {
            // Arrange: create the log parser service and prepare an empty input
            LogParserService parser = new LogParserService();

            string[] lines = Array.Empty<string>();

            // Act: parse the log lines
            List<LogEntry> result = parser.ParseLines(lines);

            // Assert: verify that no log entries are returned
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ParseLines_ShouldSkipInvalidLogLines()
        {
            // Arrange: create the log parser service and prepare an invalid log line
            LogParserService parser = new LogParserService();

            string[] lines =
            {
                "Invalid log line"
            };

            // Act: parse the log lines
            List<LogEntry> result = parser.ParseLines(lines);

            // Assert: verify that invalid log lines are skipped
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ParseLines_ShouldSkipInvalidLinesAndParseValidOnes()
        {
            // Arrange: create the log parser service and prepare valid and invalid log lines
            LogParserService parser = new LogParserService();

            string[] lines =
            {
                "2026-07-23 12:34:56 INFO Application started",
                "Invalid log line",
                "2026-07-23 12:35:10 ERROR Database connection failed"
            };

            // Act: parse the log lines
            List<LogEntry> result = parser.ParseLines(lines);

            // Assert: verify that only valid log entries are returned
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("INFO", result[0].Level);
            Assert.AreEqual("ERROR", result[1].Level);
        }

        [TestMethod]
        public void ParseLines_ShouldParseBracketedLogLevels()
        {
            // Arrange: create the log parser service and prepare log lines with bracketed log levels
            LogParserService parser = new LogParserService();

            string[] lines =
            {
                "2026-07-23 12:34:56 [INFO] Application started",
                "2026-07-23 12:35:10 [ERROR] Database connection failed"
            };

            // Act: parse the log lines
            List<LogEntry> result = parser.ParseLines(lines);

            // Assert: verify that bracketed log levels are parsed correctly
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("INFO", result[0].Level);
            Assert.AreEqual("ERROR", result[1].Level);
        }

        [TestMethod]
        public void ParseLines_ShouldSkipLogLinesWithInvalidTimestamp()
        {
            // Arrange: create the log parser service and prepare log lines with an invalid timestamp
            LogParserService parser = new LogParserService();

            string[] lines =
            {
                "InvalidDate 12:34:56 INFO Application started",
                "2026-07-23 12:35:10 ERROR Database connection failed"
            };

            // Act: parse the log lines
            List<LogEntry> result = parser.ParseLines(lines);

            // Assert: verify that log lines with invalid timestamps are skipped
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("ERROR", result[0].Level);
        }

        [TestMethod]
        public void ParseLines_ShouldParseTimestampAndMessageCorrectly()
        {
            // Arrange: create the log parser service and prepare a valid log line
            LogParserService parser = new LogParserService();

            string[] lines =
            {
                "2026-07-23 12:34:56 INFO Failed to connect to server: timeout after 30 seconds"
            };

            // Act: parse the log lines
            List<LogEntry> result = parser.ParseLines(lines);

            // Assert: verify that the timestamp and message are parsed correctly
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(new DateTime(2026, 7, 23, 12, 34, 56), result[0].Timestamp);
            Assert.AreEqual(
                "Failed to connect to server: timeout after 30 seconds",
                result[0].Message);
        }
    }
}