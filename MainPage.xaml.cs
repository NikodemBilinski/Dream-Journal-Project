using Dream_Journal_Project.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Dream_Journal_Project
{
    [QueryProperty(nameof(IncomingDream), "NewDream")]
    [QueryProperty(nameof(DreamId), "DreamId")]
    public partial class MainPage : ContentPage
    {


        private readonly DataBaseService _databaseService;

        public ObservableCollection<Dream> Dreams { get; set; } = new();

        public int DreamId { get; set; }

        public MainPage(DataBaseService databaseservice)
        {

            InitializeComponent();

            _databaseService = databaseservice;

            this.BindingContext = this;


         
        }

        public Dream IncomingDream
        {
            set
            {
                if (value != null)
                {
                    _databaseService.AddDream(value);
                }
            }
        }

        private async void SaveIncomingDream(Dream newdream)
        {
            await _databaseService.AddDream(newdream);

            Dreams.Add(newdream);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            RefreshDreams();
        }


        public async void OnAddDreamClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddDreamPage));

            
        }

        public void CleanList(object sender, EventArgs e)
        {
            _databaseService.DeleteAllDreams();
            RefreshDreams();
        }


        public async void OnDreamTapped(object sender, EventArgs e)
        {
            var border = (Border)sender;
            var tappedDream = (Dream)border.BindingContext;
            if( tappedDream != null )
            {
                await Shell.Current.GoToAsync($"{nameof(DreamDetailsPage)}?DreamId={tappedDream.Id}");
            }
        }


        private async Task RefreshDreams()
        {
            var dreamsFromDb = await _databaseService.GetDreams();
            Dreams.Clear();
            foreach (var dream in dreamsFromDb)
            {
                Dreams.Add(dream);
            }
        }
        
    }
}
