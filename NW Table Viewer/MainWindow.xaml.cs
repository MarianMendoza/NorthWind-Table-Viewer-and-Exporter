using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NW_Table_Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new LoginPage());
        }

        private void ThemeToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            DockPanel.SetDock(ThemeToggleSwitch, Dock.Right);
            AppThemes.ChangeTheme(new Uri("Themes/darkmode.xaml", UriKind.Relative));
        }

        private void ThemeToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            DockPanel.SetDock(ThemeToggleSwitch, Dock.Left);
            AppThemes.ChangeTheme(new Uri("Themes/lightmode.xaml", UriKind.Relative));

        }

        private void MainFrame_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}