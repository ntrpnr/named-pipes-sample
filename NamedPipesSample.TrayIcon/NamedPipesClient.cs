using H.Pipes;
using NamedPipesSample.Common;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace NamedPipesSample.TrayIcon
{
    public class NamedPipesClient : IDisposable
    {
        const string pipeName = "samplepipe";

        private static NamedPipesClient instance;
        private PipeClient<PipeMessage> client;

        public static NamedPipesClient Instance
        {
            get
            {
                return instance ?? new NamedPipesClient();
            }
        }

        private NamedPipesClient()
        {
            instance = this;
        }
        public async Task InitializeAsync()
        {
            if (client != null && client.IsConnected)
                return;

            client = new PipeClient<PipeMessage>(pipeName);
            client.MessageReceived += (sender, args) => OnMessageReceivedAsync(args.Message);
            client.Disconnected += (o, args) => MessageBox.Show("Disconnected from server");
            client.Connected += (o, args) => MessageBox.Show("Connected to server");
            client.ExceptionOccurred += (o, args) => OnExceptionOccurred(args.Exception);

            await client.ConnectAsync();

            await client.WriteAsync(new PipeMessage
            {
                Action = ActionType.SendText,
                Text = "Hello from client",
            });
        }

        public async Task ShowTrayIconAsync()
        {
            await client.WriteAsync(new PipeMessage
            {
                Action = ActionType.ShowTrayIcon
            });
        }

        public async Task HideTrayIconAsync()
        {
            await client.WriteAsync(new PipeMessage
            {
                Action = ActionType.HideTrayIcon
            });
        }

        private void OnMessageReceivedAsync(PipeMessage message)
        {
            switch (message.Action)
            {
                case ActionType.SendText:
                    MessageBox.Show(message.Text);
                    break;
                default:
                    MessageBox.Show($"Method {message.Action} not implemented");
                    break;
            }
        }

        private void OnExceptionOccurred(Exception exception)
        {
            MessageBox.Show($"An exception occured: {exception}");
        }

        public void Dispose()
        {
            if (client != null)
                client.DisposeAsync().GetAwaiter().GetResult();
        }
    }
}