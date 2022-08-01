using System;
using Xamarin.Forms;
using MobileSSHClient.Models;

namespace MobileSSHClient.ViewModels
{
    [QueryProperty(nameof(SnippetId), nameof(SnippetId))]
    [QueryProperty(nameof(ConnectionId), nameof(ConnectionId))]
    public class SnippetEntryViewModel : BindableObject
    {
        public Command SaveSnippetCommand { get; }

        public Snippet Snippet { get; private set; }

        public string SnippetId
        {
            set => LoadSnippet(Uri.UnescapeDataString(value ?? null));
        }

        public string ConnectionId { get; set; }

        private string command;

        public string CommandDisplay
        {
            get => command;
            set
            {
                if (value == command)
                    return;

                command = value;
                OnPropertyChanged();
            }
        }

        private string name;

        public string NameDisplay
        {
            get => name;
            set
            {
                if (value == name)
                    return;

                name = value;
                OnPropertyChanged();
            }
        }

        public SnippetEntryViewModel()
        {
            SaveSnippetCommand = new Command(OnSaveSnippet);
            Snippet = new Snippet();
        }

        private async void LoadSnippet(string snippetId)
        {
            int id = Convert.ToInt32(snippetId);
            Snippet = await App.Database.GetSnippetAsync(id);
            CommandDisplay = Snippet.Command;
            NameDisplay = Snippet.Name;
        }

        private async void OnSaveSnippet()
        {
            Snippet.Command = CommandDisplay ;
            Snippet.Name = NameDisplay;

            if (Snippet.ConnectionId == 0)
            {
                int id = Convert.ToInt32(ConnectionId);
                Snippet.ConnectionId = id;
            }

            if (string.IsNullOrWhiteSpace(Snippet.Name))
            {
                Snippet.Name = Snippet.Command;
            }

            if (!string.IsNullOrWhiteSpace(CommandDisplay))
            {
                App.Database.SaveSnippetAsync(Snippet);
                await Shell.Current.GoToAsync("..");               
            }
        }
    }
}
