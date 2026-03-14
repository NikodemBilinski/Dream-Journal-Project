using Dream_Journal_Project.Pages;

namespace Dream_Journal_Project
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddDreamPage), typeof(AddDreamPage));

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

            Routing.RegisterRoute(nameof(DreamDetailsPage), typeof(DreamDetailsPage));

            Routing.RegisterRoute(nameof(LDTechniquesPage), typeof(LDTechniquesPage));

            
        }
    }
}
