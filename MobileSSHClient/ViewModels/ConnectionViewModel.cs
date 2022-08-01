using System;
using Xamarin.Forms;
using Renci.SshNet;

namespace MobileSSHClient.ViewModels
{
    [QueryProperty(nameof(ConnectionId), nameof(ConnectionId))]
    public class ConnectionViewModel : BindableObject
    {
        private ShellViewModel shellViewModel;
        public ShellViewModel ShellViewModel { get => shellViewModel; set { shellViewModel = value; OnPropertyChanged(); } }

        private SnippetsViewModel snippetsViewModel;
        public SnippetsViewModel SnippetsViewModel { get => snippetsViewModel; set { snippetsViewModel = value; OnPropertyChanged(); } }

        public SshClient Client { get; set; }

        private string connectionId;
        public string ConnectionId
        {
            get => connectionId;
            set
            {
                connectionId = Uri.UnescapeDataString(value ?? null);
                ShellViewModel.ConnectionId = connectionId;
                SnippetsViewModel.ConnectionId = connectionId;
                Client = ShellViewModel.Client;
                SnippetsViewModel.SshShell = ShellViewModel;
            }
        }

        public Command DisconnectCommand { get; }

        public ConnectionViewModel()
        {
            DisconnectCommand = new Command(OnDisconnect);

            ShellViewModel = new ShellViewModel();
            SnippetsViewModel = new SnippetsViewModel();
        }

        private void OnDisconnect()
        {
            ShellViewModel.EndSession();
        }
    }
}