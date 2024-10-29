namespace CounterApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.CounterPage), typeof(Views.CounterPage));
        }
    }
}
