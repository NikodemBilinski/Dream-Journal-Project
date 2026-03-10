namespace Dream_Journal_Project
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddDreamPage), typeof(AddDreamPage));

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

            
        }
    }
}
