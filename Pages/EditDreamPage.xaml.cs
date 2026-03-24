using Dream_Journal_Project.Models;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace Dream_Journal_Project.Pages;

[QueryProperty(nameof(DreamId), "DreamId")]


public partial class EditDreamPage : ContentPage
{
	public int DreamId { get; set; }

	private readonly DataBaseService _databaservice;
	private DateTime EditDreamDate;

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

		EditDreamDate = DreamToEdit.DateCreated;

        if (DreamToEdit != null)
		{
			this.Title = DreamToEdit.Title;
			HeaderLabel.Text = DreamToEdit.Title;
			Dream_Title.Text = DreamToEdit.Title;
			Dream_Description.Text = DreamToEdit.Description;

		}
	}
    public async void OnSaveButtonClicked(object sender, EventArgs e)
	{

		

		var updatedDream = new Dream
		{
			Id = DreamId,
			DateCreated = EditDreamDate,
			Title = Dream_Title.Text,
			Description = Dream_Description.Text
		};
		
		await _databaservice.UpdateDream(updatedDream);


		Shell.Current.GoToAsync("..");
    }
}