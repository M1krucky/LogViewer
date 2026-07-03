### LogViewer

A Windows desktop application for viewing and analyzing log files, built with C#, .NET, and WPF.

#### Features

- Parse log files
- Display log entries
- Filter log entries by level, date, and text
- Search log messages
- Sort log entries by timestamp, level, and message
- Open log files from disk
- Display log statistics
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
- [x] Implement filtering
- [x] Implement sorting
- [x] Add statistics window
- [ ] Add simple statistics chart
- [x] Support opening log files
- [ ] Highlight ERROR and WARNING entries
- [ ] Export filtered results
- [ ] Add dark mode
- [ ] Add keyboard shortcuts
- [ ] Final UI polish

#### Status

🚧 In development