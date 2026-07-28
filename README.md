### LogViewer

A Windows desktop application for opening, filtering, analyzing, and reviewing log files, built with C#, .NET 10, WPF, and LiveCharts2.

#### Features

- Open log files from disk
- Parse standard and bracketed log formats
- Display log entries in a sortable DataGrid
- Search log entries by text
- Support multi-word AND/OR search
- Filter log entries by log level
- Filter log entries by date range
- Sort log entries by timestamp, level, and message
- Display live statistics based on currently filtered results
- Display a live log level distribution chart using LiveCharts2
- Display an error trend chart with Hour, Day, Week, and Month grouping
- Load large log files asynchronously with a loading indicator
- Highlight ERROR and WARNING entries
- Export filtered log entries to CSV
- Recent Files menu with persistent history
- Status bar showing the currently opened file
- About window
- Comprehensive unit tests for core business logic
- Self-contained Windows installer

#### Installation

1. Open the repository's **Releases** page.
2. Download **LogViewer_Setup_v1.0.exe**.
3. Run the installer.
4. Launch LogViewer from the Windows Start menu or the optional desktop shortcut.

The installer contains the required .NET runtime, so a separate .NET installation is not required.

#### Application Architecture

- Service-oriented architecture
- Layered separation between Models, Services, and Views

#### Tech Stack

- C#
- .NET 10
- WPF
- LiveCharts2
- MSTest

#### Development Tools

- Git
- GitHub
- Inno Setup

#### Testing

##### Automated Testing

The project includes automated unit tests built with MSTest.

Current unit tests cover the core business logic, including:

- Log parsing
- Log filtering
- Statistics calculation
- CSV export

The tests validate business logic, edge cases, and expected behavior of the core services independently from the WPF user interface.

##### Exploratory Testing

The WPF user interface was verified through exploratory testing, including:

- Opening log files
- Searching log entries
- AND/OR text filtering
- Filtering by log level
- Filtering by date range
- Sorting DataGrid columns
- Displaying statistics
- Displaying log level distribution and error trend charts
- Exporting filtered log entries to CSV
- Loading large log files asynchronously
- Error handling for invalid and unsupported log files
- Installing and launching the application through the Windows installer
- Launching the application from the Windows Start menu

#### Project Structure

```text
LogViewer/
├── Installer/
│   └── LogViewer.iss
│
├── LogViewer/
│   ├── Models/
│   ├── Services/
│   │   ├── LogParserService.cs
│   │   ├── LogFilterService.cs
│   │   ├── LogStatisticsService.cs
│   │   ├── LogChartService.cs
│   │   └── LogExportService.cs
│   ├── Views/
│   ├── Themes/
│   ├── MainWindow.xaml
│   ├── StatisticsWindow.xaml
│   ├── AboutWindow.xaml
│   ├── App.xaml
│   ├── sample.log
│   ├── sample_brackets.log
│   └── LogViewer.csproj
│
├── LogViewer.Tests/
│   ├── LogExportServiceTests.cs
│   ├── LogFilterServiceTests.cs
│   ├── LogParserServiceTests.cs
│   ├── LogStatisticsServiceTests.cs
│   └── MSTestSettings.cs
│
└── README.md
```

#### Roadmap

- [x] Create project structure
- [x] Create `LogEntry` model
- [x] Create `LogParserService`
- [x] Create `LogFilterService`
- [x] Read and parse log files
- [x] Support standard and bracketed log formats
- [x] Display log entries in a DataGrid
- [x] Implement text search
- [x] Implement AND/OR search
- [x] Add level filter
- [x] Add date range filter
- [x] Implement sorting
- [x] Add statistics window
- [x] Add LiveCharts2 log level distribution chart
- [x] Support asynchronous file loading
- [x] Add loading indicator
- [x] Add Recent Files with persistence
- [x] Add status bar
- [x] Add About window
- [x] Export filtered results to CSV
- [x] Add error trend chart with time grouping
- [x] Add automated unit tests
- [x] Perform exploratory UI testing
- [x] Final UI polish
- [x] Create a self-contained Windows installer

#### Status

🚀 **Version 1.0 — Feature Complete**

The application has been fully developed, tested, published, and packaged as a self-contained Windows installer.