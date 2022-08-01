using MobileSSHClient.Views;
using Xamarin.Forms;

namespace MobileSSHClient
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ConnectionPage), typeof(ConnectionPage));
            Routing.RegisterRoute(nameof(ConnectionEntryPage), typeof(ConnectionEntryPage));
            Routing.RegisterRoute(nameof(ShellPage), typeof(ShellPage));
            Routing.RegisterRoute(nameof(SnippetEntryPage), typeof(SnippetEntryPage));
        }
    }
}
