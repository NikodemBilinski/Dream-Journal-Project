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

            var entries = new[]
            {
                new ChartEntry(10)
                {
                    Label = "Lucid",
                    ValueLabelColor = SKColors.White,
                    ValueLabel = "10",
                    Color = SKColor.Parse("#FF0000")
                },
                new ChartEntry(20)
                {
                    Label = "Non-Lucid",
                    ValueLabelColor = SKColors.White,
                    ValueLabel = "20",
                    Color = SKColor.Parse("#00FF00")
                }
            };

            MyChart = new DonutChart
            {
                Entries = entries,
                BackgroundColor = SKColors.Empty
            };

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

        //public async void CleanList(object sender, EventArgs e)
        //{
        //    bool response = await DisplayAlertAsync("u sure?,", "are you sure?", "Yes", "No");

            

        //    if (response)
        //    {
        //        _databaseService.DeleteAllDreams();
        //        RefreshDreams();
        //    }
            
        //}


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
    }
}
