using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileSSHClient.ViewModels;

namespace MobileSSHClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectionPage : TabbedPage
    {
        public ConnectionPage()
        {
            InitializeComponent();
        }
    }
}