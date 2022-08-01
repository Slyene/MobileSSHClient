using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MobileSSHClient.Models;
using Xamarin.Forms;
using MobileSSHClient.Views;

namespace MobileSSHClient.ViewModels
{
    public class SavedConnectionsViewModel : BindableObject
    {
        private Connection selectedConnection;

        public Connection SelectedConnection
        {
            get => selectedConnection;
            set
            {
                if (value != null)
                {
                    SelectConnectionCommand.Execute(value);
                    value = null;
                }

                selectedConnection = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Connection> connections;

        public ObservableCollection<Connection> Connections
        { 
            get => connections;
            set
            {
                if (value == connections)
                    return;

                connections = value;
            }
        }

        public Command AddConnectionCommand { get; }

        public Command<Connection> SelectConnectionCommand { get; }

        public SavedConnectionsViewModel()
        {
            AddConnectionCommand = new Command(OnAddConnection);
            SelectConnectionCommand = new Command<Connection>(OnSelectConnection);
            Connections = new ObservableCollection<Connection>();
            App.Database.DataChanged += OnDataChanged;
            LoadConnections();
        }

        private async void OnAddConnection()
        {
            await Shell.Current.GoToAsync(nameof(ConnectionEntryPage));
        }

        private async void OnSelectConnection(Connection selectedConnection)
        {
            string result = await Application.Current.MainPage.DisplayActionSheet("Choose action", "Cancel", null, "Connect", "Edit", "Delete");

            switch (result)
            {
                case "Connect":
                    await Shell.Current.GoToAsync($"{nameof(ConnectionPage)}?{nameof(ConnectionViewModel.ConnectionId)}={selectedConnection.Id}");
                    break;
                case "Edit":
                    if (selectedConnection != null)
                        await Shell.Current.GoToAsync($"{nameof(ConnectionEntryPage)}?{nameof(ConnectionEntryViewModel.ConnectionId)}={selectedConnection.Id}");
                    break;
                case "Delete":
                    App.Database.DeleteConnectionAsync(selectedConnection);
                    App.Database.DeleteSnippetsByConnectionIdAsync(selectedConnection.Id);
                    break;
            }
        }

        private async void LoadConnections()
        {
            List<Connection> connections = await App.Database.GetConnectionsAsync();
            Connections.Clear();
            connections.ForEach(connection => Connections.Add(connection));
        }

        private void OnDataChanged(object sender, EventArgs e) => LoadConnections();
    }
}
