using Dream_Journal_Project.Models;
using System.Collections.ObjectModel;

namespace Dream_Journal_Project
{
    [QueryProperty(nameof(IncomingDream), "NewDream")]
    public partial class MainPage : ContentPage
    {


        private readonly DreamService _dreamservice;

        public ObservableCollection<Dream> Dreams { get; set; }


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


        
    }
}
