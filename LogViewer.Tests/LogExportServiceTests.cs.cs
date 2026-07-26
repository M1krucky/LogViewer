using LogViewer.Models;
using LogViewer.Services;

namespace LogViewer.Tests
{
    /// <summary>
    /// Contains unit tests for the LogExportService class.
    /// </summary>

    [TestClass]
    public class LogExportServiceTests
    {
        [TestMethod]
        public void ExportToCsv_ShouldCreateCsvFile()
        {
            // Arrange: create a log entry and a temporary file path
            List<LogEntry> entries = new List<LogEntry>
            {
                new LogEntry
                {
                    Timestamp = new DateTime(2026, 7, 26, 10, 00, 00),
                    Level = "INFO",
                    Message = "Application started"
                }
            };

            string filePath = Path.GetTempFileName();

            try
            {
                // Act: export the log entry to a CSV file 
                LogExportService.ExportToCsv(entries, filePath);

                // Assert: verify that the file exists
                Assert.IsTrue(File.Exists(filePath));
            }

            finally
            {
                if (File.Exists(filePath))
                {
                    // Cleanup: remove the temporary file
                    File.Delete(filePath);
                }
            }

        }


        [TestMethod]
        public void ExportToCsv_ShouldWriteCsvHeader()
        {
            // Arrange: create an empty list of log entries and a temporary file path
            List<LogEntry> entries = new List<LogEntry>();

            string filePath = Path.GetTempFileName();

            try
            {
                // Act: export the empty log list to a CSV file
                LogExportService.ExportToCsv(entries, filePath);

                // Assert: verify that the CSV header is written correctly
                string[] lines = File.ReadAllLines(filePath);

                Assert.AreEqual("Timestamp;Level;Message", lines[0]);
            }

            finally
            {
                if (File.Exists(filePath))
                {
                    // Cleanup: remove the temporary file
                    File.Delete(filePath);
                }
            }
        }


        [TestMethod]
        public void ExportToCsv_ShouldWriteLogEntry()
        {
            // Arrange: create a log entry and a temporary file path
            List<LogEntry> entries = new List<LogEntry>
            {
                new LogEntry
                {
                    Timestamp = new DateTime(2026, 7, 26, 10, 00, 00),
                    Level = "INFO",
                    Message = "Application started"
                }
            };

            string filePath = Path.GetTempFileName();

            try
            {
                // Act: export the log entry to a CSV file
                LogExportService.ExportToCsv(entries, filePath);

                // Assert: verify that the exported log entry is correct
                string[] lines = File.ReadAllLines(filePath);

                Assert.AreEqual(2, lines.Length);

                string expectedRow =
                    $"\"{entries[0].Timestamp}\";\"INFO\";\"Application started\"";

                Assert.AreEqual(expectedRow, lines[1]);
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    // Cleanup: remove the temporary file
                    File.Delete(filePath);
                }
            }

        }


        [TestMethod]
        public void ExportToCsv_ShouldWriteMultipleLogEntries()
        {
            // Arrange: create multiple log entries and a temporary file path
            List<LogEntry> entries = new List<LogEntry>
            {
                new LogEntry
                {
                    Timestamp = new DateTime(2026, 7, 26, 10, 00, 00),
                    Level = "INFO",
                    Message = "Application started"
                },
                new LogEntry
                {
                    Timestamp = new DateTime(2026, 7, 26, 10, 05, 00),
                    Level = "ERROR",
                    Message = "Connection failed"
                }
            };

            string filePath = Path.GetTempFileName();

            try
            {
                // Act: export multiple log entries to a CSV file
                LogExportService.ExportToCsv(entries, filePath);

                // Assert: verify that the header and both entries were written
                string[] lines = File.ReadAllLines(filePath);

                Assert.AreEqual(3, lines.Length);
                Assert.IsTrue(lines[1].Contains("\"INFO\";\"Application started\""));
                Assert.IsTrue(lines[2].Contains("\"ERROR\";\"Connection failed\""));
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    // Cleanup: remove the temporary file
                    File.Delete(filePath);
                }
            }
        }


        [TestMethod]
        public void ExportToCsv_ShouldEscapeQuotesInMessage()
        {
            // Arrange: create a log entry containing quotation marks
            List<LogEntry> entries = new List<LogEntry>
        {
            new LogEntry
            {
                Timestamp = new DateTime(2026, 7, 26, 10, 00, 00),
                Level = "INFO",
                Message = "User clicked \"Login\" button"
            }
        };

            string filePath = Path.GetTempFileName();

            try
            {
                // Act: export the log entry to a CSV file
                LogExportService.ExportToCsv(entries, filePath);

                // Assert: verify that quotation marks are escaped correctly
                string[] lines = File.ReadAllLines(filePath);

                Assert.IsTrue(lines[1].Contains("\"\"Login\"\""));
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    // Cleanup: remove the temporary file
                    File.Delete(filePath);
                }
            }
        }


        [TestMethod]
        public void ExportToCsv_ShouldHandleEmptyMessage()
        {
            // Arrange: create a log entry with an empty message
            List<LogEntry> entries = new List<LogEntry>
            {
                new LogEntry
                {
                    Timestamp = new DateTime(2026, 7, 26, 10, 00, 00),
                    Level = "INFO",
                    Message = string.Empty
                }
            };

            string filePath = Path.GetTempFileName();

            try
            {
                // Act: export the log entry to a CSV file
                LogExportService.ExportToCsv(entries, filePath);

                // Assert: verify that an empty message is exported correctly
                string[] lines = File.ReadAllLines(filePath);

                Assert.IsTrue(lines[1].EndsWith(";\"\""));
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    // Cleanup: remove the temporary file
                    File.Delete(filePath);
                }
            }
        }


        [TestMethod]
        public void ExportToCsv_ShouldHandleEmptyLogList()
        {
            // Arrange: create an empty list of log entries
            List<LogEntry> entries = new List<LogEntry>();

            string filePath = Path.GetTempFileName();

            try
            {
                // Act: export the empty log list to a CSV file
                LogExportService.ExportToCsv(entries, filePath);

                // Assert: verify that only the header is written
                string[] lines = File.ReadAllLines(filePath);

                Assert.AreEqual(1, lines.Length);
                Assert.AreEqual("Timestamp;Level;Message", lines[0]);
            }
            finally
            {
                if (File.Exists(filePath))
                {   // Cleanup: remove the temporary file
                    File.Delete(filePath);
                }
            }
        }
    }
}
