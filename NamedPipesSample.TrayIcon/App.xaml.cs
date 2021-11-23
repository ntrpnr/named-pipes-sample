using Hardcodet.Wpf.TaskbarNotification;
using System.Threading.Tasks;
using System.Windows;

namespace NamedPipesSample.TrayIcon
{
    public partial class App : Application
    {
        private TaskbarIcon notifyIcon;

        public App()
        {
            NamedPipesClient.Instance.InitializeAsync().ContinueWith(t =>
                MessageBox.Show($"Error while connecting to pipe server: {t.Exception}"),
                TaskContinuationOptions.OnlyOnFaulted);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            NamedPipesClient.Instance.Dispose();
            notifyIcon.Dispose();

            base.OnExit(e);
        }
    }
}