using System;
using Xamarin.Forms;
using MobileSSHClient.Models;

namespace MobileSSHClient.ViewModels
{
    [QueryProperty(nameof(ConnectionId), nameof(ConnectionId))]
    public class ConnectionEntryViewModel : BindableObject
    {
        public Command SaveConnectionCommand { get; }

        public Connection Connection { get; private set; }

        public string ConnectionId
        {
            set => LoadConnection(Uri.UnescapeDataString(value ?? null));
        }

        private string ip;

        public string IpDisplay
        {
            get => ip;
            set
            {
                if (value == ip)
                    return;

                ip = value;
                OnPropertyChanged();
            }
        }

        private string username;

        public string UsernameDisplay
        {
            get => username;
            set
            {
                if (value == username)
                    return;

                username = value;
                OnPropertyChanged();
            }
        }

        private string password;

        public string PasswordDisplay
        {
            get => password;
            set
            {
                if (value == password)
                    return;

                password = value;
                OnPropertyChanged();
            }
        }

        public ConnectionEntryViewModel()
        {
            SaveConnectionCommand = new Command(OnSaveConnection);
            Connection = new Connection();
        }

        private async void LoadConnection(string connectionId)
        {
            int id = Convert.ToInt32(connectionId);
            Connection = await App.Database.GetConnectionAsync(id);
            IpDisplay = Connection.Ip;
            UsernameDisplay = Connection.Username;
            PasswordDisplay = Connection.Password;
        }

        private async void OnSaveConnection()
        {
            Connection.Ip = IpDisplay;
            Connection.Username = UsernameDisplay;
            Connection.Password = PasswordDisplay;

            if (!string.IsNullOrWhiteSpace(IpDisplay) && !string.IsNullOrWhiteSpace(UsernameDisplay) && !string.IsNullOrWhiteSpace(PasswordDisplay))
            {
                App.Database.SaveConnectionAsync(Connection);
                await Shell.Current.GoToAsync("..");               
            }
        }
    }
}
