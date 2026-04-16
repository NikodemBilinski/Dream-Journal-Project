using Dream_Journal_Project.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Dream_Journal_Project
{
    public partial class App : Application
    {
        private readonly DataBaseService _databaseService;
        public App(DataBaseService databaseservice)
        {
            InitializeComponent();

            _databaseService = databaseservice;


            var currentTheme = Preferences.Default.Get("AppTheme", AppTheme.Unspecified.ToString());

            if(Enum.TryParse(currentTheme, out AppTheme theme))
            {
                Application.Current.UserAppTheme = theme;
            }


        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
   
    }
}