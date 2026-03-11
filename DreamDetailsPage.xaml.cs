using Dream_Journal_Project.Models;

namespace Dream_Journal_Project;

[QueryProperty(nameof(DreamId), "DreamId")]
public partial class DreamDetailsPage : ContentPage
{
	private readonly DreamService _dreamservice;

	public int DreamId { get; set; }
    public DreamDetailsPage(DreamService dreamService)
	{
		
		InitializeComponent();
		_dreamservice = dreamService;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

		var selectedDream = _dreamservice.Dreams.FirstOrDefault(d => d.Id == DreamId);

		if (selectedDream != null)
		{
			BindingContext = selectedDream;
        }
    }
}