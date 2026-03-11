using Dream_Journal_Project.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Dream_Journal_Project
{
    [QueryProperty(nameof(IncomingDream), "NewDream")]
    [QueryProperty(nameof(DreamId), "DreamId")]
    public partial class MainPage : ContentPage
    {


        private readonly DreamService _dreamservice;

        public ObservableCollection<Dream> Dreams { get; set; }

        public int DreamId { get; set; }

        public Dream IncomingDream
        {
            set
            {
                if(value != null)
                {
                    value.Id = _dreamservice.Dreams.Count + 1;
                    _dreamservice.Dreams.Add(value);
                }
            }
        }

        

        public int DreamCount_Id => Dreams.Count + 1;
        public MainPage(DreamService dreamservice)
        {

            _dreamservice = dreamservice;

            Dreams = _dreamservice.Dreams;
            
            InitializeComponent();

            this.BindingContext = this;


         
        }

        public async void OnAddDreamClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddDreamPage));

            
        }

        public void CleanList(object sender, EventArgs e)
        {
            _dreamservice.Dreams.Clear();
            
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

        
    }
}
