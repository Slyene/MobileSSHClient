using MobileSSHClient.Data;
using System;
using System.IO;
using Xamarin.Forms;
using MobileSSHClient.Views;

namespace MobileSSHClient
{
    public partial class App : Application
    {
        static MobileSSHDatabase database;

        // Create the database connection as a singleton.
        public static MobileSSHDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new MobileSSHDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MobileSSHDatabase.db3"));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
