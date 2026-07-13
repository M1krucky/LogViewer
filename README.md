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
- Load large log files asynchronously with a loading indicator
- Highlight ERROR and WARNING entries
- Recent Files menu with persistent history
- About window
- Status bar showing the currently opened file

#### Architecture

- WPF (UI)
- C#
- .NET 10
- Service-based architecture
- MVVM migration planned for Version 2

#### Tech Stack

- C#
- .NET 10
- WPF
- LiveCharts2
- Git
- GitHub

#### Project Structure

```text
LogViewer/
├── Models/
├── Services/
│   ├── LogParserService.cs
│   └── LogFilterService.cs
├── ViewModels/ (planned)
├── Views/
├── MainWindow.xaml
├── StatisticsWindow.xaml
├── AboutWindow.xaml
├── App.xaml
└── sample.log
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
- [ ] Export filtered results to CSV
- [ ] Add error trend chart
- [ ] Final UI polish
- [ ] Create installer and first release

#### Status

🚧 **Version 1 in active development (~80–85% complete)**