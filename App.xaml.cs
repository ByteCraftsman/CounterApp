namespace CounterApp
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzUzNDQ5MUAzMjM3MmUzMDJlMzBaVWlKOCtKMWhLQ29qcjhxZ21VOFlYOEV6YkhNK0JDUjl2UEVuNTJtWkZnPQ==");
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
