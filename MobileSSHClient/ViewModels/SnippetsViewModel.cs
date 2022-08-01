using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MobileSSHClient.Models;
using Xamarin.Forms;
using MobileSSHClient.Views;
using Renci.SshNet;
using Shell = Xamarin.Forms.Shell;

namespace MobileSSHClient.ViewModels
{
    public class SnippetsViewModel : BindableObject
    {

        private Snippet seletedSnippet;
        public Snippet SelectedSnippet
        {
            get => seletedSnippet;
            set
            {
                if (value != null)
                {
                    SelectSnippetCommand.Execute(value);
                    value = null;
                }

                seletedSnippet = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Snippet> snippets;
        public ObservableCollection<Snippet> Snippets
        {
            get => snippets;
            set
            {
                if (value == snippets)
                    return;

                snippets = value;
            }
        }

        public ShellViewModel SshShell { get; set; }

        private string connectionId;
        public string ConnectionId
        {
            get => connectionId;
            set
            {
                connectionId = value;
                LoadSnippets(ConnectionId);
            }
        }

        public Command AddSnippetCommand { get; }

        public Command<Snippet> SelectSnippetCommand;

        public SnippetsViewModel()
        {
            Snippets = new ObservableCollection<Snippet>();
            AddSnippetCommand = new Command(OnAddSnippet);
            SelectSnippetCommand = new Command<Snippet>(OnSelectSnippet);
            App.Database.DataChanged += OnDataChanged;
            LoadSnippets(ConnectionId);
        }

        private async void OnAddSnippet()
        {
            await Shell.Current.GoToAsync($"{nameof(SnippetEntryPage)}?{nameof(SnippetEntryViewModel.ConnectionId)}={ConnectionId}");
        }

        private async void OnSelectSnippet(Snippet selectedSnippet)
        {
            string result = await Application.Current.MainPage.DisplayActionSheet("Choose action", "Cancel", null, "Execute", "Copy to input line", "Edit", "Delete");

            switch (result)
            {
                case "Execute":
                    SshShell.Input = selectedSnippet.Command;
                    SshShell.OnRunCommandInShell();
                    break;
                case "Copy to input line":
                    SshShell.Input = selectedSnippet.Command;
                    break;
                case "Edit":
                    if (selectedSnippet != null)
                        await Shell.Current.GoToAsync($"{nameof(SnippetEntryPage)}?{nameof(SnippetEntryViewModel.SnippetId)}={selectedSnippet.Id}");
                    break;
                case "Delete":
                    App.Database.DeleteSnippetAsync(selectedSnippet);
                    break;
            }
        }

        private async void LoadSnippets(string connectionId)
        {
            int id = Convert.ToInt32(connectionId);
            List<Snippet> snippets = await App.Database.GetSnippetsAsync(id);
            Snippets.Clear();
            snippets.ForEach(snippet => Snippets.Add(snippet));
        }        

        private void OnDataChanged(object sender, EventArgs e)
        {
            LoadSnippets(ConnectionId);
        }
    }
}
