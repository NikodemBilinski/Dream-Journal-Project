using Dream_Journal_Project.Models;
using Dream_Journal_Project.Pages;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Microcharts;
using SkiaSharp;


namespace Dream_Journal_Project
{
    [QueryProperty(nameof(IncomingDream), "NewDream")]
    [QueryProperty(nameof(DreamId), "DreamId")]
    public partial class MainPage : ContentPage
    {
        public Chart MyChart { get; set; }

        private readonly DataBaseService _databaseService;

        public ObservableCollection<Dream> Dreams { get; set; } = new();

        public int DreamId { get; set; }

        public MainPage(DataBaseService databaseservice)
        {

            InitializeComponent();


            _databaseService = databaseservice;

            this.BindingContext = this;

            //ideas dumpster

            // jak na razie tyle mialem jakis pomysl jeden jeszcze ale mi uciekl, ewentualnie tutaj sobie bede dodawal jakies pomysly co by tu jeszcze mozna bylo zrobic

            //todo check on layout on android and fix it if needed to

            //todo wallpaper or some shit

            //todo ld techniques dokonczyc w koncu

            //todo obczaic light theme czy ma sens i czy dziala na telefonie aby syfu nie bylo

            //todo ustawienia? (ewentualne pomysly?)

            //todo dodac wiecej snow dla testow (bardziej aby sie pobawic jak to by wygladalo przy np 200 snach - uzyc sobie chata aby uzupelnil tabele dreams czy cos

            //Todo na pewno jakis filter snow od razu jak ma byc tyle snow - wyszukiwarka i filtrowanie po np dacie jakby sie udalo

            //todo boczne rozsuwane menu z opcjami typu tagi, wykresy, ustawienia itp i inne, aby nei zaslaniac tytulu 




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

            await Task.Delay(200);

            bool AnyUpdates = await _databaseService.CheckForUpdates();

            if (AnyUpdates)
            {
                bool response = await DisplayAlertAsync("Update Available", "A new version of the app is available! Do you want to download a new version of Dream Journal?", "Yes", "No");
                if (response)
                {
                    // Open the app's page GITHUBBBBBBBB
                    await Launcher.OpenAsync("https://github.com/NikodemBilinski/Dream-Journal-Project/releases/latest");
                }
            }

            await _databaseService.GenerateDefaultTags();

            await RefreshDreams();


        }


        public async void On_Add_Dream_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddDreamPage));

            
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
            Debug.WriteLine($"sny:  {dreamsFromDb.Count}");
            Dreams.Clear();
            foreach (var dream in dreamsFromDb)
            {
                Dreams.Add(dream);
            }
        }

       
        private async void LD_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(LDTechniquesPage));
        }

        private async void Trash_Bin_Clicked(object sender, TappedEventArgs e)
        {
            var element = (VisualElement)sender;

            var tappedDream = (Dream)element.BindingContext; 

            Debug.Write(tappedDream.Title);

            bool response = await DisplayAlertAsync("Delete Dream", $"You sure you want to delete that? '{tappedDream.Title}'?", "Yes", "No");

            if(response)
            {
                await _databaseService.DeleteDream(tappedDream);
                await RefreshDreams();
            }

        }

        private async void Refresh_Button_Clicked(object sender, EventArgs e)
        {
            await RefreshDreams();
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            var element = (VisualElement)sender;
            var tappedDream = (Dream)element.BindingContext; 
            Debug.Write(tappedDream.Title);

            await Shell.Current.GoToAsync($"{nameof(EditDreamPage)}?DreamId={tappedDream.Id}");
            //await Shell.Current.GoToAsync($"{nameof(EditDreamPage)}?Dream={tappedDream}");
        }

        private async void Open_Chart_Page(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ChartPage));
        }

        private async void On_Tags_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(TagPage));
        }
    }
}
