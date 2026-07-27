// -----------------------------------------------------------------------------
// AboutWindow
// -----------------------------------------------------------------------------

using System.Windows;

namespace LogViewer
{
    /// <summary>
    /// Displays application information in the About dialog.
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
