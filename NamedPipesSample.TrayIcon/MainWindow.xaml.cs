using System.Windows;

namespace NamedPipesSample.TrayIcon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ShowTrayIconBtn_Click(object sender, RoutedEventArgs e)
        {
            await NamedPipesClient.Instance.ShowTrayIconAsync();
        }

        private async void HideTrayIconBtn_Click(object sender, RoutedEventArgs e)
        {
            await NamedPipesClient.Instance.HideTrayIconAsync();
        }
    }
}
