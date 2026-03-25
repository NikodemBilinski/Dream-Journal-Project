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

	private List<Tag> MySelectedTags = new List<Tag>(); 

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

			var tags = await _databaservice.GetTags();



			

			if (!string.IsNullOrEmpty(DreamToEdit.TagIds))
			{
				var TagsArray = DreamToEdit.TagIds.Split(",", StringSplitOptions.TrimEntries);

				

				foreach (var tag in tags)
				{
					if(TagsArray.Contains(tag.Id.ToString()))
					{
						MySelectedTags.Add(tag);
						tag.CurrentThickness = 5;

                    }

                }

			}

            TagsSelection.ItemsSource = tags;
        }
	}
    public async void OnSaveButtonClicked(object sender, EventArgs e)
	{

		var selectedTags = MySelectedTags;

		string TagsIdsJoin = string.Join(",", selectedTags.Select(x => x.Id));

		

		var updatedDream = new Dream
		{
			Id = DreamId,
			DateCreated = EditDreamDate,
			Title = Dream_Title.Text,
			Description = Dream_Description.Text,
			TagIds = TagsIdsJoin

        };
		
		await _databaservice.UpdateDream(updatedDream);


		Shell.Current.GoToAsync("..");
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		var border = (Border)sender;

		var tag = (Tag)border.BindingContext;

        if (MySelectedTags.Contains(tag))
        {
            MySelectedTags.Remove(tag);
            border.StrokeThickness = 0;
			border.Stroke = Colors.Transparent;
        }
        else
        {
            MySelectedTags.Add(tag);
            border.StrokeThickness = 5;
			border.Stroke = Colors.Black;
        }
    }
}