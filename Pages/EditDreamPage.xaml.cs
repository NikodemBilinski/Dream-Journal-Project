using Dream_Journal_Project.Models;
using System.Diagnostics;

namespace Dream_Journal_Project.Pages;

[QueryProperty(nameof(DreamId), "DreamId")]


public partial class EditDreamPage : ContentPage
{
	public int DreamId { get; set; }

	private readonly DataBaseService _databaservice;

    public EditDreamPage(DataBaseService databaseservice)
	{
		InitializeComponent();

		_databaservice = databaseservice;


        Debug.WriteLine(DreamId + ": dream id");




    }

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		Debug.WriteLine(DreamId + ": dream id in onappearing");
        var DreamToEdit = await _databaservice.GetSpecificDream(DreamId);

		if (DreamToEdit != null)
		{
			this.Title = DreamToEdit.Title;
			Dream_Description.Text = DreamToEdit.Description;

		}
	}
    public async Task OnSaveButtonClicked(object sender, EventArgs e)
	{
		var updatedDream = new Dream
		{
			Id = DreamId,
			Description = Dream_Description.Text
            /*Description = DescriptionEditor.Text,
			DateCreated = DateTime.Now,
			LucidDream = LucidSwitch.IsToggled*/
        };
		
		await _databaservice.UpdateDream(updatedDream);


		Shell.Current.GoToAsync("..");
    }
}