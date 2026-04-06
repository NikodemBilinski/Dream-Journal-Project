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

        private bool DidFilterApplied = false;
        
        private bool IsFirstLoad = true;

        public ObservableCollection<Dream> Dreams { get; set; } = new();

        private List<Tag> MySelectedTags = new List<Tag>();

        // przysiegam ze kiedys naprawde nasram do turbiny samolotu
        //ktokolwiek to czyta to wiedz ze jschlatt nie mial absolutnie nic wspolnego z wydarzeniami z 11 wrzesnia 2001 roku

        public int DreamId { get; set; }

        public MainPage(DataBaseService databaseservice)
        {

            InitializeComponent();


            _databaseService = databaseservice;

            this.BindingContext = this;

            //ideas dumpster

            //ewentualnie tutaj sobie bede dodawal jakies pomysly co by tu jeszcze mozna bylo zrobic

            //todo check on layout on android and fix it if needed to

            //todo wallpaper or some shit

            //todo ld techniques dokonczyc w koncu

            //todo obczaic light theme czy ma sens i czy dziala na telefonie aby syfu nie bylo

            //todo ustawienia? (ewentualne pomysly?)

            //todo dodac wiecej snow dla testow (bardziej aby sie pobawic jak to by wygladalo przy np 200 snach - uzyc sobie chata aby uzupelnil tabele dreams czy cos

            //todo settings page z opcjami typu zmiana motywu, zarzadzanie tagami, zarzadzanie snami (np masowe usuwanie) itp, (wyclearowanie calej bazy snow)
            //todo moze eksport danych do pliku json czy cos, aby mozna bylo sobie zbackupowac sny przed reinstalem systemu czy cos, a potem wczytac je z powrotem do aplikacji

            //todo about app page (kontakt, github, skrocone readme)

            //todo zastanowic sie czy nie zachowac wybranego filtru po przegladzie konkretnych snow, bo teraz po kliknieciu w filtr i przegladzie snu, filtr sie resetuje

        }

        public Dream IncomingDream
        {
            set
            {
                if (value != null)
                {
                    SaveIncomingDream(value);
                }
            }
        }

        private async void SaveIncomingDream(Dream newdream)
        {
            await _databaseService.AddDream(newdream);

            Dreams.Add(newdream);

            await RefreshDreams();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await Task.Delay(50);

            // Check for updates and load dreams only on the first load of the page
            if (IsFirstLoad)
            {
                IsFirstLoad = false;
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

            

            
            // if filter is selected, dont change the filtered dreams list and selected tag list
            if (!DidFilterApplied)
            {
                var tags = await _databaseService.GetTags();

                TagsSelection.ItemsSource = tags;

                await RefreshDreams();
            }

            


        }


        public async void On_Add_Dream_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddDreamPage));

        }



        public async void OnDreamTapped(object sender, EventArgs e)
        {
            var border = (Border)sender;
            var tappedDream = (Dream)border.BindingContext;
            if (tappedDream != null)
            {
                await Shell.Current.GoToAsync($"{nameof(DreamDetailsPage)}?DreamId={tappedDream.Id}");
            }
        }


        private async Task RefreshDreams()
        {
            //refresh tags too for search

            var tags = await _databaseService.GetTags();
            TagsSelection.ItemsSource = tags;


            //refresh dreams
            var dreamsFromDb = await _databaseService.GetDreams();
            Debug.WriteLine("sny: "+dreamsFromDb.Count);
            Dreams.Clear();
            foreach (var dream in dreamsFromDb)
            {
                Dreams.Add(dream);
            }
        }

        private async void Trash_Bin_Clicked(object sender, TappedEventArgs e)
        {
            var element = (VisualElement)sender;

            var tappedDream = (Dream)element.BindingContext;

            Debug.Write(tappedDream.Title);

            bool response = await DisplayAlertAsync("Delete Dream", $"You sure you want to delete that? '{tappedDream.Title}'?", "Yes", "No");

            if (response)
            {
                await _databaseService.DeleteDream(tappedDream);
                await RefreshDreams();
            }

        }

        private async void Refresh_Button_Clicked(object sender, EventArgs e)
        {
            DidFilterApplied = false;

            MySelectedTags.Clear();

            TitleFilter.Text = string.Empty;

            DateFilterPicker.Date = DateTime.Now;

            await RefreshDreams();
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            var element = (VisualElement)sender;
            var tappedDream = (Dream)element.BindingContext;
            Debug.Write(tappedDream.Title);

            await Shell.Current.GoToAsync($"{nameof(EditDreamPage)}?DreamId={tappedDream.Id}");
        }

        private async void On_Tags_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(TagPage));
        }


        // FILTERING BELOW
        private async void Filter_By_Date(object sender, EventArgs e)
        {
            DidDateChange.IsChecked = true;
        }

        

        private async void Apply_Filters(object sender, EventArgs e)
        {

            var dreamsfromdb = await _databaseService.GetDreams();

            var selectedtags = MySelectedTags;

            var selectedtitle = TitleFilter.Text;

            var selecteddate = DateFilterPicker.Date;

            var filteredDreams = dreamsfromdb.Where(dream =>
            {
                bool matchesTitle = string.IsNullOrWhiteSpace(selectedtitle) || dream.Title.Contains(selectedtitle, StringComparison.OrdinalIgnoreCase);

                bool matchesDate = !DidDateChange.IsChecked || dream.DateCreated.Date == selecteddate;

                bool matchesTags = selectedtags.Count == 0 || (dream.TagIds != null && selectedtags.All(tag => dream.TagIds.Split(",").Contains(tag.Id.ToString())));

                return matchesTitle && matchesDate && matchesTags;
            }).ToList();

            Dreams.Clear();

            foreach(var dream in filteredDreams)
            {
                Dreams.Add(dream);
            }

            DidFilterApplied = true;

            DidDateChange.IsChecked = false;
        }

        private async void Toggle_Filter(object sender, EventArgs e)
        {
            FilterPanel.IsVisible = !FilterPanel.IsVisible;
        }

        private async void Filter_TagSelected(object sender, EventArgs e)
        {
            var border = (Border)sender;
            var border2 = (Border)border.Parent;

            var tag = (Tag)border.BindingContext;

            if (MySelectedTags.Contains(tag))
            {
                MySelectedTags.Remove(tag);
                border2.StrokeThickness = 0;
                border2.Stroke = Colors.Transparent;
            }
            else
            {
                MySelectedTags.Add(tag);
                border2.StrokeThickness = 5;
                border2.Stroke = Colors.GhostWhite;
            }
        }

    }
}
