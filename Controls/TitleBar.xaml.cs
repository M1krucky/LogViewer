using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LogViewer.Controls
{
    public partial class TitleBar : UserControl
    {
        public TitleBar()
        {
            InitializeComponent();
        }

        // Allow each window to provide its own title
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title),
                typeof(string),
                typeof(TitleBar),
                new PropertyMetadata(string.Empty));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        // Move the window or maximize it with a double-click
        private void TitleBar_MouseLeftButtonDown(
            object sender,
            MouseButtonEventArgs e)
        {
            Window? window = Window.GetWindow(this);

            if (window == null)
            {
                return;
            }

            if (e.ClickCount == 2)
            {
                window.WindowState =
                    window.WindowState == WindowState.Maximized
                        ? WindowState.Normal
                        : WindowState.Maximized;

                return;
            }

            if (e.ButtonState == MouseButtonState.Pressed)
            {
                window.DragMove();
            }
        }

        // Minimize the parent window
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Window? window = Window.GetWindow(this);

            if (window != null)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        // Maximize or restore the parent window
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            Window? window = Window.GetWindow(this);

            if (window != null)
            {
                window.WindowState =
                    window.WindowState == WindowState.Maximized
                        ? WindowState.Normal
                        : WindowState.Maximized;
            }
        }

        // Close the parent window
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }
    }
}