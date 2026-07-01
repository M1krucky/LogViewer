using LogViewer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LogViewer
{
    /// <summary>
    /// Displays summary statistics for the loaded log entries.
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        public StatisticsWindow(List<LogEntry> logEntries)  // constructor that receives the loaded log entries
        {
            InitializeComponent();

            StatisticsTextBlock.Text = $"Total log entries: {logEntries.Count}";
        }
    }
}
