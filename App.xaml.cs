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

            
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
   
    }
}