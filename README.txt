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
- [ ] Read log entries from a file
- [ ] Parse multiple log entries
- [ ] Display log entries in a table
- [ ] Implement filtering
- [ ] Implement sorting
- [ ] Add statistics panel
- [ ] Support opening log files
- [ ] Highlight ERROR and WARNING entries
- [ ] Export filtered results
- [ ] Add dark mode
- [ ] Add keyboard shortcuts
- [ ] Final UI polish

#### Status

🚧 In development