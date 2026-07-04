### LogViewer

A Windows desktop application for viewing and analyzing log files, built with C#, .NET, and WPF.

#### Features

- Parse log files
- Display log entries
- Search and filter log entries by text
- Filter log entries by log level
- Filter log entries by date range
- Sort log entries by timestamp, level, and message
- Open log files from disk
- Display statistics based on currently filtered results
- Display an interactive log level distribution chart using LiveCharts2
- Display future error trend chart by hour/day/week/month
- Support large log files
- Highlight ERROR and WARNING entries
- Export filtered results
- Dark mode
- Keyboard shortcuts

#### Architecture

- WPF (UI)
- C#
- .NET
- MVVM architecture (planned)

#### Tech Stack

- C#
- .NET
- WPF
- Git
- GitHub

#### Project Structure

```text
LogViewer/
├── Models/
├── Services/
├── ViewModels/
├── Views/
├── MainWindow.xaml
├── App.xaml
└── sample.log
```

#### Roadmap

- [x] Create project structure
- [x] Create `LogEntry` model
- [x] Create `LogParserService`
- [x] Display sample log entry
- [x] Read log entries from a file
- [x] Parse multiple log entries
- [x] Display log entries in a table
- [x] Implement text filtering
- [ ] Add level filter
- [ ] Add date range filter
- [ ] Centralize filtering in `ApplyFilters()`
- [ ] Store current filtered results separately
- [x] Implement sorting
- [x] Add statistics window
- [ ] Connect statistics to current filtered results
- [x] Add LiveCharts2 log level distribution chart
- [ ] Connect chart to current filtered results
- [ ] Add future error trend chart by hour/day/week/month
- [x] Support opening log files
- [x] Highlight ERROR and WARNING entries
- [ ] Export filtered results
- [ ] Add dark mode
- [ ] Add keyboard shortcuts
- [ ] Final UI polish

#### Status

🚧 In development