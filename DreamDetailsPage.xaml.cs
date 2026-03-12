using Dream_Journal_Project.Models;

namespace Dream_Journal_Project;

[QueryProperty(nameof(DreamId), "DreamId")]
public partial class DreamDetailsPage : ContentPage
{
	private readonly DataBaseService _databaseservice;

	public int DreamId { get; set; }
    public DreamDetailsPage(DataBaseService databaseservice)
	{
		
		InitializeComponent();
		_databaseservice = databaseservice;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

		////var selectedDream = _databaseservice.get;

		//if (selectedDream != null)
		//{
		//	BindingContext = selectedDream;
  //      }
    }
}