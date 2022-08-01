using MobileSSHClient.Models;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileSSHClient.ViewModels
{
    public class ShellViewModel : BindableObject
    {
        public Connection Connection { get; set; }

        public SshClient Client { get; set; }

        private ShellStream ShStream { get; set; }

        private string output;
        public string Output { get => output; set { output = value; OnPropertyChanged(); } }

        private string input;
        public string Input
        {
            get => input;
            set
            {
                input = value;
                OnPropertyChanged();
            }
        }

        private string connectionId;
        public string ConnectionId
        {
            get => connectionId;
            set
            {
                connectionId = value;
                LoadConnection(ConnectionId);
            }
        }

        private ConnectionInfo info;

        private string regexExpression;

        public Command RunCommandInShell { get; }

        public ShellViewModel()
        {
            RunCommandInShell = new Command(OnRunCommandInShell);
        }

        public void StartSession()
        {
            if (Client == null)
            {
                AuthenticationMethod authMethod = new PasswordAuthenticationMethod(Connection.Username, Connection.Password);
                info = new ConnectionInfo(Connection.Ip, Connection.Username, authMethod);
                Client = new SshClient(info);
                try
                {
                    Client.Connect();
                }
                catch (Exception ex)
                {
                    App.Current.MainPage.DisplayAlert(ex.GetType().ToString(), ex.Message, "Cancel");
                    Xamarin.Forms.Shell.Current.GoToAsync("..");
                    return;
                }
            }
            IDictionary<TerminalModes, uint> modes = new Dictionary<TerminalModes, uint>();
            modes.Add(TerminalModes.IUTF8, 1);
            ShStream = Client.CreateShellStream("", 0, 0, 0, 0, 10000, modes);
            ShStream.DataReceived += ShStream_OnDataReceived;
            Client.ErrorOccurred += Client_OnErrorOccurred;
            ShStream.ErrorOccurred += ShStream_OnErrorOccurred;
        } 

        private void ShStream_OnErrorOccurred(object sender, ExceptionEventArgs e)
        {
            Output += e.Exception.Message;
        }

        private void Client_OnErrorOccurred(object sender, ExceptionEventArgs e)
        {
            Output += e.Exception.Message;
        }

        private async void ShStream_OnDataReceived(object sender, ShellDataEventArgs e)
        {
            await Task.Run(() => regexExpression = new Regex(@"\x1B\[[^@-~]*[@-~]").Replace(ShStream.Read(), ""));
            Output += regexExpression;
        }

        public async void EndSession()
        {
            Client.Disconnect();
            Client.Dispose();
            await Xamarin.Forms.Shell.Current.GoToAsync("..");
        }

        private async void LoadConnection(string connectionId)
        {
            int id = Convert.ToInt32(connectionId);
            Connection = await App.Database.GetConnectionAsync(id);
            StartSession();
        }

        public void OnRunCommandInShell()
        {
            if (Client.IsConnected && Input != null && Input != string.Empty)
            {
                try
                {
                    ShStream.WriteLine(Input);
                    ShStream.Flush();
                }
                catch (Exception e)
                {
                    Output += e.Message;
                }

                Input = string.Empty;
            }
        }
    }
}
