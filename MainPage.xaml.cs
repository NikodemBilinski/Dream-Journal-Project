using Dream_Journal_Project.Models;
using System.Collections.ObjectModel;

namespace Dream_Journal_Project
{
    [QueryProperty(nameof(IncomingDream), "NewDream")]
    public partial class MainPage : ContentPage
    {
        public Dream IncomingDream
        {
            set
            {
                if(value != null)
                {
                    value.Id = DreamCount_Id;

                    Dreams.Add(value);
                }
            }
        }

        public ObservableCollection<Dream> Dreams { get; set; } = new();

        public int DreamCount_Id => Dreams.Count + 1;
        public MainPage()
        {
            
        InitializeComponent();

            if(Dreams.Count == 0)
            {
                Dreams.Add(new Dream(1, "nasralem w turbine samolotu", "naprawde nasralem przysiegam"));
            }

            this.BindingContext = this;

        }

        public async void OnAddDreamClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddDreamPage));

            
        }

        public async void CleanList(object sender, EventArgs e)
        {
            Dreams.Clear();
        }


        
    }
}
