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

    protected override async void OnAppearing()
    {
        base.OnAppearing();

		var dream = await _databaseservice.GetSpecificDream(DreamId);

		if (dream != null)
		{

			this.Title = dream.Title;
			var DateCreated = dream.DateCreated;
			var description = dream.Description;


			DescriptionLabel.Text = description;
			Date.Text = DateCreated.ToString();


        }
		
	}
}